namespace TasksTracker.Common;

public static class Numbers {
    private const float DefaultFloatDeviation = 0.000001f;

    public static bool
    IsLessZero(this float value, float deviation = DefaultFloatDeviation) => value < -deviation;

    public static bool
    IsLessZero(this double value, float deviation = DefaultFloatDeviation) => value < -deviation;
    
    public static bool
    IsZero(this long value) => value == 0;
} 