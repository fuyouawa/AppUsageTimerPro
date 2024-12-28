using System;
using System.ComponentModel;
using EasyFramework;
using EasyUiFramework;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IEasyEventSubscriber
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

        private bool _prepareClose = false;

        public MainWindow()
        {
            DebugHelper.Assert(_instance == null);
            _instance = this;
            InitializeComponent();

            this.RegisterEasyEventSubscriberInUiThread().UnRegisterWhenUnloaded(this);
        }

        [EasyEventHandler]
        void OnEvent(object sender, LogicTaskClosedEvent e)
        {
            DebugHelper.Assert(_prepareClose);
            Close();
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            if (_prepareClose)
                return;

            e.Cancel = true;
            _prepareClose = true;
            this.TriggerEasyEvent(new PrepareClosingEvent());
            this.ShowMetroDialogAsync(new WaitCloseDialog());
        }
    }
}
