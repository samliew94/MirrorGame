using Mirror;
using System;

public class CampEvents : IEvent
{
    private static readonly Lazy<CampEvents> lazy = new Lazy<CampEvents>(() => new CampEvents());
    public static CampEvents GetInstance() => lazy.Value;    

    private CampEvents() { }

    public EventHandler<EventEnteredCamp> eventEnteredCamp;
    public EventHandler<EventExitedCamp> eventExitedCamp;

    public void ClearEventSubcribers()
    {
        if (eventEnteredCamp != null)
            foreach (var subscriber in eventEnteredCamp.GetInvocationList())
                eventEnteredCamp -= subscriber as EventHandler<EventEnteredCamp>;

        if (eventExitedCamp != null)
            foreach (var subscriber in eventExitedCamp.GetInvocationList())
                eventExitedCamp -= subscriber as EventHandler<EventExitedCamp>;
    }
}

public class EventEnteredCamp
{
    public readonly NetworkConnection Player;
    public readonly Camp Camp;
    public readonly bool IsAddHealth;

    public EventEnteredCamp(NetworkConnection player, Camp camp, bool isAddHealth)
    {
        Player = player;
        Camp = camp;
        IsAddHealth = isAddHealth;
    }
}
public class EventExitedCamp
{
    public readonly NetworkConnection Player;
    public readonly Camp Camp;

    public EventExitedCamp(NetworkConnection player, Camp camp)
    {
        Player = player;
        Camp = camp;
    }
}


