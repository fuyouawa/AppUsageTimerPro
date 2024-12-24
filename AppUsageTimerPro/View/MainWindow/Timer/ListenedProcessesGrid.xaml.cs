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
    /// ListenedProcessesGrid.xaml 的交互逻辑
    /// </summary>
    public partial class ListenedProcessesGrid : UserControl
    {
        public event EventHandler<string>? ErrorOccurHandler;

        public ListenedProcessesViewModel Model => (ListenedProcessesViewModel)DataContext;

        public ListenedProcessesGrid()
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
                TxtListenAppName.Text = Path.GetFileName(dialog.FileName);
            }
        }

        private void OnClickAddListenApp(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtListenAppName.Text))
            {
                ErrorOccurHandler?.Invoke(this, "无效应用名称");
                return;
            }
            
            Model.Collection.Add(new ListenedProcess(TxtListenAppName.Text));
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
