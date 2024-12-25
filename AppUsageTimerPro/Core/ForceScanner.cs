using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;
using Serilog;

namespace AppUsageTimerPro
{
    public class ForceScanner : Singleton<ForceScanner>
    {
        public Process? CurrentForcedProcess { get; private set; }
        
        private DispatcherTimer _timer = new();
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(16);


        ForceScanner()
        {
        }

        public void Initialize()
        {
            _timer.Interval = _interval;
            _timer.Tick += Scan;
            _timer.Start();
        }

        private void Scan(object? sender, EventArgs args)
        {
            try
            {
                var proc = WindowsHelper.GetForegroundProcess();
                if (proc?.ProcessName != CurrentForcedProcess?.ProcessName)
                {
                    UsageRecordManager.Instance.AddRecord(DateTime.Now, proc?.ProcessName);
                }
                
                CurrentForcedProcess = proc;
            }
            catch (Exception e)
            {
                Log.Error(e, "监听应用聚焦时出现错误");
            }
        }
    }
}