using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUsageTimerPro
{
    public class ResourceManager
    {
        public static MetroDialogSettings LocadedDialogSettings { get; private set; } = new()
        {
            AffirmativeButtonText = "确定",
            NegativeButtonText = "取消",
            DialogButtonFontSize = 15
        };
    }
}
