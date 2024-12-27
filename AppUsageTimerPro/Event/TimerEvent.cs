using System;
using System.Collections.Generic;

namespace AppUsageTimerPro;

public enum TimerChangedTypes
{
    SpanOfTodayUsageTime
}

public record AddTimerEvent(TimerItem Timer);
public record RemoveTimerEvent(TimerItem Timer);
public record TimerChangedEvent(string TimerName, TimerChangedTypes ChangedType, object Value);

public record ReloadTimersEvent(List<TimerItem> Timers);