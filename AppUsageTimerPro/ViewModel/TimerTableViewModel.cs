using AppUsageTimerPro.Model;
using AppUsageTimerPro.Tools;
using AppUsageTimerPro.Utils;
using AppUsageTimerPro.View.Custom.Controls;
using AppUsageTimerPro.View.MainWindow.Timer;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.Media;
using System.Windows;

namespace AppUsageTimerPro.ViewModel
{
    public class TimerTableViewModel
    {
        public ObservableCollection<TimerItem> TimerTableSource { get; set; } = new()
        {
            new TimerItem("Test1", "Tag1", "D:\\General\\Plantform\\CloudMusic\\cloudmusic.exe"),
            new TimerItem("Test2", "Tag2", "D:\\Tool\\Program\\IDA\\ida64.exe"),
            new TimerItem("Test3", "Tag3", "D:\\Tool\\Program\\Vmware\\vmware.exe")
        };

        public RelayCommand AddTimerCmd => new(execute => AddTimer());
        public RelayCommand RemoveTimerCmd => new(execute => RemoveTimer());

        private AddTimerDialog _addTimerDialog = new();

        public TimerTableViewModel()
        {
            _addTimerDialog.btnCancel.Click += CloseAddTimerDialog;
            _addTimerDialog.SuccessAddTimerEvent += AddTimerSuccessed;
        }

        private async void AddTimer()
        {
            _addTimerDialog.ClearText();
            await GlobalManager.Instance.MainWindow.ShowMetroDialogAsync(_addTimerDialog);
        }

        private async void CloseAddTimerDialog(object sender, RoutedEventArgs e)
        {
            await GlobalManager.Instance.MainWindow.HideMetroDialogAsync(_addTimerDialog);
        }

        private async void AddTimerSuccessed(object? sender, SuccessAddTimerEventArgs e)
        {
            TimerTableSource.Add(e.Item);
            await GlobalManager.Instance.MainWindow.HideMetroDialogAsync(_addTimerDialog);
            GlobalManager.Instance.MainWindow.ShowPopupMessageBox("计时器添加成功!");
            SystemSounds.Beep.Play();
        }

        private void RemoveTimer()
        {
        }
    }
}
