using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppUsageTimerPro
{
    public enum TimerStatus
    {
        Running,
        Pausing,
        Standing
    }

    public class TimerDayUsageTime : ICloneable
    {
        public DateTime Day;
        public TimeSpan Span;

        public TimerDayUsageTime(DateTime day, TimeSpan span)
        {
            Day = day;
            Span = span;
        }

        public object Clone()
        {
            return new TimerDayUsageTime(Day, Span);
        }
    }

    public class DayUsageTimeList : List<TimerDayUsageTime>
    {
        public DayUsageTimeList() {}
        public DayUsageTimeList(int capacity) : base(capacity) { }
        public DayUsageTimeList(IEnumerable<TimerDayUsageTime> collection) : base(collection) {}
    }

    public class ParseDateRecordList : List<DateTime>
    {
        public ParseDateRecordList() {}
        public ParseDateRecordList(int capacity) : base(capacity) { }
        public ParseDateRecordList(IEnumerable<DateTime> collection) : base(collection) {}
    }

    public class ListenedAppList : List<ListenedApp>
    {
        public ListenedAppList() {}
        public ListenedAppList(int capacity) : base(capacity) { }
        public ListenedAppList(IEnumerable<ListenedApp> collection) : base(collection) {}

        public bool ContainAppName(string appName)
        {
            return this.FirstOrDefault(app => app.Name == appName) != null;
        }
    }

    public class TimerItemList : List<TimerItem>
    {
        public TimerItemList() {}
        public TimerItemList(int capacity) : base(capacity) { }
        public TimerItemList(IEnumerable<TimerItem> collection) : base(collection) {}
    }

    public class TimerItem : ViewModelBase, ICloneable
    {
        public string Name { get; set; }

        public DayUsageTimeList DayUsageTimes { get; }

        public bool Pausing { get; set; }
        public ParseDateRecordList ParseDateRecords { get; }

        public ListenedAppList ListenedApps { get; }

        public string Tag { get; set; }

        public bool Forcing { get; set; }
        
        /// <summary>
        /// 除了今天以外的使用时间总和
        /// </summary>
        private TimeSpan _totalUsageSpanExceptToday;
        
        public TimeSpan TotalUsageSpan => _totalUsageSpanExceptToday + TodayUsageTime.Span;

        public TimerDayUsageTime TodayUsageTime
        {
            get
            {
                var time = DayUsageTimes[^1];
                if (time.Day != DateTime.Today)
                {
                    _totalUsageSpanExceptToday += time.Span;

                    time = new TimerDayUsageTime(DateTime.Today, TimeSpan.Zero);
                    DayUsageTimes.Add(time);
                }
                return time;
            }
        }
        public string TotalUsageSpanDisplay => TotalUsageSpan.ToDisplayTimeString();

        public string TodayUsageSpanDisplay => TodayUsageTime.Span.ToDisplayTimeString();

        public string StatusDisplay
        {
            get
            {
                if (Pausing)
                    return "暂停中";
                if (Forcing)
                    return "使用中";
                return "待命中";
            }
        }

        public TimerItem(string name, string tag, ListenedAppList listenedApps)
            : this(name, tag, listenedApps, new DayUsageTimeList(), new ParseDateRecordList())
        {
        }

        public TimerItem(string name, string tag, ListenedAppList listenedApps, DayUsageTimeList dayUsageTimes, ParseDateRecordList parseDateRecords)
        {
            Name = name;
            Tag = tag;
            ListenedApps = listenedApps;
            DayUsageTimes = dayUsageTimes;
            ParseDateRecords = parseDateRecords;

            var today = DateTime.Today;
            if (dayUsageTimes.Count == 0 || dayUsageTimes[^1].Day != today)
            {
                dayUsageTimes.Add(new TimerDayUsageTime(today, TimeSpan.Zero));
            }

            for (int i = 0; i < dayUsageTimes.Count - 1; i++)
            {
                _totalUsageSpanExceptToday += dayUsageTimes[i].Span;
            }
        }

        public void SetPause(bool pause)
        {
            if (pause)
            {
                if (Pausing) return;
                Pausing = true;
                ParseDateRecords.Add(DateTime.Now);
            }
            else
            {
                Pausing = false;
            }
        }

        public object Clone()
        {
            return new TimerItem(Name, Tag, ListenedApps, DayUsageTimes, ParseDateRecords)
            {
                Pausing = Pausing,
                Forcing = Forcing
            };
        }
    }


    public class TimerItemConverter : JsonConverter<TimerItem>
    {
        public override void WriteJson(JsonWriter writer, TimerItem? value, JsonSerializer serializer)
        {
            if (value == null)
                return;

            writer.WriteStartObject();

            writer.WritePropertyName("Name");
            writer.WriteValue(value.Name);

            writer.WritePropertyName("Tag");
            writer.WriteValue(value.Tag);

            writer.WritePropertyName("ListenedApps");
            serializer.Serialize(writer, value.ListenedApps);

            writer.WritePropertyName("DayUsageTimes");
            serializer.Serialize(writer, value.DayUsageTimes);

            writer.WritePropertyName("Pausing");
            writer.WriteValue(value.Pausing);

            writer.WritePropertyName("ParseDateRecords");
            serializer.Serialize(writer, value.ParseDateRecords);

            writer.WriteEndObject();
        }

        public override TimerItem? ReadJson(JsonReader reader, Type objectType, TimerItem? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            if (!obj.HasValues) return null;

            var name = obj["Name"]!.ToString();
            var tag = obj["Tag"]!.ToString();

            var listenedApps = obj["ListenedApps"]!.ToObject<ListenedAppList>()!;
            var dayUsageTimes = obj["DayUsageTimes"]!.ToObject<DayUsageTimeList>()!;

            var parsing = obj["Pausing"]!.ToObject<bool>();
            var parseDateRecords = obj["ParseDateRecords"]!.ToObject<ParseDateRecordList>()!;

            var res = new TimerItem(name, tag, listenedApps, dayUsageTimes, parseDateRecords)
            {
                Pausing = parsing
            };
            return res;
        }
    }
}
