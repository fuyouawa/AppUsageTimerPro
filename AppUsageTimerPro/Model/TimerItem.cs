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

        private DateTime _usageTime;

        public string UsageTimeStr => _usageTime.ToShortTimeString();

        private string[] _tags;

        public string TagStr => string.Join(", ", _tags);

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


        public TimerItem(string name, string[] tags, string appName)
        {
            _name = name;
            _tags = tags;
            _status = TimerStatus.Standing;
            _appName = appName;
        }
    }
}
