using AppUsageTimerPro;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppUsageTimerPro
{
    public enum MessageType
    {
        Info,
        Warning,
        Error
    }

    public class TypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageType type)
            {
                var icon = type switch
                {
                    MessageType.Info => new PackIconCodicons() { Kind = PackIconCodiconsKind.Info },
                    MessageType.Warning => new PackIconCodicons() { Kind = PackIconCodiconsKind.Warning },
                    _ => new PackIconCodicons() { Kind = PackIconCodiconsKind.Error }
                };
                icon.VerticalAlignment = VerticalAlignment.Center;
                icon.Height = PopupMessageBox.IconWidth;
                icon.Width = PopupMessageBox.IconWidth;
                return icon;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// PopupMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class PopupMessageBox : Flyout, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public static readonly double IconWidth = 30;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PopupMessageBox()
        {
            InitializeComponent();
            DataContext = this;
        }


        private MessageType _msgType;

        public MessageType MsgType
        {
            get { return _msgType; }
            set { _msgType = value; OnPropertyChanged(); }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsAutoCloseEnabled)
            {
                var originMsg = TxtMessage.Text;

                var formatter = delegate (long milliseconds)
                {
                    double seconds = milliseconds / 1000.0;
                    return string.Format("{0:F1}", seconds);
                };

                TxtMessage.Text = formatter(AutoCloseInterval);
                AdjustPosition(ActualWidth);
                for (int i = 0; i < AutoCloseInterval / 100; i++)
                {
                    await Task.Delay(100);
                    TxtMessage.Text = $"{originMsg} ({formatter(AutoCloseInterval - i * 100)}s)";
                }
                TxtMessage.Text = originMsg;
            }
        }


        private void AdjustPosition(double newWidth)
        {
            Panel.Margin = new Thickness((newWidth - TxtMessage.ActualWidth - IconWidth - 10) / 2 - 60, 0, 0, 0);
        }
    }
}
