using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AppUsageTimerPro
{
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

        public event EventHandler<SuccessAddTimerEventArgs>? SuccessAddTimerHandler;

        public void ClearText()
        {
            TxtTimerName.Text = string.Empty;
            TxtTagTag.Text = string.Empty;
            ListenedProcessesGrid.ErrorOccurHandler += (sender, err) =>
            {
                ErrorOccur(err);
            };
        }

        private void ErrorOccur(string err)
        {
            TxtError.Text = err;
            SoundsManager.PlayError();
        }

        private void OnClickOk(object sender, RoutedEventArgs e)
        {
            var trimedName = TxtTimerName.Text.Trim();
            var trimedTag = TxtTagTag.Text.Trim();
            if (trimedName.Length == 0)
            {
                ErrorOccur("计时器名称不能为空!");
                return;
            }
            else if (trimedName.Length > 16)
            {
                ErrorOccur("计时器名称不能超过16个字!");
                return;
            }
            if (trimedTag.Length == 0)
            {
                ErrorOccur("标签不能为空!");
                return;
            }
            else if (trimedTag.Length > 16)
            {
                ErrorOccur("标签不能不能超过16个字!");
                return;
            }
            TxtError.Text = "";
            SuccessAddTimerHandler?.Invoke(this, new SuccessAddTimerEventArgs(new TimerItem(trimedName, trimedTag, ListenedProcessesGrid.Model.Collection.ToList())));
        }
    }
}
