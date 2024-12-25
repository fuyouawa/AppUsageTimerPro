using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace AppUsageTimerPro
{
    /// <summary>
    /// ListenedAppsGrid.xaml 的交互逻辑
    /// </summary>
    public partial class ListenedAppsGrid : UserControl
    {
        public event EventHandler<string>? ErrorOccurHandler;

        public ListenedAppsViewModel Model => (ListenedAppsViewModel)DataContext;

        public ListenedAppsGrid()
        {
            InitializeComponent();
        }

        private void OnClickOpenAppName(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "选择要监听的应用",
                Filter = "应用程序(*.exe)|*.exe"
            };
            if (dialog.ShowDialog() == true)
            {
                TxtListenAppName.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private void OnClickAddListenApp(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtListenAppName.Text))
            {
                ErrorOccurHandler?.Invoke(this, "无效应用名称");
                return;
            }
            
            Model.Collection.Add(new ListenedApp(TxtListenAppName.Text));
            TxtListenAppName.Clear();
        }

        private void OnClickRemoveListenApp(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var row = btn.FindParent<DataGridRow>();
            if (row != null)
            {
                Model.Collection.RemoveAt(row.GetIndex());
            }
        }
    }
}
