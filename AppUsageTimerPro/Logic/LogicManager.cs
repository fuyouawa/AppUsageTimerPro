using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using AppUsageTimerPro.Logic;
using EasyFramework;

namespace AppUsageTimerPro;

public class LogicManager : Singleton<LogicManager>
{
    public delegate void FixedUpdateDelegate(TimeSpan deltaTime);

    private List<Action> _pendingFunctors = new();
    private TimeSpan _deltaTime;

    private Timer _timer = new();

    LogicManager()
    {
        _deltaTime = SettingsManager.Instance.Settings.UpdateDeltaTime.Value;
        SettingsManager.Instance.Settings.UpdateDeltaTime.Register(span => Invoke(() => _deltaTime = span));
    }

    public void Initialize()
    {
        _timer.Interval = _deltaTime.Milliseconds;
        _timer.Elapsed += FixedUpdate;
        _timer.AutoReset = true;
        _timer.Start();
    }

    public void Invoke(Action action)
    {
        lock (_pendingFunctors)
        {
            _pendingFunctors.Add(action);
        }
    }

    void FixedUpdate(object? sender, ElapsedEventArgs e)
    {
        // 处理PendingFunctors
        var functors = new List<Action>();
        lock (_pendingFunctors)
        {
            (_pendingFunctors, functors) = (functors, _pendingFunctors);
        }

        foreach (var functor in functors)
        {
            functor();
        }

        // 更新
        ForceScanner.Instance.FixedUpdate(_deltaTime);
        TimerManager.Instance.FixedUpdate(_deltaTime);
        UsageRecordManager.Instance.FixedUpdate(_deltaTime);
    }
}