using System.IO;
using System.Windows;
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
                .WriteTo.File(
                    Path.Combine(DataManager.Instance.LogSaveDir, "log.txt"),
                    rollingInterval: RollingInterval.Day) // 输出到文件，按天滚动
                .WriteTo.Sink(new LogSink())
                .CreateLogger();

            LogicManager.Instance.Initialize();
        }
    }
}