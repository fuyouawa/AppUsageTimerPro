using System;
using System.Diagnostics;
using System.Windows;
using AppUsageTimerPro.Tools;
using Serilog;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // 设置最低日志级别
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // 输出到文件，按天滚动
                .CreateLogger();

            ForceScanner.Instance.Initialize();
        }
    }
}
