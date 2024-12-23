﻿using System;

namespace AppUsageTimerPro
{
    public static class Extensions
    {
        public static string ToTimeString(this TimeSpan span)
        {
            return $"{(long)span.TotalHours}时{span.Minutes:D2}分{span.Seconds:D2}秒";
        }
    }
}
