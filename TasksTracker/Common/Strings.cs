namespace TasksTracker.Common;

public static class Strings {

    public static string
    Quoted(this string? value) => value != null ? $"\"{value}\"" : string.Empty;

    public static string
    Unquoted(this string? value) =>
        value.IsEmpty() ? string.Empty : value!.Trim('"');

    public static bool
    IsEmpty(this string? value) => string.IsNullOrWhiteSpace(value);

    public static bool
    IsNotEmpty(this string? value) => !string.IsNullOrWhiteSpace(value);

    public static string
    Remove(this string value, string substring) => value.Replace(substring, "");

    public static string
    TakeAfter(this string value, char delimiter = ' ') =>
        value[(value.LastIndexOf(delimiter) + 1)..];
}