namespace TasksTracker.Common;

public static class Strings {

    public static string
    Quoted(this string? value) => value != null ? $"\"{value}\"" : string.Empty;

    public static string
    Unquoted(this string? value) =>
        value.IsEmpty() ? string.Empty : value!.Trim('"');

    public static bool
    IsEmpty(this string? @string) => string.IsNullOrWhiteSpace(@string);
    
    public static bool
    IsNotEmpty(this string? @string) => !string.IsNullOrWhiteSpace(@string);

    public static string
    Remove(this string @string, string substring) => @string.Replace(substring, "");

    public static string
    TakeAfter(this string @string, char delimiter = ' ') =>
        @string.Substring(@string.LastIndexOf(delimiter) + 1);
}