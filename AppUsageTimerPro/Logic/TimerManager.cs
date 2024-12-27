using System;
using System.Collections.Generic;
using System.Diagnostics;
using EasyFramework;

namespace AppUsageTimerPro.Logic;

internal class TimerManager : Singleton<TimerManager>, IEasyEventSubscriber
{
    public List<TimerItem> Timers { get; } = new();

    TimerManager()
    {
        this.RegisterEasyEventSubscriber(triggerExtension: invoker => LogicManager.Instance.Invoke(invoker));
    }

    [EasyEventHandler]
    void OnEvent(object sender, LoadedTimerUiEvent e)
    {
        this.TriggerEasyEvent(new ReloadTimersEvent(Timers));
    }

    [EasyEventHandler]
    void OnEvent(object sender, AddTimerEvent e)
    {
        Timers.Add(e.Timer);
    }

    [EasyEventHandler]
    void OnEvent(object sender, RemoveTimerEvent e)
    {
        Timers.Remove(e.Timer);
    }

    public void FixedUpdate(TimeSpan deltaTime)
    {
        // 当前聚焦的进程
        var proc = ForceScanner.Instance.CurrentForcedProcess;
        if (proc == null) return;
        
        foreach (var timer in Timers)
        {
            // 此计时器的监听列表是否包含当前聚焦的进程
            if (timer.ListenedApps.ContainAppName(proc.ProcessName))
            {
                timer.TodayUsageTime.Span += deltaTime;
                // 发送更改事件
                this.TriggerEasyEvent(new TimerChangedEvent(timer.Name, TimerChangedTypes.SpanOfTodayUsageTime, timer.TodayUsageTime.Span));
            }
        }
    }
}