using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using Serilog;

namespace AppUsageTimerPro
{
    public static class DebugHelper
    {
        public static void Assert([DoesNotReturnIf(false)] bool condition, string message = "Assert failed", 
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            if (!condition)
            {
                var file = Path.GetFileName(filePath);
                Log.Error($"|{file}:{memberName}:{lineNumber}| {message}");
            }
        }
    }
}