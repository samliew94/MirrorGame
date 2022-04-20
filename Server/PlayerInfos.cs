using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInfos : Dictionary<NetworkConnection, PlayerInfo>
{
    public IEnumerable<PlayerInfo> Get() => Values;
    public PlayerInfo Get(NetworkConnection conn)
    {
        if (!this.ContainsKey(conn))
            this[conn] = new PlayerInfo();

        return this[conn];
    }
}

public class PlayerInfo
{
    private string playerName;
    private Color color = Color.clear; // default
    private bool isHost;

    public string GetPlayerName() => playerName;
    public void SetPlayerName(string v) => playerName = v;
    public Color GetColor() => color;  
    public void SetColor(Color v) => color = v;
    public bool GetIsHost() => isHost;
    public void SetIsHost(bool v) => isHost = v;
}