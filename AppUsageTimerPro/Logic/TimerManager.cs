using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyFramework;
using Newtonsoft.Json;

namespace AppUsageTimerPro.Logic;

internal class TimerManager : Singleton<TimerManager>, IEasyEventSubscriber
{
    public Dictionary<string, TimerItem> Timers { get; } = new();
    public bool Loaded { get; private set; }
    public TaskCompletionSource LoadedTsc { get; } = new();

    TimerManager()
    {
        JsonConvert.DefaultSettings += () => new JsonSerializerSettings()
        {
            Converters = new List<JsonConverter>()
            {
                new TimerItemConverter()
            }
        };

        this.RegisterEasyEventSubscriber(triggerExtension: invoker => LogicManager.Instance.Invoke(invoker));
    }

    public async Task Initialize()
    {
        await LoadAsync();
    }

    [EasyEventHandler]
    async Task OnEvent(object sender, GetTimersReq req)
    {
        void Call() => this.TriggerEasyEvent(new GetTimersRes(new List<TimerItem>(Timers.Values)));

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
        DebugHelper.Assert(suc);
    }

    [EasyEventHandler]
    void OnEvent(object sender, RemoveTimerEvent e)
    {
        var suc = Timers.Remove(e.TimerName);
        DebugHelper.Assert(suc);
    }

    public void Update(TimeSpan deltaTime)
    {
        // 当前聚焦的进程
        var proc = ForceScanner.Instance.CurrentForcedProcess;
        if (proc == null) return;

        foreach (var timer in Timers.Values)
        {
            // 此计时器的监听列表是否包含当前聚焦的进程
            if (timer.ListenedApps.ContainAppName(proc.ProcessName))
            {
                timer.TodayUsageTime.Span += deltaTime;
                // 发送更改事件
                this.TriggerEasyEvent(new TimerChangedEvent(timer.Name, TimerChangedTypes.SpanOfTodayUsageTime,
                    timer.TodayUsageTime.Span));
            }
        }
    }

    public async Task AutoSave(TimeSpan interval)
    {
        await SaveAsync();
    }

    public async Task LoadAsync()
    {
        Timers.Clear();
        Loaded = false;
        foreach (var file in Directory.GetFiles(DataManager.Instance.TimerSaveDir))
        {
            var json = await FileHelper.ReadAllTextWithHashAsync(file);
            var val = JsonConvert.DeserializeObject<TimerItem>(json);
            DebugHelper.Assert(val != null);
            var suc = Timers.TryAdd(val.Name, val);
            DebugHelper.Assert(suc);
        }
        Loaded = true;
        LoadedTsc.TrySetResult();
        this.TriggerEasyEvent(new LoadedTimersEvent());
    }

    public async Task SaveAsync()
    {
        foreach (var timer in Timers.Values)
        {
            var path = Path.Combine(DataManager.Instance.TimerSaveDir, timer.Name + ".json");
            await FileHelper.WriteAllTextWithHashAsync(path, JsonConvert.SerializeObject(timer));
        }
    }
}