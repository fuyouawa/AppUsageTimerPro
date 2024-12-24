using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Security.Principal;

namespace AppUsageTimerPro;

public class WindowsHelper
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    public static Process? GetForegroundProcess()
    {
        var hWnd = GetForegroundWindow();
        if (hWnd == IntPtr.Zero)
        {
            return null;
        }

        GetWindowThreadProcessId(hWnd, out var processId);
        
        var process = Process.GetProcessById((int)processId);
        return process;
    }


    public static bool IsAdministrator()
    {
        using (var identity = WindowsIdentity.GetCurrent())
        {
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}