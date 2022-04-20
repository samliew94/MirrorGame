using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public enum MiniGameName : byte
{
    NONE,
    PLANT_FLAG,
    RECOLLECT_FLAG
}

public class PlayerMiniGame : PlayerNetworkBehaviour
{
    private static PlayerMiniGame instance;
    public static PlayerMiniGame GetInstance() => instance;

    public override void OnStartLocalPlayer()
    {
        instance = this;
    }

    protected override void OnPlayingMiniGameCallback(object sender, EventPlayingMiniGame e)
    {
        UIPlayer.GetInstance().ShowButtons(!isPlayingMiniGame);
    }
}