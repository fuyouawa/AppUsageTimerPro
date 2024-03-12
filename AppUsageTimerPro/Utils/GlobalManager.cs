using AppUsageTimerPro.Tools;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace AppUsageTimerPro.Utils
{
    public class GlobalManager : Singleton<GlobalManager>
    {
		private MetroWindow? _mainWindow;

		public MetroWindow MainWindow
		{
			get
			{
				Debug.Assert(_mainWindow != null);
				return _mainWindow;
			}
			set { _mainWindow = value; }
		}
	}
}
