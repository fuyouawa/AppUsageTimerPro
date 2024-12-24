using System;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AppUsageTimerPro
{
    public enum HamburgerOptionsIndex
    {
        TimerPage,
        SchedulerPage,
        ChartPage,
        CloudPage,
        HealthPage,
        SettingsPage
    }

    /// <summary>
    /// Interaction logic for HamburgerOptions.xaml
    /// </summary>
    public partial class HamburgerOptions : UserControl
    {
        protected Frame HamburgerContentFrame = new() { NavigationUIVisibility = NavigationUIVisibility.Hidden };
        protected TimerPage TimerPage = new();
        protected SchedulerPage SchedulerPage = new();
        protected ChartPage ChartPage = new();
        protected CloudPage CloudPage = new();
        protected HealthPage HealthPage = new();
        protected SettingsPage SettingsPage = new();

        public HamburgerOptions()
        {
            InitializeComponent();
            HamburgerMenuControl.Content = HamburgerContentFrame;
            HamburgerContentFrame.Navigate(TimerPage);
        }

        public void NavigateFrame(HamburgerOptionsIndex index)
        {
            switch (index)
            {
                case HamburgerOptionsIndex.TimerPage:
                    HamburgerContentFrame.Navigate(TimerPage);
                    break;
                case HamburgerOptionsIndex.SchedulerPage:
                    HamburgerContentFrame.Navigate(SchedulerPage);
                    break;
                case HamburgerOptionsIndex.ChartPage:
                    HamburgerContentFrame.Navigate(ChartPage);
                    break;
                case HamburgerOptionsIndex.CloudPage:
                    HamburgerContentFrame.Navigate(CloudPage);
                    break;
                case HamburgerOptionsIndex.HealthPage:
                    HamburgerContentFrame.Navigate(HealthPage);
                    break;
                case HamburgerOptionsIndex.SettingsPage:
                    HamburgerContentFrame.Navigate(SettingsPage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index), index, null);
            }

            HamburgerMenuControl.SelectedIndex = (int)index;
        }

        private void OnClickItem(object sender, ItemClickEventArgs args)
        {
            NavigateFrame((HamburgerOptionsIndex)HamburgerMenuControl.SelectedIndex);
        }

        private void OnClickOptionsItem(object sender, ItemClickEventArgs args)
        {
            HamburgerContentFrame.Navigate(SettingsPage);
        }
    }
}
