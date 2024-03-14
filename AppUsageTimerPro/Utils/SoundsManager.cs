using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace AppUsageTimerPro.Utils
{
    public class SoundsManager
    {
        public static void PlayError()
        {
            SystemSounds.Hand.Play();
        }
        public static void PlayTip()
        {
            SystemSounds.Beep.Play();
        }
    }
}
