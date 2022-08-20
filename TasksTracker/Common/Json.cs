using System.Diagnostics.CodeAnalysis;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TasksTracker.Common;

public static class Json {

    public static string
    ToJson(this object @object, Formatting formatting = Formatting.Indented) => JsonConvert.SerializeObject(@object, formatting);

    public static T
    ParseJson<T>(this string json) => JsonConvert.DeserializeObject<T>(json)
        ?? throw new InvalidOperationException($"Failed to parse json: {json}");

    public static object
    ParseJson(this string json, Type type) =>
        JsonConvert.DeserializeObject(json, type)
           ?? throw new InvalidOperationException($"Failed to parse json: {json}");

    public static T
    ApplyJsonPatch<T>(this T @object, string patch) where T : notnull =>
        new JsonDiffPatch().Patch(@object.ToJson(), patch).ParseJson<T>();

    public static string
    GetJsonDifference<T>(this T left, T right) where T : notnull =>
        new JsonDiffPatch().Diff(left.ToJson(), right.ToJson());

    public static JObject
    ParseJObject(this string @string) => JObject.Parse(@string);

    public static bool
    TryGetString(this JObject jObject, string property, [MaybeNullWhen(false)] out string result) =>
        jObject.TryGet(property, out result);

    public static bool
    TryGet<T>(this JObject jObject, string property, [MaybeNullWhen(false)] out T result) {
        result = default;
        if (!jObject.TryGetValue(property, StringComparison.Ordinal, out var jToken))
            return false;

        if (jToken.Type == JTokenType.String) {
            if (typeof(T) == typeof(string)) {
                result = jToken.ToObject<T>();
                return result != null;
            }
            else {
                result = JsonConvert.DeserializeObject<T>(jToken.ToObject<string>() ?? string.Empty);
                return result != null;
            }
        }

        result = jToken.ToObject<T>();
        return result != null;
    }
}