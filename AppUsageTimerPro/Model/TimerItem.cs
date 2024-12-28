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

    public class TimerItem : ViewModelBase, ICloneable
    {
        public string Name { get; set; }

        public DayUsageTimeList DayUsageTimes { get; }

        public bool Parsing { get; set; }
        public ParseDateRecordList ParseDateRecords { get; }

        public ListenedAppList ListenedApps { get; }

        public string Tag { get; set; }
        
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
        
        public TimerStatus Status { get; set; }
        
        public string StatusDisplay => Status switch
        {
            TimerStatus.Running => "运行中",
            TimerStatus.Pausing => "暂停中",
            _ => "待命中",
        };

        public TimerItem(string name, string tag, ListenedAppList listenedApps)
            : this(name, tag, listenedApps, new DayUsageTimeList(), new ParseDateRecordList())
        {
        }

        public TimerItem(string name, string tag, ListenedAppList listenedApps, DayUsageTimeList dayUsageTimes, ParseDateRecordList parseDateRecords)
        {
            Name = name;
            Tag = tag;
            Status = TimerStatus.Standing;
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

        public void SetParse(bool parse)
        {
            if (parse)
            {
                if (Parsing) return;
                Parsing = true;
                ParseDateRecords.Add(DateTime.Now);
            }
            else
            {
                Parsing = false;
            }
        }

        public void ApplyChange(TimerChangedTypes changedTypes, object value, bool withEvent = false)
        {
            switch (changedTypes)
            {
                case TimerChangedTypes.SpanOfTodayUsageTime:
                    TodayUsageTime.Span = (TimeSpan)value;
                    if (withEvent)
                    {
                        OnPropertyChanged(nameof(TodayUsageSpanDisplay));
                        OnPropertyChanged(nameof(TotalUsageSpanDisplay));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(changedTypes), changedTypes, null);
            }
        }

        public object Clone()
        {
            return new TimerItem(Name, Tag, ListenedApps, DayUsageTimes, ParseDateRecords)
            {
                Parsing = Parsing,
                Status = Status
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

            writer.WritePropertyName("Parsing");
            writer.WriteValue(value.Parsing);

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

            var parsing = obj["Parsing"]!.ToObject<bool>();
            var parseDateRecords = obj["ParseDateRecords"]!.ToObject<ParseDateRecordList>()!;

            var res = new TimerItem(name, tag, listenedApps, dayUsageTimes, parseDateRecords)
            {
                Parsing = parsing
            };
            return res;
        }
    }
}
