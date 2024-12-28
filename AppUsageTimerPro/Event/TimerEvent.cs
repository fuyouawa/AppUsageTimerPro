using System;
using System.Collections.Generic;

namespace AppUsageTimerPro;

public enum TimerChangedTypes
{
    SpanOfTodayUsageTime
}

public record AddTimerEvent(TimerItem Timer);
public record RemoveTimerEvent(string TimerName);
public record TimerChangedEvent(string TimerName, TimerChangedTypes ChangedType, object Value);

public record LoadedTimersEvent();

public record GetTimersReq();
public record GetTimersRes(List<TimerItem> Timers);