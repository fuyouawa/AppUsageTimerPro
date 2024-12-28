using System;
using System.Windows;
using Serilog.Core;
using Serilog.Events;

namespace AppUsageTimerPro
{
    public class LogSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level != LogEventLevel.Error && logEvent.Level != LogEventLevel.Fatal)
                return;

            // 将日志消息格式化为字符串
            string message = logEvent.RenderMessage();

            // 弹出消息框（此处为了避免阻塞，建议运行在非主线程时使用调度器）
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                var res = MessageBox.Show(message, logEvent.Level.ToString(), MessageBoxButton.OK, logEvent.Level switch
                {
                    LogEventLevel.Verbose => MessageBoxImage.Information,
                    LogEventLevel.Debug => MessageBoxImage.Information,
                    LogEventLevel.Information => MessageBoxImage.Information,
                    LogEventLevel.Warning => MessageBoxImage.Warning,
                    LogEventLevel.Error => MessageBoxImage.Error,
                    LogEventLevel.Fatal => MessageBoxImage.Error,
                    _ => throw new ArgumentOutOfRangeException()
                });
                if (res == MessageBoxResult.OK)
                {
                    Environment.Exit(1);
                }
            });
        }
    }
}