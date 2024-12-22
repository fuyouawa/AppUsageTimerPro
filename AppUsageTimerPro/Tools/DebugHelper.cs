using System.Runtime.CompilerServices;
using System.Windows;
using Serilog;

namespace AppUsageTimerPro.Tools;

public static class DebugHelper
{
    public static void Assert(bool condition, string message = "Assert failed", 
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (!condition)
        {
            Log.Error($"[{filePath}:{memberName}:{lineNumber}] {message}");
        }
    }
}