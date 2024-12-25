using System;
using System.Windows;
using Serilog;

namespace AppUsageTimerPro;

public static class ErrorHelper
{
    public static void DataBreak(string path)
    {
        Log.Fatal($"{path}数据损坏，尝试运行Helper.exe进行修复，或者联系开发者");
        Environment.Exit(1);
    }
}