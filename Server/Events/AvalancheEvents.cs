using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class AvalancheEvents : IEvent
{
    private static readonly Lazy<AvalancheEvents> lazy = new Lazy<AvalancheEvents>(() => new AvalancheEvents());
    public static AvalancheEvents GetInstance() => lazy.Value;
    private AvalancheEvents() { }

    public EventHandler<EventAvalancheForewarn> eventAvalancheForewarn;
    public EventHandler<EventAvalanche> eventAvalanche;
    public EventHandler<EventAvalancheStopped> eventAvalancheStopped;

    public void ClearEventSubcribers()
    {
        if (eventAvalancheForewarn != null)
            foreach (var subscriber in eventAvalancheForewarn.GetInvocationList())
                eventAvalancheForewarn -= subscriber as EventHandler<EventAvalancheForewarn>;

        if (eventAvalanche != null)
            foreach (var subscriber in eventAvalanche.GetInvocationList())
                eventAvalanche -= subscriber as EventHandler<EventAvalanche>;

        if (eventAvalancheStopped != null)
            foreach (var subscriber in eventAvalancheStopped.GetInvocationList())
                eventAvalancheStopped -= subscriber as EventHandler<EventAvalancheStopped>;
    }
}

public class EventAvalancheForewarn : EventArgs
{
    public readonly int WarningsIssued;
    public readonly NetworkConnection [] Conns;

    public EventAvalancheForewarn(int warningsIssued, params NetworkConnection [] conns)
    {
        WarningsIssued = warningsIssued;
        Conns = conns;
    }
}

public class EventAvalanche : EventArgs
{
    public readonly NetworkConnection [] Conns;

    public EventAvalanche(params NetworkConnection [] conns)
    {
        Conns = conns;
    }
}
public class EventAvalancheStopped : EventArgs
{
    public readonly NetworkConnection [] Conns;

    public EventAvalancheStopped(params NetworkConnection [] conns)
    {
        Conns = conns;
    }
}
