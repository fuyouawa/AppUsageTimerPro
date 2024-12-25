using System;
using System.Collections.Generic;
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

    public class TimerDayUsageTime
    {
        public DateTime Day;
        public TimeSpan Span;
    }

    public class TimerItem
    {
        public string Name { get; set; }

        public List<TimerDayUsageTime> DayUsageTimes { get; }

        public bool Parsing { get; set; }
        public List<DateTime> ParseDateRecords { get; }

        public List<ListenedApp> ListenedApps { get; }

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

                    time = new TimerDayUsageTime() { Day = DateTime.Today, Span = TimeSpan.Zero };
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

        public TimerItem(string name, string tag, List<ListenedApp> listenedApps)
            : this(name, tag, listenedApps, new List<TimerDayUsageTime>(), new List<DateTime>())
        {
        }

        public TimerItem(string name, string tag, List<ListenedApp> listenedApps, List<TimerDayUsageTime> dayUsageTimes, List<DateTime> parseDateRecords)
        {
            Name = name;
            Tag = tag;
            Status = TimerStatus.Standing;
            ListenedApps = listenedApps;
            DayUsageTimes = dayUsageTimes;
            ParseDateRecords = parseDateRecords;

            var today = DateTime.Today;
            if (dayUsageTimes.Count == 0)
            {
                dayUsageTimes[0] = new TimerDayUsageTime() { Day = today };
            }

            if (dayUsageTimes[^1].Day != today)
            {
                dayUsageTimes.Add(new TimerDayUsageTime() { Day = today });
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

            var listenedApps = obj["ListenedApps"]!.ToObject<List<ListenedApp>>()!;
            var dayUsageTimes = obj["DayUsageTimes"]!.ToObject<List<TimerDayUsageTime>>()!;

            var parsing = obj["Parsing"]!.ToObject<bool>();
            var parseDateRecords = obj["ParseDateRecords"]!.ToObject<List<DateTime>>()!;

            var res = new TimerItem(name, tag, listenedApps, dayUsageTimes, parseDateRecords)
            {
                Parsing = parsing
            };
            return res;
        }
    }
}
