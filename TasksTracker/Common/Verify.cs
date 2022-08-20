using System.Diagnostics.CodeAnalysis;

namespace TasksTracker.Common;

public static class Verify {
    public static T
    VerifyNotNull<T>(this T? value)  {
        if (value == null)
            throw new ArgumentNullException($"Expected object of type {typeof(T).Name} is not null");
        return value;
    }

    public static T
    VerifyType<T>(this object @object) {
        if (!(@object is T result))
            throw new InvalidOperationException($"Expected object of type {typeof(T).Name} but was {@object.GetType().Name}");
        return result;
    }

    public static TEnum
    VerifyType<TEnum>(this string? value, string? argumentName = null) where TEnum : struct, Enum {
        if (!Enum.TryParse<TEnum>(value, ignoreCase: true, out var result))
            throw new ArgumentException($"Value {argumentName} is invalid");
        return result;
    }

    public static string
    VerifyNotEmpty(this string value, string? argumentName = null) {
        if (string.IsNullOrWhiteSpace(value))
            ThrowHelper.ThrowArgumentEmptyException(value, argumentName);
        return value;
    }

    public static string
    VerifyNotNullOrEmpty(this string? value, string? argumentName = null) {
        if (string.IsNullOrWhiteSpace(value))
            ThrowHelper.ThrowArgumentEmptyException(value, argumentName);
        return value;
    }

    public static string
    VerifyLengthLessOrEqual(this string value, int length, string? argumentName = null) {
        if (value.Length > length) {
            throw new ArgumentOutOfRangeException($"Expecting length of {argumentName.Quoted()} to be less or equal " +
                $"that {length} but was {value.Length}");
        }
        return value;
    }

    public static Guid
    VerifyNotEmpty(this Guid? value, string? argumentName = null) {
        if (value == null || value.Value == Guid.Empty)
            ThrowHelper.ThrowArgumentEmptyException(value, argumentName);
        return value.Value;
    }

    public static int
    VerifyGreaterZero(this int? number, string? argument = null) =>
        number is > 0 ? number.Value : throw new ArgumentOutOfRangeException(argument);

    public static int
    VerifyGreaterOrEqualZero(this int value, string? argumentName = null) {
        if (value < 0)
            ThrowHelper.ThrowArgumentGreaterOrEqualZeroException(value, argumentName);
        return value;
    }

    public static float
    VerifyGreaterOrEqualZero(this float value, string? argumentName = null) {
        if (value.IsLessZero())
            ThrowHelper.ThrowArgumentGreaterOrEqualZeroException(value, argumentName);
        return value;
    }

    public static int
    VerifyGreaterZero(this int value, string? argumentName = null) {
        if (value <= 0)
            ThrowHelper.ThrowArgumentGreaterZeroException(value, argumentName);
        return value;
    }

    public static double
    VerifyGreaterOrEqualZero(this double value, string? argumentName = null) {
        if (value.IsLessZero())
            ThrowHelper.ThrowArgumentGreaterOrEqualZeroException(value, argumentName);
        return value;
    }
}

internal static class ThrowHelper {
    [DoesNotReturn]
    internal static void ThrowArgumentGreaterOrEqualZeroException<T>(T value, string? argumentName = null) =>
        throw new ArgumentOutOfRangeException($"Expecting value {argumentName} to be greater or equal zero but was {value}");

    [DoesNotReturn]
    internal static void ThrowArgumentGreaterZeroException<T>(T value, string? argumentName = null) =>
        throw new ArgumentOutOfRangeException($"Expecting value {argumentName} to be greater zero but was {value}");

    [DoesNotReturn]
    internal static void ThrowArgumentEmptyException<T>(T value, string? argumentName = null) =>
        throw new ArgumentException($"Value {argumentName} is not expected to be empty");
}