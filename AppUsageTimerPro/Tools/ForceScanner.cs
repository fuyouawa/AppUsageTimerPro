using System.Diagnostics;
using System.Threading.Tasks;

namespace AppUsageTimerPro.Tools;

public class ForceScanner : Singleton<ForceScanner>
{
    public int MsDelay = 100;
    
    private Process? _process;

    public string? ForcedProcessName
    {
        get
        {
            lock (this)
            {
                return _process?.ProcessName;
            }
        }
    }

    public ProcessModule? ForcedProcessModule
    {
        get
        {
            lock (this)
            {
                return _process?.MainModule;
            }
        }
    }

    ForceScanner()
    {
    }

    public void Initialize()
    {
        Task.Run(Scan);
    }

    private async void Scan()
    {
        while (true)
        {
            var proc = WindowsHelper.GetForegroundProcess();
            if (proc != _process)
            {
                
            }
            lock (this)
            {
                _process = proc;
            }

            await Task.Delay(MsDelay);
        }
    }
}