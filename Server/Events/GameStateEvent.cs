using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameStateEvent : IEvent
{
    private static readonly Lazy<GameStateEvent> lazy = new Lazy<GameStateEvent>(() => new GameStateEvent());
    public static GameStateEvent GetInstance() => lazy.Value;    

    private GameStateEvent() { }

    public EventHandler<EventGameStateChanged> eventGameStateChanged;

    public void ClearEventSubcribers()
    {
        if (eventGameStateChanged != null)
            foreach (var subscriber in eventGameStateChanged.GetInvocationList())
                eventGameStateChanged -= subscriber as EventHandler<EventGameStateChanged>;
    }
}

public enum GameState
{
    START,
    WIN,
    LOSE
}

public class EventGameStateChanged : EventArgs
{
    public readonly GameState GameState;

    public EventGameStateChanged(GameState gameState)
    {
        this.GameState = gameState;
    }
}