using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager 
{
    private static readonly Lazy<PlayerManager> lazy = new Lazy<PlayerManager>(() => new PlayerManager());
    public static PlayerManager GetInstance() => lazy.Value;
    private PlayerManager() { }

    private PlayerInfos playerInfoManager = new PlayerInfos();
    private PlayerStats playerStats = new PlayerStats();

    public void ReplacePlayers(PlayerPrefab playerPrefab)
    {
        foreach (var conn in NetworkServer.connections.Values)
            ReplacePlayer(conn, playerPrefab);
    }

    private void ReplacePlayer(NetworkConnection conn, PlayerPrefab playerPrefab, bool isDestroy = true)
    { 
        MyNetwork network = MyNetwork.getInstance(); // so as long as the code has this line, will erorr...

        GameObject oldPlayer = conn.identity.gameObject;
        GameObject newPlayer;
        string newPlayerPrefabName = null;

        if (playerPrefab == PlayerPrefab.LOBBY)
            newPlayerPrefabName = "PlayerLobby";
        else if (playerPrefab == PlayerPrefab.GAME)
            newPlayerPrefabName = "PlayerGame";

        var playerSpawnPosY = RNG.GetInstance().Random().Next(0, LobbySettings.GetInstance().height); // randomize player's spawn position (y-axis only)
        newPlayer = GameObject.Instantiate(network.findSpawnPrefabByName(newPlayerPrefabName));
        
        NetworkServer.ReplacePlayerForConnection(conn, newPlayer);

        if (isDestroy)
            NetworkServer.Destroy(oldPlayer);
    }

    public IEnumerable<PlayerInfo> GetPlayerInfos() => playerInfoManager.Get();
    public PlayerInfo GetPlayerInfo(NetworkConnection conn) => playerInfoManager.Get(conn);
    public IEnumerable<PlayerStat> GetPlayerStats() => playerStats.Get();
    public PlayerStat GetPlayerStats(NetworkConnection conn) => playerStats.Get(conn);
}

public enum PlayerPrefab
{
    LOBBY,
    GAME,
}

