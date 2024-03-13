using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AppUsageTimerPro.View.MainWindow
{
    /// <summary>
    /// Interaction logic for HamburgerOptions.xaml
    /// </summary>
    public partial class HamburgerOptions : UserControl
    {
        protected Frame hamburgerContentFrame = new() { NavigationUIVisibility = NavigationUIVisibility.Hidden };
        protected Timer.TimerPage timerPage = new();
        protected Chart.ChartPage chartPage = new();
        protected Cloud.CloudPage cloudPage = new();
        protected Design.DesignPage designPage = new();
        protected Settings.SettingsPage settingsPage = new();

        public HamburgerOptions()
        {
            InitializeComponent();
            HamburgerMenuControl.Content = hamburgerContentFrame;
            hamburgerContentFrame.Navigate(timerPage);
        }


        private void HamburgerMenuControl_ItemClick(object sender, ItemClickEventArgs args)
        {
            switch (HamburgerMenuControl.SelectedIndex)
            {
                case 0:
                    hamburgerContentFrame.Navigate(timerPage);
                    break;
                case 1:
                    hamburgerContentFrame.Navigate(chartPage);
                    break;
                case 2:
                    hamburgerContentFrame.Navigate(cloudPage);
                    break;
                case 3:
                    hamburgerContentFrame.Navigate(designPage);
                    break;
            }
        }

        private void HamburgerMenuControl_OptionsItemClick(object sender, ItemClickEventArgs args)
        {
            hamburgerContentFrame.Navigate(settingsPage);
        }
    }
}
