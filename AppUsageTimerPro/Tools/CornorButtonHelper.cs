using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppUsageTimerPro.Tools
{
    public static class CornorButtonHelper
    {
        public static readonly DependencyProperty CornorRadiusProperty =
            DependencyProperty.RegisterAttached(
                "CornorRadius",
                typeof(double),
                typeof(CornorButtonHelper),
                new FrameworkPropertyMetadata(10.0));

        public static double GetCornorRadius(DependencyObject obj)
        {
            return (double)obj.GetValue(CornorRadiusProperty);
        }

        public static void SetCornorRadius(DependencyObject obj, double value)
        {
            obj.SetValue(CornorRadiusProperty, value);
        }
    }
}
