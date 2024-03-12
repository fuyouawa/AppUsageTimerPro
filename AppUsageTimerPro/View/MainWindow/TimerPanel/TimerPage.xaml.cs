using AppUsageTimerPro.Tools;
using AppUsageTimerPro.Utils;
using AppUsageTimerPro.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppUsageTimerPro.View.MainWindow.TimerPanel
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
        }
    }
}
