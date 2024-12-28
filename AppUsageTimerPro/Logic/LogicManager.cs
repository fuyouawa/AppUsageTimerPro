using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using AppUsageTimerPro.Logic;
using EasyFramework;
using Serilog;

namespace AppUsageTimerPro;

public class LogicManager : Singleton<LogicManager>
{
    public delegate void FixedUpdateDelegate(TimeSpan deltaTime);

    private List<Action> _pendingFunctors = new();
    private TimeSpan _deltaTime;

    LogicManager()
    {
    }

    public void Initialize()
    {
        _deltaTime = SettingsManager.Instance.Settings.UpdateDeltaTime.Value;
        SettingsManager.Instance.Settings.UpdateDeltaTime.Register(span => Invoke(() => _deltaTime = span));

        Task.Run(async () =>
        {
            await TimerManager.Instance.Initialize();
            await UpdateLoop();
        });
    }

    public void Invoke(Action action)
    {
        lock (_pendingFunctors)
        {
            _pendingFunctors.Add(action);
        }
    }

    async Task UpdateLoop()
    {
        DateTime lastBegin = DateTime.Now;
        while (true)
        {
            try
            {
                var begin = DateTime.Now;

                // 通过上一次开始时间减去这一次开始时间，计算deltaTime
                var deltaTime = begin - lastBegin;
                lastBegin = begin;

                var functors = new List<Action>();
                lock (_pendingFunctors)
                {
                    (_pendingFunctors, functors) = (functors, _pendingFunctors);
                }

                foreach (var functor in functors)
                {
                    functor();
                }

                ForceScanner.Instance.Update(deltaTime);
                TimerManager.Instance.Update(deltaTime);
                UsageRecordManager.Instance.Update(deltaTime);

                var end = DateTime.Now;
                var diff = end - begin;

                if (diff < _deltaTime)
                {
                    await Task.Delay(_deltaTime - diff);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "逻辑线程更新时出现错误");
            }
        }
    }
}