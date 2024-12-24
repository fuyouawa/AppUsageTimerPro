using System.Windows;
using System.Windows.Controls;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
        }

        private void OnClickSaveToCloudMenuItem(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.HamburgerOptions.NavigateFrame(HamburgerOptionsIndex.CloudPage);
        }
    }
}
