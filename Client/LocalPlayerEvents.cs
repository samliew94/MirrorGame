using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class LocalPlayerEvents
{
    private LocalPlayerEvents() { }
    private static readonly Lazy<LocalPlayerEvents> lazy = new Lazy<LocalPlayerEvents>(() => new LocalPlayerEvents());
    public static LocalPlayerEvents getInstance() => lazy.Value;

    public EventHandler eventStopPlayingMiniGame;
    public void stopPlayingMiniGame() => eventStopPlayingMiniGame.Invoke(null, EventArgs.Empty);

    public EventHandler eventExitHiding;
    public void exitHiding() => eventExitHiding.Invoke(null, EventArgs.Empty);






}
