using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppUsageTimerPro.Tools
{
    public static class Extensions
    {
        public static string ToTimeString(this TimeSpan span)
        {
            return $"{(long)span.TotalHours}时{span.Minutes:D2}分{span.Seconds:D2}秒";
        }
    }
}
