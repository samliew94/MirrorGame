using Mirror;
using System;

public class HealthEvents : IEvent
{
    private static readonly Lazy<HealthEvents> lazy = new Lazy<HealthEvents>(() => new HealthEvents());
    public static HealthEvents GetInstance() => lazy.Value;

    private HealthEvents() { }

    public EventHandler<EventHealthChanged> eventHealthChanged;
    public EventHandler<EventAddHealth> eventAddHealth;

    public void ClearEventSubcribers()
    {
        if (eventHealthChanged != null)
            foreach (var subscriber in eventHealthChanged.GetInvocationList())
                eventHealthChanged -= subscriber as EventHandler<EventHealthChanged>;

        if (eventAddHealth != null)
            foreach (var subscriber in eventAddHealth.GetInvocationList())
                eventAddHealth -= subscriber as EventHandler<EventAddHealth>;
    }
}

public class EventHealthChanged
{
    public readonly NetworkConnection Player;
    public readonly int Health;

    public EventHealthChanged(NetworkConnection player, int health)
    {
        Player = player;
        Health = health;
    }
}
public class EventAddHealth
{
    public readonly NetworkConnection Player;
    public readonly int AddHealth;

    public EventAddHealth(NetworkConnection player, int addHealth)
    {
        Player = player;
        AddHealth = addHealth;
    }
}

