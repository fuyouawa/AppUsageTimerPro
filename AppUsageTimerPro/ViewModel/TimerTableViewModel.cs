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
    public class TimerTableViewModel : ViewModelBase
    {
        public ObservableCollection<TimerItem> Collection { get; set; } = new()
        {
            new TimerItem("Test1", "Tag1", "D:\\General\\Plantform\\CloudMusic\\cloudmusic.exe"),
            new TimerItem("Test2", "Tag2", "D:\\Tool\\Program\\IDA\\ida64.exe"),
            new TimerItem("Test3", "Tag3", "D:\\Tool\\Program\\Vmware\\vmware.exe")
        };

        public RelayCommand AddTimerCmd => new(execute => AddTimer());
        public RelayCommand RemoveTimerCmd => new(execute => RemoveTimer(), canExecute => SelectedItem != null);

        private object? _selectedItem;

        public object? SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private int _selectedIndex = 0;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; OnPropertyChanged(); }
        }



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
            Collection.Add(e.Item);
            await GlobalManager.Instance.MainWindow.HideMetroDialogAsync(_addTimerDialog);
            GlobalManager.Instance.MainWindow.ShowPopupMessage("计时器添加成功!");
            SoundsManager.PlayTip();
        }

        private async void RemoveTimer()
        {
            var selectedTimer = Collection[SelectedIndex];
            var res = await GlobalManager.Instance.MainWindow.ShowMessageAsync(
                "警告",
                $"您确定要删除\"{selectedTimer.Name}\"计时器吗?\n此操作不可逆!",
                MessageDialogStyle.AffirmativeAndNegative,
                ResourceManager.LocadedDialogSettings);

            if (res == MessageDialogResult.Affirmative)
            {
                var verifName = await GlobalManager.Instance.MainWindow.ShowInputAsync(
                    "双重验证",
                    $"请确认当前要删除的计时器名称({selectedTimer.Name})",
                    ResourceManager.LocadedDialogSettings);
                if (verifName == null)
                    return;
                if (verifName != selectedTimer.Name)
                {
                    GlobalManager.Instance.MainWindow.ShowPopupMessage("验证失败, 计时器名称不一致!", 4000, MessageType.Error);
                    SoundsManager.PlayError();
                    return;
                }
                bool suc = Collection.Remove(selectedTimer);
                if (suc)
                {
                    GlobalManager.Instance.MainWindow.ShowPopupMessage("删除成功!", 2000);
                    SoundsManager.PlayTip();
                    return;
                }
                else
                {
                    SoundsManager.PlayError();
                    await GlobalManager.Instance.MainWindow.ShowMessageAsync(
                        "错误",
                        "出现预料之外的错误?\n删除已经被删除的计时器!\n可能是由于程序bug, 请联系开发者",
                        MessageDialogStyle.Affirmative,
                        ResourceManager.LocadedDialogSettings);
                    return;
                }
            }
        }
    }
}
