using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyFramework;
using Newtonsoft.Json;
using Serilog;

namespace AppUsageTimerPro.Logic;

internal class TimerManager : Singleton<TimerManager>, IEasyEventDispatcher
{
    public Dictionary<string, TimerItem> Timers { get; } = new();
    public bool Loaded { get; private set; }
    public TaskCompletionSource LoadedTsc { get; } = new();

    TimerManager()
    {
        this.RegisterEasyEventSubscriber(triggerExtension: invoker => LogicManager.Instance.Invoke(invoker));
    }

    public async Task Initialize()
    {
        await LoadAsync();
    }

    [EasyEventHandler]
    void OnEvent(object sender, TimerPauseChangedEvent e)
    {
        var suc = Timers.TryGetValue(e.TimerName, out var timer);
        Debug.Assert(suc && timer != null);
        timer.Pausing = e.Pause;
    }

    [EasyEventHandler]
    async Task OnEvent(object sender, GetTimersReq req)
    {
        void Call() => this.TriggerEasyEvent(new GetTimersRes(new TimerItemList(Timers.Values.CloneAll())));

        if (Loaded)
        {
            Call();
        }
        else
        {
            await LoadedTsc.Task;
            Debug.Assert(Loaded);
            Call();
        }
    }

    [EasyEventHandler]
    void OnEvent(object sender, AddTimerEvent e)
    {
        // 确保不会有重复计时器名称
        var suc = Timers.TryAdd(e.Timer.Name, e.Timer);
        Debug.Assert(suc);
    }

    [EasyEventHandler]
    void OnEvent(object sender, RemoveTimerEvent e)
    {
        var suc = Timers.Remove(e.TimerName);
        Debug.Assert(suc);
    }

    public void Update(TimeSpan deltaTime)
    {
        // 当前聚焦的进程
        var proc = ForceScanner.Instance.CurrentForcedProcess;
        if (proc == null) return;

        foreach (var timer in Timers.Values)
        {
            if (timer.Pausing)
                continue;
            // 此计时器的监听列表是否包含当前聚焦的进程
            timer.Forcing = timer.ListenedApps.ContainAppName(proc.ProcessName);
            if (timer.Forcing)
            {
                timer.TodayUsageTime.Span += deltaTime;
                // 发送更改事件
                this.TriggerEasyEvent(new TimerTodayUsageTimeSpanChangedEvent(timer.Name, timer.TodayUsageTime.Span));
            }
        }
    }

    public void AutoSave(TimeSpan interval)
    {
        BeginSave();
    }

    public async Task LoadAsync()
    {
        try
        {
            Timers.Clear();
            Loaded = false;
            foreach (var file in Directory.GetFiles(DataManager.Instance.TimerSaveDir).Where(p => !p.EndsWith("hash")))
            {
                var json = await FileHelper.ReadAllTextWithHashAsync(file);
                var val = JsonConvert.DeserializeObject<TimerItem>(json);
                Debug.Assert(val != null);
                var suc = Timers.TryAdd(val.Name, val);
                Debug.Assert(suc);
            }

            Loaded = true;
            LoadedTsc.TrySetResult();
            this.TriggerEasyEvent(new LoadedTimersEvent());
        }
        catch (Exception e)
        {
            Log.Fatal(e, "加载计时器失败");
        }
    }

    public async Task CloseAsync()
    {
        this.UnRegisterEasyEventSubscriber();
    }

    public async Task SaveAsync()
    {
        var timers = new TimerItemList(Timers.Values.CloneAll());
        await SaveAsync(timers);
    }

    public void BeginSave(Action? savedCallback = null)
    {
        var timers = new TimerItemList(Timers.Values.CloneAll());
        Task.Run(async () =>
        {
            await SaveAsync(timers);
            savedCallback?.Invoke();
        });
    }

    private static async Task SaveAsync(TimerItemList timers)
    {
        foreach (var timer in timers)
        {
            var path = Path.Combine(DataManager.Instance.TimerSaveDir, timer.Name + ".json");
            await FileHelper.WriteAllTextWithHashAsync(path, JsonConvert.SerializeObject(timer));
        }
    }
}