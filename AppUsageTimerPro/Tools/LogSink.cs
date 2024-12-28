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

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show($"{logEvent.MessageTemplate.Text}\n详细消息见日志:{DataManager.Instance.LogSaveDir}", logEvent.Level.ToString(), MessageBoxButton.OK, logEvent.Level switch
                {
                    LogEventLevel.Verbose => MessageBoxImage.Information,
                    LogEventLevel.Debug => MessageBoxImage.Information,
                    LogEventLevel.Information => MessageBoxImage.Information,
                    LogEventLevel.Warning => MessageBoxImage.Warning,
                    LogEventLevel.Error => MessageBoxImage.Error,
                    LogEventLevel.Fatal => MessageBoxImage.Error,
                    _ => throw new ArgumentOutOfRangeException()
                });
            });

            if (logEvent.Level == LogEventLevel.Fatal)
            {
                throw new Exception(logEvent.MessageTemplate.Text, logEvent.Exception);
            }
        }
    }
}