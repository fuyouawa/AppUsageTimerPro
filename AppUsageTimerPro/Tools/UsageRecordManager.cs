using System.Collections.Generic;
using MessagePack;

namespace AppUsageTimerPro.Tools;

public struct UsageRecord
{
    public long TimeStamp;
    public string Process;
}

public class UsageRecordManager : Singleton<UsageRecordManager>
{
    private List<UsageRecord> _records = new();

    UsageRecordManager()
    {

    }

    public void AddRecord(UsageRecord record)
    {
        _records.Add(record);
        Save();
    }

    void Save()
    {
        MessagePackSerializer.Serialize(_records);
    }
}