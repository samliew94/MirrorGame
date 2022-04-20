using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ColorEvents
{
    private static readonly Lazy<ColorEvents> lazy = new Lazy<ColorEvents>(() => new ColorEvents());
    public static ColorEvents getInstance() => lazy.Value;
    private ColorEvents()
    {
    }

    public EventHandler<EventChangeColor> eventChangeColor; // receiver should be PlayerColorManager
    public EventHandler<EventChangedColor> eventChangedColor; // receiver should be PlayerLobbyColor

    public void changeColor(object sender, int oldColorId, int newColorId) => eventChangeColor?.Invoke(sender,  new EventChangeColor(oldColorId, newColorId));
    public void changedColor(object sender, List<int> takenColors) => eventChangedColor?.Invoke(sender,  new EventChangedColor(takenColors));
}

public class EventChangeColor : EventArgs
{
    public readonly int oldColorId;
    public readonly int newColorId;

    public EventChangeColor(int oldColorId, int newColorId)
    {
        this.oldColorId = oldColorId;
        this.newColorId = newColorId;
    }
}

public class EventChangedColor : EventArgs
{
    public readonly List<int> takenColors;

    public EventChangedColor(List<int> takenColors)
    {
        this.takenColors = takenColors;
    }
}
