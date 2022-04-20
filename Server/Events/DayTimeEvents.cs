using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;

public class DayTimeEvents : IEvent
{
    private static readonly Lazy<DayTimeEvents> lazy = new Lazy<DayTimeEvents>(() => new DayTimeEvents());
    public static DayTimeEvents GetInstance() => lazy.Value;
    private DayTimeEvents() { }

    public EventHandler<EventTimeChanged> eventTimeChanged;
    public EventHandler<EventIsNightChanged> eventIsNightChanged;

    public void ClearEventSubcribers()
    {
        if (eventTimeChanged != null)
            foreach (var subscriber in eventTimeChanged.GetInvocationList())
                eventTimeChanged -= subscriber as EventHandler<EventTimeChanged>;

        if (eventIsNightChanged != null)
            foreach (var subscriber in eventIsNightChanged.GetInvocationList())
                eventIsNightChanged -= subscriber as EventHandler<EventIsNightChanged>;
    }
}

public class EventTimeChanged : EventArgs
{
    public readonly int Time;

    public EventTimeChanged(int time)
    {
        Time = time;
    }
}

public class EventIsNightChanged : EventArgs
{
    public readonly bool IsNight;

    public EventIsNightChanged(bool isNight)
    {
        IsNight = isNight;
    }
}


