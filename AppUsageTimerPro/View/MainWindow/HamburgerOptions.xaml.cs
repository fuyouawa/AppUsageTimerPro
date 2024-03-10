using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppUsageTimerPro.View.MainWindow
{
    /// <summary>
    /// Interaction logic for HamburgerOptions.xaml
    /// </summary>
    public partial class HamburgerOptions : UserControl
    {
        protected Frame hamburgerContentFrame = new() { NavigationUIVisibility = NavigationUIVisibility.Hidden };
        protected TimerPage timerPage = new();
        protected ChartPage chartPage = new();
        protected CloudPage cloudPage = new();
        protected SettingsPage settingsPage = new();

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
            }
        }

        private void HamburgerMenuControl_OptionsItemClick(object sender, ItemClickEventArgs args)
        {
            hamburgerContentFrame.Navigate(settingsPage);
        }
    }
}
