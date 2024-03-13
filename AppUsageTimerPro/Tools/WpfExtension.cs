using AppUsageTimerPro.View.Custom.Controls;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUsageTimerPro.Tools
{
    public static class WpfExtension
    {
        public static void ShowFlyout(this MetroWindow window, Flyout flyout)
        {
            window.Flyouts.Items.Add(flyout);
        }

        public static void RemoveFlyout(this MetroWindow window, Flyout flyout)
        {
            window.Flyouts.Items.Remove(flyout);
        }

        public static void ShowPopupMessageBox(this MetroWindow window, string message, MessageType type = MessageType.Info, long lifeTime = 3000, bool hasLifeTime = true)
        {
            window.ShowFlyout(new PopupMessageBox()
            {
                IsAutoCloseEnabled = hasLifeTime,
                AutoCloseInterval = lifeTime,
                Message = message,
                MsgType = type
            });
        }
    }
}
