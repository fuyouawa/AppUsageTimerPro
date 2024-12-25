﻿using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;

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
            ListenedAppsGrid.ErrorOccurHandler += (sender, err) =>
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

            if (trimedName.Length > 16)
            {
                ErrorOccur("计时器名称不能超过16个字!");
                return;
            }
            if (trimedTag.Length == 0)
            {
                ErrorOccur("标签不能为空!");
                return;
            }

            if (trimedTag.Length > 16)
            {
                ErrorOccur("标签不能不能超过16个字!");
                return;
            }

            var apps = ListenedAppsGrid.Model.Collection.ToList();
            if (apps.Count == 0)
            {
                ErrorOccur("必须有至少一个监听的应用!");
                return;
            }
            TxtError.Text = "";
            SuccessAddTimerHandler?.Invoke(this, new SuccessAddTimerEventArgs(new TimerItem(trimedName, trimedTag, apps)));
        }
    }
}
