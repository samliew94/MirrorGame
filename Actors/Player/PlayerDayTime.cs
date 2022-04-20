using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDayTime : PlayerNetworkBehaviour
{
    private static PlayerDayTime instance;
    public static PlayerDayTime GetInstance() => instance;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;
    }

    protected override void OnTimeChanged(object sender, EventTimeChanged e)
    {
        base.OnTimeChanged(sender, e);

        UIDayTime.GetInstance().SetTime(e.Time);
    }

    protected override void OnDayChanged(object sender, EventIsNightChanged e)
    {
        base.OnDayChanged(sender, e);

        UIDayTime.GetInstance().SetDay(e.IsNight);
    }

    

    
}
