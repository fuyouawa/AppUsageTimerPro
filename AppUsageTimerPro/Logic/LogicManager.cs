using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using AppUsageTimerPro.Logic;
using EasyFramework;
using Serilog;

namespace AppUsageTimerPro;

public class LogicManager : Singleton<LogicManager>, IEasyEventDispatcher
{
    public delegate void FixedUpdateDelegate(TimeSpan deltaTime);

    private List<Action> _pendingFunctors = new();
    private TimeSpan _deltaTime;
    private TimeSpan _autoSaveInterval;
    private TimeSpan _autoSaveCounter = TimeSpan.Zero;
    private bool _prepareClose = false;

    LogicManager()
    {
    }

    public void Initialize()
    {
        _deltaTime = SettingsManager.Instance.Settings.UpdateDeltaTime.Value;
        SettingsManager.Instance.Settings.UpdateDeltaTime.Register(span => Invoke(() => _deltaTime = span));
        _autoSaveInterval = SettingsManager.Instance.Settings.AutoSaveInterval.Value;
        SettingsManager.Instance.Settings.AutoSaveInterval.Register(span => Invoke(() => _autoSaveInterval = span));

        this.RegisterEasyEventSubscriber(triggerExtension: Invoke);

        Task.Run(async () =>
        {
            await TimerManager.Instance.Initialize();
            await UpdateLoop();
        });
    }

    [EasyEventHandler]
    void OnEvent(object sender, PrepareClosingEvent e)
    {
        _prepareClose = true;
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

                DoPendingFunctors();

                if (_prepareClose)
                {
                    await CloseAsync();
                    break;
                }

                Update(deltaTime);
                AutoSave(deltaTime);

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

    void DoPendingFunctors()
    {
        var functors = new List<Action>();
        lock (_pendingFunctors)
        {
            (_pendingFunctors, functors) = (functors, _pendingFunctors);
        }

        foreach (var functor in functors)
        {
            functor();
        }
    }

    async Task CloseAsync()
    {
        this.UnRegisterEasyEventSubscriber();

        await TimerManager.Instance.SaveAsync();
        await UsageRecordManager.Instance.SaveAsync();

        await TimerManager.Instance.CloseAsync();
        await ForceScanner.Instance.CloseAsync();
        await UsageRecordManager.Instance.CloseAsync();

        this.TriggerEasyEvent(new LogicTaskClosedEvent());
    }

    void Update(TimeSpan deltaTime)
    {
        ForceScanner.Instance.Update(deltaTime);
        TimerManager.Instance.Update(deltaTime);
        UsageRecordManager.Instance.Update(deltaTime);
    }

    void AutoSave(TimeSpan deltaTime)
    {
        if (_autoSaveCounter >= _autoSaveInterval)
        {
            TimerManager.Instance.AutoSave(_autoSaveInterval);
            UsageRecordManager.Instance.AutoSave(_autoSaveInterval);
            _autoSaveCounter = TimeSpan.Zero;
        }
        _autoSaveCounter += deltaTime;
    }
}