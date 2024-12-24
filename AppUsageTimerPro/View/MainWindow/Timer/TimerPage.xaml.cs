using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for TimerPage.xaml
    /// </summary>
    public partial class TimerPage : Page
    {
        public TimerPage()
        {
            InitializeComponent();
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
