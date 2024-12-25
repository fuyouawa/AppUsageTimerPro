using System;
using System.Collections.Generic;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;

namespace AppUsageTimerPro
{
    public class TimerTableViewModel : ViewModelBase
    {
        public ObservableCollection<TimerItem> Collection { get; set; } = new();

        public RelayCommand AddTimerCmd => new(execute => AddTimer());
        public RelayCommand RemoveTimerCmd => new(execute => RemoveTimer(), canExecute => SelectedItem != null);

        private object? _selectedItem;

        public object? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private int _selectedIndex = 0;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }


        private AddTimerDialog _addTimerDialog = new();

        public TimerTableViewModel()
        {
            _addTimerDialog.BtnCancel.Click += CloseAddTimerDialog;
            _addTimerDialog.SuccessAddTimerHandler += AddTimerSuccessed;
        }

        private async void AddTimer()
        {
            _addTimerDialog.ClearText();
            await MainWindow.Instance.ShowMetroDialogAsync(_addTimerDialog);
        }

        private async void CloseAddTimerDialog(object sender, RoutedEventArgs e)
        {
            await MainWindow.Instance.HideMetroDialogAsync(_addTimerDialog);
        }

        private async void AddTimerSuccessed(object? sender, SuccessAddTimerEventArgs e)
        {
            Collection.Add(e.Item);
            await MainWindow.Instance.HideMetroDialogAsync(_addTimerDialog);
            MainWindow.Instance.ShowPopupMessage("计时器添加成功!");
            SoundsManager.PlayTip();
        }

        private async void RemoveTimer()
        {
            var selectedTimer = Collection[SelectedIndex];
            var res = await MainWindow.Instance.ShowMessageAsync(
                "警告",
                $"您确定要删除\"{selectedTimer.Name}\"计时器吗?\n此操作不可逆!",
                MessageDialogStyle.AffirmativeAndNegative,
                ResourceManager.LocadedDialogSettings);

            if (res == MessageDialogResult.Affirmative)
            {
                var verifName = await MainWindow.Instance.ShowInputAsync(
                    "双重验证",
                    $"请确认当前要删除的计时器名称({selectedTimer.Name})",
                    ResourceManager.LocadedDialogSettings);
                if (verifName == null)
                    return;
                if (verifName != selectedTimer.Name)
                {
                    MainWindow.Instance.ShowPopupMessage("验证失败, 计时器名称不一致!", 4000, MessageType.Error);
                    SoundsManager.PlayError();
                    return;
                }

                bool suc = Collection.Remove(selectedTimer);
                if (suc)
                {
                    MainWindow.Instance.ShowPopupMessage("删除成功!", 2000);
                    SoundsManager.PlayTip();
                    return;
                }
                else
                {
                    SoundsManager.PlayError();
                    await MainWindow.Instance.ShowMessageAsync(
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