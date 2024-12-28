using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;
using EasyFramework;
using EasyUiFramework;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for TimerPage.xaml
    /// </summary>
    public partial class TimerPage : Page, IEasyEventSubscriber
    {
        private AddTimerDialog _addTimerDialog = new();
        private bool _loaded = false;

        public TimerPageViewModel ViewModel => (TimerPageViewModel)DataContext;

        public TimerPage()
        {
            InitializeComponent();

            _addTimerDialog.BtnCancel.Click += CloseAddTimerDialog;
            _addTimerDialog.SuccessAddTimerEvent += AddTimerSuccessed;

            Loaded += (sender, args) =>
            {
                if (_loaded)
                    return;
                this.TriggerEasyEvent(new GetTimersReq());
                _loaded = true;
            };

            Unloaded += (sender, args) =>
            {
                Debug.WriteLine("23452");
                _loaded = false;
            };

            this.RegisterEasyEventSubscriberInUiThread()
                .UnRegisterWhenUnloaded(this);
        }

        [EasyEventHandler]
        void OnEvent(object sender, GetTimersRes e)
        {
            ViewModel.Collection = new ObservableCollection<TimerItem>(e.Timers);
        }

        [EasyEventHandler]
        void OnEvent(object sender, AddTimerEvent e)
        {
            ViewModel.Collection.Add(e.Timer);
        }

        [EasyEventHandler]
        void OnEvent(object sender, RemoveTimerEvent e)
        {
            int i = 0;
            for (; i < ViewModel.Collection.Count; i++)
            {
                if (ViewModel.Collection[i].Name == e.TimerName)
                {
                    break;
                }
            }
            DebugHelper.Assert(i < ViewModel.Collection.Count);
            ViewModel.Collection.RemoveAt(i);
        }

        [EasyEventHandler]
        void OnEvent(object sender, TimerChangedEvent e)
        {
            var timer = ViewModel.Collection.FirstOrDefault(t => t.Name == e.TimerName);
            if (timer == null)
                return;
            timer.ApplyChange(e.ChangedType, e.Value, true);
        }

        private async void CloseAddTimerDialog(object sender, RoutedEventArgs e)
        {
            await MainWindow.Instance.HideMetroDialogAsync(_addTimerDialog);
        }

        private async void AddTimerSuccessed(object? sender, SuccessAddTimerEventArgs e)
        {
            this.TriggerEasyEvent(new AddTimerEvent((TimerItem)e.Timer.Clone()));
            await MainWindow.Instance.HideMetroDialogAsync(_addTimerDialog);
            MainWindow.Instance.ShowPopupMessage("计时器添加成功!");
            SoundsManager.PlayTip();
        }

        private async void OnClickAddTimer(object sender, RoutedEventArgs e)
        {
            _addTimerDialog.Clear();
            await MainWindow.Instance.ShowMetroDialogAsync(_addTimerDialog);
        }

        private async void OnClickRemoveTimer(object sender, RoutedEventArgs e)
        {
            var selectedTimer = ViewModel.Collection[ViewModel.SelectedIndex];
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

                this.TriggerEasyEvent(new RemoveTimerEvent(selectedTimer.Name));
                MainWindow.Instance.ShowPopupMessage("删除成功!", 2000);
                SoundsManager.PlayTip();
            }
        }
    }
}