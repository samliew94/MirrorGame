using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerLobbyHost : NetworkBehaviour
{
    private static PlayerLobbyHost instance;
    public static PlayerLobbyHost GetInstance() => instance;

    [SyncVar(hook = nameof(OnHostChanged))]
    private bool isHost;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        var playerInfo = PlayerManager.GetInstance().GetPlayerInfo(connectionToClient);

        var anyExistingHost = PlayerManager.GetInstance().GetPlayerInfos().Any(x => x.GetIsHost());

        if (!anyExistingHost)
            playerInfo.SetIsHost(true);

        isHost = playerInfo.GetIsHost();
    }

    private void OnHostChanged(bool oldValue, bool newValue)
    {
        if (isLocalPlayer)
            UILobbyHost.GetInstance().HostChanged(newValue);
    }

    [Command]
    public void CmdStartGame()
    {
        var playerInfo = PlayerManager.GetInstance().GetPlayerInfo(connectionToClient);

        if (!playerInfo.GetIsHost())
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Non-Host attempts to start game");
            return;
        }

        MyNetwork.getInstance().StartGame();
    }
}