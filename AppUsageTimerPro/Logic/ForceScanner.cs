using System;
using System.Diagnostics;
using EasyFramework;
using Serilog;

namespace AppUsageTimerPro.Logic;

internal class ForceScanner : Singleton<ForceScanner>
{
    public Process? CurrentForcedProcess { get; private set; }
        

    ForceScanner()
    {
    }

    public void FixedUpdate(TimeSpan deltaTime)
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