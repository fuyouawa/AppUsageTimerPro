using AppUsageTimerPro.Model;
using AppUsageTimerPro.Utils;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace AppUsageTimerPro.View.MainWindow.Timer
{
    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && parameter is FrameworkElement elem)
            {
                return width - elem.ActualHeight - 3;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SuccessAddTimerEventArgs: EventArgs
    {
        public TimerItem Item { get; internal set; }

        public SuccessAddTimerEventArgs(TimerItem item)
        {
            Item = item;
        }
    }


    /// <summary>
    /// AddTimerDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddTimerDialog : CustomDialog
    {
        public AddTimerDialog()
        {
            InitializeComponent();
        }

        public event EventHandler<SuccessAddTimerEventArgs>? SuccessAddTimerEvent;

        public void ClearText()
        {
            tbTimerName.Text = string.Empty;
            tbTagTag.Text = string.Empty;
            tbAppName.Text = string.Empty;
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "选择要监听的应用程序",
                Filter = "应用程序(*.exe)|*.exe"
            };
            if (dialog.ShowDialog() == true)
            {
                tbAppName.Text = dialog.FileName;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            var trimedName = tbTimerName.Text.Trim();
            var trimedTag = tbTagTag.Text.Trim();
            if (trimedName.Length == 0)
            {
                tbError.Text = "计时器名称不能为空!";
                goto error;
            }
            else if (trimedName.Length > 16)
            {
                tbError.Text = "计时器名称不能超过16个字!";
                goto error;
            }
            if (trimedTag.Length == 0)
            {
                tbError.Text = "标签不能为空!";
                goto error;
            }
            else if (trimedTag.Length > 16)
            {
                tbError.Text = "标签不能不能超过16个字!";
                goto error;
            }
            if(!File.Exists(tbAppName.Text))
            {
                tbError.Text = "应用程序路径无效!";
                goto error;
            }
            tbError.Text = "";
            SuccessAddTimerEvent?.Invoke(this, new SuccessAddTimerEventArgs(new TimerItem(trimedName, trimedTag, tbAppName.Text)));
            return;
        error:
            SoundsManager.PlayError();
            return;
        }
    }
}
