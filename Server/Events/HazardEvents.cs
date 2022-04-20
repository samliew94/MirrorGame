using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;

public class HazardEvents : IEvent
{
    private static readonly Lazy<HazardEvents> lazy = new Lazy<HazardEvents>(() => new HazardEvents());
    public static HazardEvents GetInstance() => lazy.Value;
    private HazardEvents() { }

    public EventHandler<EventPlayerOnHazard> eventPlayerOnHazard;
    public EventHandler<EventPlayerRescued> eventPlayerRescued;
    public EventHandler<EventPlayerFailRescue> eventPlayerFailRescue;

    public void ClearEventSubcribers()
    {
        if (eventPlayerOnHazard != null)
            foreach (var subscriber in eventPlayerOnHazard.GetInvocationList())
                eventPlayerOnHazard -= subscriber as EventHandler<EventPlayerOnHazard>;

        if (eventPlayerRescued != null)
            foreach (var subscriber in eventPlayerRescued.GetInvocationList())
                eventPlayerRescued -= subscriber as EventHandler<EventPlayerRescued>;

        if (eventPlayerFailRescue != null)
            foreach (var subscriber in eventPlayerFailRescue.GetInvocationList())
                eventPlayerFailRescue -= subscriber as EventHandler<EventPlayerFailRescue>;
    }
}

public class EventPlayerRescued : EventArgs
{
    public readonly NetworkConnection Rescuer;
    public readonly NetworkConnection Rescuee;

    public EventPlayerRescued(NetworkConnection rescuer, NetworkConnection rescuee)
    {
        Rescuer = rescuer;
        Rescuee = rescuee;
    }
}

public class EventPlayerOnHazard : EventArgs
{
    public readonly NetworkConnection Player;

    public EventPlayerOnHazard(NetworkConnection player)
    {
        Player = player;
    }
}

public class EventPlayerFailRescue : EventArgs
{
    public readonly NetworkConnection Player;

    public EventPlayerFailRescue(NetworkConnection player)
    {
        Player = player;
    }
}

