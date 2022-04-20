using Mirror;
using System;

public class FlagEvents : IEvent
{
    private static readonly Lazy<FlagEvents> lazy = new Lazy<FlagEvents>(() => new FlagEvents());
    public static FlagEvents GetInstance() => lazy.Value;    
    private FlagEvents() { }

    public EventHandler<EventEnteredFlag> eventEnteredFlag;
    public EventHandler<EventExitedFlag> eventExitedFlag;

    public void ClearEventSubcribers()
    {
        if (eventEnteredFlag != null)
            foreach (var subscriber in eventEnteredFlag.GetInvocationList())
                eventEnteredFlag -= subscriber as EventHandler<EventEnteredFlag>;

        if (eventExitedFlag != null)
            foreach (var subscriber in eventExitedFlag.GetInvocationList())
                eventExitedFlag -= subscriber as EventHandler<EventExitedFlag>;
    }
}

public class EventEnteredFlag
{
    public readonly NetworkConnection Player;
    public readonly Flag Flag;
    public readonly int TriggerRingId;

    public EventEnteredFlag(NetworkConnection player, Flag flag, int triggerRingId)
    {
        Player = player;
        Flag = flag;
        TriggerRingId = triggerRingId;
    }

}
public class EventExitedFlag
{
    public readonly NetworkConnection Player;
    public readonly Flag Flag;
    public readonly int TriggerRingId;

    public EventExitedFlag(NetworkConnection player, Flag flag, int triggerRingId)
    {
        Player = player;
        Flag = flag;
        TriggerRingId = triggerRingId;
    }
}
