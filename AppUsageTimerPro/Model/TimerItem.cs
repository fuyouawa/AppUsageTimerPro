using System;
using System.Collections.Generic;

namespace AppUsageTimerPro
{
    public enum TimerStatus
    {
        Running,
        Pausing,
        Standing
    }

    public class TimerItem
    {
        public string Name { get; set; }

        public TimeSpan TotalUsageTime;

        public string TotalUsageTimeStr => TotalUsageTime.ToDisplayTimeString();

        public TimeSpan TodayUsageTime;

        public TimeSpan ContinueUsageTime;

        public string TodayTimeStr => TodayUsageTime.ToDisplayTimeString();

        public string Tag { get; set; }

        public TimerStatus Status;

        public string StatusStr => Status switch
        {
            TimerStatus.Running => "运行中",
            TimerStatus.Pausing => "暂停中",
            _ => "待命中",
        };

        public List<ListenedProcess> ListenedProcesses;
        
        public TimerItem(string name, string tag, List<ListenedProcess> listenedProcesses)
            : this(name, tag, listenedProcesses, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero)
        {
        }
        
        public TimerItem(string name, string tag, List<ListenedProcess> listenedProcesses, TimeSpan todayUsageTime, TimeSpan totalUsageTime)
            : this(name, tag, listenedProcesses, todayUsageTime, totalUsageTime, TimeSpan.Zero)
        {
        }

        public TimerItem(string name, string tag, List<ListenedProcess> listenedProcesses, TimeSpan todayUsageTime, TimeSpan totalUsageTime, TimeSpan continueUsageTime)
        {
            Name = name;
            Tag = tag;
            Status = TimerStatus.Standing;
            ListenedProcesses = listenedProcesses;
            TodayUsageTime = todayUsageTime;
            TotalUsageTime = totalUsageTime;
            ContinueUsageTime = continueUsageTime;
        }
    }
}
