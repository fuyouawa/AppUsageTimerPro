using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace AppUsageTimerPro;

public struct UsageRecord
{
    public TimeSpan Span { get; set; }
    public int AppIndex { get; set; }
}

public class UsageRecordList : List<UsageRecord> {}

public class AppIndexTable : List<string>
{
    public AppIndexTable() : base() {}
    public AppIndexTable(int capacity) : base(capacity) { }
    public AppIndexTable(IEnumerable<string> collection) : base(collection) {}

    private bool _hasChange;

    private Dictionary<string, int> _appToIndex = new();

    public bool CheckChange()
    {
        var t = _hasChange;
        _hasChange = false;
        return t;
    }

    public int GetIndex(string appName)
    {
        if (_appToIndex.Count != Count)
        {
            for (int i = 0; i < Count; i++)
            {
                _appToIndex[this[i]] = i;
            }
        }

        if (!_appToIndex.TryGetValue(appName, out var index))
        {
            DebugHelper.Assert(IndexOf(appName) == -1);
            Add(appName);
            index = Count - 1;
            _appToIndex[appName] = index;
            _hasChange = true;
        }
        return index;
    }
}

public class UsageRecordListConverter : JsonConverter<UsageRecordList>
{
    public override void WriteJson(JsonWriter writer, UsageRecordList? value, JsonSerializer serializer)
    {
        if (value == null)
            return;
        writer.WriteStartObject();

        foreach (var record in value)
        {
            writer.WritePropertyName(record.Span.ToTimeString());
            writer.WriteValue(record.AppIndex);
        }

        writer.WriteEndObject();
    }

    public override UsageRecordList? ReadJson(JsonReader reader, Type objectType, UsageRecordList? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        if (!obj.HasValues) return null;

        var res = new UsageRecordList();
        foreach (var property in obj.Properties())
        {
            var time = property.Name;
            var index = property.Value.ToObject<int>();
            res.Add(new UsageRecord(){AppIndex = index, Span = TimeSpan.Parse(time)});
        }
        return res;
    }
}

public class AppIndexTableConverter : JsonConverter<AppIndexTable>
{
    public override void WriteJson(JsonWriter writer, AppIndexTable? value, JsonSerializer serializer)
    {
        if (value == null)
            return;
        writer.WriteStartObject();

        for (int i = 0; i < value.Count; i++)
        {
            writer.WritePropertyName(value[i]);
            writer.WriteValue(i);
        }

        writer.WriteEndObject();
    }

    public override AppIndexTable? ReadJson(JsonReader reader, Type objectType, AppIndexTable? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        if (!obj.HasValues) return null;

        var tmp = new Dictionary<int, string>();
        foreach (var property in obj.Properties())
        {
            var appName = property.Name;
            var index = property.Value.ToObject<int>();
            tmp.Add(index, appName);
        }
        return new AppIndexTable(tmp.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToList());
    }
}

public class UsageRecordManager : Singleton<UsageRecordManager>
{
    private AppIndexTable _appIndexTable;

    private Dictionary<DateTime, UsageRecordList> _recordsSaveQueue = new();

    public readonly string AppIndexSavePath;

    UsageRecordManager()
    {
        SettingsManager.Instance.JsonSerializerSettings.Converters.Add(new AppIndexTableConverter());
        SettingsManager.Instance.JsonSerializerSettings.Converters.Add(new UsageRecordListConverter());

        AppIndexSavePath = Path.Combine(DataManager.Instance.UsageSaveDir, "AppIndex.json");

        FileHelper.CreateIfNotExist(AppIndexSavePath);

        var json = File.ReadAllText(AppIndexSavePath);
        _appIndexTable = JsonConvert.DeserializeObject<AppIndexTable>(json) ?? new AppIndexTable();

        Task.Run(AutoSaveLoop);
    }


    private async Task AutoSaveLoop()
    {
        try
        {
            while (true)
            {
                await Task.Delay(SettingsManager.Instance.Settings.AutoSaveInterval);
                await SaveAsync();
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "自动保存使用记录时出现错误");
        }
    }


    public void AddRecord(DateTime time, string? appName)
    {
        var i = appName == null ? -1 : _appIndexTable.GetIndex(appName);
        if (!_recordsSaveQueue.TryGetValue(time.Date, out var list))
        {
            list = new UsageRecordList();
            _recordsSaveQueue[time.Date] = list;
        }
        list.Add(new UsageRecord() { AppIndex = i, Span = time.TimeOfDay});
    }

    public async Task SaveAsync()
    {
        try
        {
            // 如果有新增app，保存app索引表
            if (_appIndexTable.CheckChange())
            {
                await File.WriteAllTextAsync(AppIndexSavePath, JsonConvert.SerializeObject(_appIndexTable));
            }

            if (_recordsSaveQueue.Count > 0)
            {
                foreach (var records in _recordsSaveQueue)
                {
                    // 获取日期对应的记录文件
                    var path = Path.Combine(DataManager.Instance.UsageSaveDir,
                        records.Key.ToString("yyyy-MM-dd") + ".json");
                    
                    FileHelper.CreateIfNotExist(path);

                    // 读取json并反序列化
                    var json = await File.ReadAllTextAsync(path);
                    var res = JsonConvert.DeserializeObject<UsageRecordList>(json) ?? new UsageRecordList();
                    // 将记录队列中的值与原值合并
                    res.AddRange(records.Value);
                    // 保存
                    await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(res));
                }

                _recordsSaveQueue.Clear();
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "保存使用记录时出现错误");
        }
    }
}