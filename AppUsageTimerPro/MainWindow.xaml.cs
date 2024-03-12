using AppUsageTimerPro.Utils;
using MahApps.Metro.Controls;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            GlobalManager.Instance.MainWindow = this;
            InitializeComponent();
        }
    }
}
