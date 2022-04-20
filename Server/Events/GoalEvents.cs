using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;

public class GoalEvents : IEvent
{
    private static readonly Lazy<GoalEvents> lazy = new Lazy<GoalEvents>(() => new GoalEvents());
    public static GoalEvents GetInstance() => lazy.Value;
    private GoalEvents() { }

    public EventHandler<EventPlayerLandedOnGoal> eventPlayerLandedOnGoal;

    public void ClearEventSubcribers()
    {
        if (eventPlayerLandedOnGoal != null)
            foreach (var subscriber in eventPlayerLandedOnGoal.GetInvocationList())
                eventPlayerLandedOnGoal -= subscriber as EventHandler<EventPlayerLandedOnGoal>;
    }
}

public class EventPlayerLandedOnGoal : EventArgs
{
    public readonly NetworkConnection Conn;
    public readonly bool isLandedOnGoal;

    public EventPlayerLandedOnGoal(NetworkConnection conn, bool isLandedOnGoal)
    {
        Conn = conn;
        this.isLandedOnGoal = isLandedOnGoal;
    }
}

