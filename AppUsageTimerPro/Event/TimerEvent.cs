using System;
using System.Collections.Generic;

namespace AppUsageTimerPro;

public record AddTimerEvent(TimerItem Timer);
public record RemoveTimerEvent(string TimerName);
public record TimerTodayUsageTimeSpanChangedEvent(string TimerName, TimeSpan Span);
public record TimerPauseChangedEvent(string TimerName, bool Pause);

public record LoadedTimersEvent();

public record GetTimersReq();
public record GetTimersRes(TimerItemList Timers);

public record PrepareClosingEvent();
public record LogicTaskClosedEvent();