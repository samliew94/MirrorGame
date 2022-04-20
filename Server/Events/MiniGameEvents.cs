using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MiniGameEvents : IEvent
{
    private static readonly Lazy<MiniGameEvents> lazy = new Lazy<MiniGameEvents>(() => new MiniGameEvents());
    public static MiniGameEvents GetInstance() => lazy.Value;
    private MiniGameEvents() { }

    public EventHandler<EventPlayingMiniGame> eventPlayingMiniGame;

    public void ClearEventSubcribers()
    {
        if (eventPlayingMiniGame != null)
            foreach (var subscriber in eventPlayingMiniGame.GetInvocationList())
                eventPlayingMiniGame -= subscriber as EventHandler<EventPlayingMiniGame>;
    }

}

public class EventPlayingMiniGame : EventArgs
{
    public readonly NetworkConnection PlayerConn;
    public readonly bool IsPlayingMiniGame;
    public readonly MiniGameName MiniGameGame;

    public EventPlayingMiniGame(NetworkConnection playerConn, bool isPlayingMiniGame, MiniGameName miniGameGame)
    {
        this.PlayerConn = playerConn;
        this.IsPlayingMiniGame = isPlayingMiniGame;
        MiniGameGame = miniGameGame;
    }
}
