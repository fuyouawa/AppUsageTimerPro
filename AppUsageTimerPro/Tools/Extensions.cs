using System;

namespace AppUsageTimerPro
{
    public static class Extensions
    {
        public static string ToTimeString(this TimeSpan span)
        {
            return $"{(long)span.TotalHours}:{span.Minutes:D2}:{span.Seconds:D2}";
        }
        public static string ToDisplayTimeString(this TimeSpan span)
        {
            return $"{Math.Floor(span.TotalHours):00}:{span.Minutes:00}:{span.Seconds:00}.{span.Milliseconds / 10:00}";
        }
    }
}
