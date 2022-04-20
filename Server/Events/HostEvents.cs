using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HostEvents : IEvent
{
    private static readonly Lazy<HostEvents> lazy = new Lazy<HostEvents>(() => new HostEvents());
    public static HostEvents GetInstance() => lazy.Value;
    private HostEvents() { }

    public EventHandler<EventHostChanged> eventHostChanged;

    public void ClearEventSubcribers()
    {
        if(eventHostChanged != null)  
            foreach (var subscriber in eventHostChanged.GetInvocationList())
                eventHostChanged -= subscriber as EventHandler<EventHostChanged>;
    }
}

public class EventHostChanged : EventArgs
{
    public readonly NetworkConnection conn;
    public readonly bool isHost;

    public EventHostChanged(NetworkConnection conn, bool isHost)
    {
        this.conn = conn;
        this.isHost = isHost;
    }
}
