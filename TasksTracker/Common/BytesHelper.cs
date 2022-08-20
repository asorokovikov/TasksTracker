using System.Text;

namespace TasksTracker.Common;

public static class BytesHelper {
    public static string
    ToUtf8String(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

    public static string
    ToBase64String(this byte[] bytes) => Convert.ToBase64String(bytes);

    public static byte[]
    Utf8ToBytes(this string @string) => Encoding.UTF8.GetBytes(@string);

    public static MemoryStream
    ToMemoryStream(this byte[] bytes) => new(bytes);
}