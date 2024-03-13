using AppUsageTimerPro.Tools;
using System;

namespace AppUsageTimerPro.Model
{
    public enum TimerStatus
    {
        Running,
        Pausing,
        Standing
    }

    public class TimerItem
    {

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private TimeSpan _totalUsageTime = new();

        public string TotalUsageTimeStr => _totalUsageTime.ToTimeString();

        private TimeSpan _todayUsageTime = new();

        private TimeSpan _continueUsageTime = new();

        public string TodayTimeStr => _todayUsageTime.ToTimeString();

        private string _tag;

        public string Tag
        {
            get => _tag;
            set => _tag = value;
        }

        private TimerStatus _status;

        public string StatusStr => _status switch
        {
            TimerStatus.Running => "运行中",
            TimerStatus.Pausing => "暂停中",
            _ => "待命中",
        };

        private string _appName;

        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }


        public TimerItem(string name, string tag, string appName)
        {
            _name = name;
            _tag = tag;
            _status = TimerStatus.Standing;
            _appName = appName;
        }
    }
}
