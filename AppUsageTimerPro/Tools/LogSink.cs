using System.Windows;
using Serilog.Core;
using Serilog.Events;

namespace AppUsageTimerPro.Tools;

public class LogSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        // 将日志消息格式化为字符串
        string message = logEvent.RenderMessage();

        // 弹出消息框（此处为了避免阻塞，建议运行在非主线程时使用调度器）
        Application.Current.Dispatcher.Invoke(() =>
        {
            MessageBox.Show(message, logEvent.Level.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
        });
    }
}