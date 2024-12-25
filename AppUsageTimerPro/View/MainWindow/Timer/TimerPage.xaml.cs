using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for TimerPage.xaml
    /// </summary>
    public partial class TimerPage : Page
    {
        private DispatcherTimer _timer = new();
        private TimeSpan _interval = TimeSpan.FromMilliseconds(500);

        public TimerPage()
        {
            InitializeComponent();

            _timer.Interval = _interval;
            _timer.Tick += Update;

            Loaded += (sender, args) =>
            {
                _timer.Start();
            };

            Unloaded += (sender, args) =>
            {
                _timer.Stop();
            };
        }


        private void Update(object? sender, EventArgs args)
        {
            var model = (TimerTableViewModel)DataContext;

            var proc = ForceScanner.Instance.CurrentForcedProcess;
            if (proc == null) return;

            foreach (var item in model.Collection)
            {
                
            }
        }
    }
}
