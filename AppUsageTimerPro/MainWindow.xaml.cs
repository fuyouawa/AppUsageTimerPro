using System;
using MahApps.Metro.Controls;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static MainWindow? _instance;

        public static MainWindow Instance
        {
            get
            {
                DebugHelper.Assert(_instance != null);
                return _instance;
            }
        }

        public MainWindow()
        {
            DebugHelper.Assert(_instance == null);
            _instance = this;
            InitializeComponent();
        }
    }
}
