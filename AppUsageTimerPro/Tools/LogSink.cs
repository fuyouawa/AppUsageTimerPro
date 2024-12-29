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

            if (logEvent.Level == LogEventLevel.Error)
            {
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show($"{logEvent.MessageTemplate.Text}\n详细消息见日志:{DataManager.Instance.LogSaveDir}", logEvent.Level.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}