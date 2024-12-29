using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EasyFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace AppUsageTimerPro.Logic;

internal struct UsageRecord
{
    public TimeSpan Span { get; set; }
    public string? AppName { get; set; }
}

internal class UsageRecordList : List<UsageRecord>
{
}
internal class UsageRecordListConverter : JsonConverter<UsageRecordList>
{
    public override void WriteJson(JsonWriter writer, UsageRecordList? value, JsonSerializer serializer)
    {
        if (value == null)
            return;
        writer.WriteStartObject();

        foreach (var record in value)
        {
            writer.WritePropertyName(record.Span.ToString());
            writer.WriteValue(record.AppName);
        }

        writer.WriteEndObject();
    }

    public override UsageRecordList? ReadJson(JsonReader reader, Type objectType, UsageRecordList? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        if (!obj.HasValues) return null;

        var res = new UsageRecordList();
        foreach (var property in obj.Properties())
        {
            var time = property.Name;
            var appName = property.Value.ToString();
            res.Add(new UsageRecord() { AppName = appName, Span = TimeSpan.Parse(time) });
        }

        return res;
    }
}

internal class UsageRecordManager : Singleton<UsageRecordManager>
{
    private Dictionary<DateTime, UsageRecordList> _recordsSaveQueue = new();

    UsageRecordManager()
    {
    }

    public void Update(TimeSpan deltaTime)
    {
    }

    public void AddRecord(DateTime time, string? appName)
    {
        if (!_recordsSaveQueue.TryGetValue(time.Date, out var list))
        {
            list = new UsageRecordList();
            _recordsSaveQueue[time.Date] = list;
        }

        list.Add(new UsageRecord() { AppName = appName, Span = time.TimeOfDay });
    }

    public async Task CloseAsync()
    {
        this.UnRegisterEasyEventSubscriber();
    }

    public void AutoSave(TimeSpan interval)
    {
        BeginSave();
    }

    public async Task SaveAsync()
    {
        var recordsSaveQueue = new Dictionary<DateTime, UsageRecordList>();
        (recordsSaveQueue, _recordsSaveQueue) = (_recordsSaveQueue, recordsSaveQueue);
        
        await SaveAsync(recordsSaveQueue);
    }

    public void BeginSave(Action? savedCallback = null)
    {
        var recordsSaveQueue = new Dictionary<DateTime, UsageRecordList>();
        (recordsSaveQueue, _recordsSaveQueue) = (_recordsSaveQueue, recordsSaveQueue);

        Task.Run(async () =>
        {
            await SaveAsync(recordsSaveQueue);
            savedCallback?.Invoke();
        });
    }

    private static async Task SaveAsync(Dictionary<DateTime, UsageRecordList> recordsSaveQueue)
    {
        try
        {
            if (recordsSaveQueue.Count > 0)
            {
                foreach (var records in recordsSaveQueue)
                {
                    // 获取日期对应的记录文件
                    var path = Path.Combine(DataManager.Instance.UsageSaveDir,
                        records.Key.ToString("yyyy-MM-dd") + ".json");

                    SafeFile.CreateIfNotExist(path);

                    // 读取json并反序列化
                    var res = await SafeFile.ReadAndDeserializeJsonAsync<UsageRecordList>(path) ?? new UsageRecordList();
                    // 将记录队列中的值与原值合并
                    res.AddRange(records.Value);
                    // 保存
                    await SafeFile.WriteAllTextAsync(path, JsonConvert.SerializeObject(res));
                }
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "保存使用记录时出现错误");
        }
    }
}