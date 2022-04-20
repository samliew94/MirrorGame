using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerLobbyName : NetworkBehaviour
{
    private static PlayerLobbyName instance;
    public static PlayerLobbyName GetInstance() => instance;

    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    private string playerName;


    private string[] firstNames = new string[] { "Tom", "Dick", "Harry" };
    private string[] lastNames = new string[] { "Smith", "Rogers", "Ruffalo" };

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var playerManager = PlayerManager.GetInstance();
        var playerInfo = playerManager.GetPlayerInfo(connectionToClient);

        if (playerInfo.GetPlayerName() == null) // no name
        {
            // randomized first name
            var firstName = firstNames.OrderBy(x => RNG.GetInstance().Random().Next()).First();
            var lastName = lastNames.OrderBy(x => RNG.GetInstance().Random().Next()).First();

            playerInfo.SetPlayerName(firstName + " " + lastName);
        }

        playerName = playerInfo.GetPlayerName();
    }
    private void OnPlayerNameChanged(string oldName, string newName)
    {
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"PlayerName={newName}");
    }
}