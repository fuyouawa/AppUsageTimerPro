using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using EasyFramework;
using MahApps.Metro.IconPacks;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for TimerItemOperations.xaml
    /// </summary>
    public partial class TimerItemOperations : UserControl, IEasyEventDispatcher
    {
        public TimerPage? OwnerPage { get; private set; }
        public DataGridRow? Row { get; private set; }

        private bool _loaded = false;

        public TimerItemOperations()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                if (_loaded)
                    return;
                Refresh();
                _loaded = true;
            };

            Unloaded += (sender, args) =>
            {
                _loaded = false;
            };
        }

        public void Refresh()
        {
            var row = this.FindParent<DataGridRow>();
            Debug.Assert(row != null);
            Row = row;
            
            var page = row.FindParent<TimerPage>();
            Debug.Assert(page != null);
            OwnerPage = page;
            
            var timer = OwnerPage!.ViewModel.Collection[Row!.GetIndex()];
            RefreshBtnPause(timer.Pausing);
        }

        private void RefreshBtnPause(bool pause)
        {
            BtnPause.Content = new PackIconZondicons()
            {
                Kind = pause
                    ? PackIconZondiconsKind.PlayOutline
                    : PackIconZondiconsKind.PauseOutline,
                Width = 21,
                Height = 21,
            };
        }

        private void OnClickPause(object sender, RoutedEventArgs e)
        {
            var timer = OwnerPage!.ViewModel.Collection[Row!.GetIndex()];

            RefreshBtnPause(!timer.Pausing);

            this.TriggerEasyEvent(new TimerPauseChangedEvent(timer.Name, !timer.Pausing));
        }
    }
}