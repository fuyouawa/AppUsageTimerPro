using System.Diagnostics;
using System.Threading.Tasks;
using AppUsageTimerPro.Tools;
using AppUsageTimerPro.Utils;
using AppUsageTimerPro.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppUsageTimerPro.View.MainWindow.Timer
{
    /// <summary>
    /// Interaction logic for TimerPage.xaml
    /// </summary>
    public partial class TimerPage : Page
    {
        public TimerPage()
        {
            InitializeComponent();
            DataContext = new TimerTableViewModel();

            Loaded += (sender, args) =>
            {
                _isRunning = true;
            };

            Unloaded += (sender, args) =>
            {
                _isRunning = false;
            };
        }

        private bool _isRunning = true;

        private async void Update()
        {
            var model = (TimerTableViewModel)DataContext;
            while (_isRunning)
            {

                foreach (var item in model.Collection)
                {
                    
                }
                await Task.Delay(1000);
            }
        }
    }
}
