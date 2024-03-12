using AppUsageTimerPro.Model;
using AppUsageTimerPro.Tools;
using AppUsageTimerPro.Utils;
using AppUsageTimerPro.View.MainWindow.TimerPanel;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;

namespace AppUsageTimerPro.ViewModel
{
    public class TimerTableViewModel
    {
        public ObservableCollection<TimerItem> TimerTableSource { get; set; } = new()
        {
            new TimerItem("Test1", new[]{"Tag1"}, "D:\\General\\Plantform\\CloudMusic\\cloudmusic.exe"),
            new TimerItem("Test2", new[]{"Tag2"}, "D:\\Tool\\Program\\IDA\\ida64.exe"),
            new TimerItem("Test3", new[]{"Tag3", "Tag4"}, "D:\\Tool\\Program\\Vmware\\vmware.exe")
        };

        public RelayCommand AddTimerCmd => new(execute => AddTimer());
        public RelayCommand RemoveTimerCmd => new(execute => RemoveTimer());


        private async void AddTimer()
        {
            await GlobalManager.Instance.MainWindow.ShowMetroDialogAsync(new AddTimerDialog());
        }

        private void RemoveTimer()
        {

        }
    }
}
