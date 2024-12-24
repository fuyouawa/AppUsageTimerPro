using MahApps.Metro.Controls;
using System.Windows.Media;
using System.Windows;

namespace AppUsageTimerPro
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

        public static void ShowPopupMessage(this MetroWindow window, string message, long lifeTime = 3000, MessageType type = MessageType.Info, bool hasLifeTime = true)
        {
            PopupMessageBox box = new()
            {
                IsAutoCloseEnabled = hasLifeTime,
                AutoCloseInterval = lifeTime,
                MsgType = type
            };
            box.TxtMessage.Text = message;
            window.ShowFlyout(box);
        }

        public static T? FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            if (parentObject is T parent) return parent;

            return FindParent<T>(parentObject);
        }
    }
}
