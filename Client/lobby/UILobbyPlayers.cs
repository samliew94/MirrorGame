using System;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyPlayers : MonoBehaviour
{  
    [SerializeField] private UILobbyPlayer playerPrefab;
    [SerializeField] private List<UILobbyPlayer> uiLobbyPlayers;

    public void clear()
    {
        foreach (var uiLobbyPlayer in uiLobbyPlayers)
            Destroy(uiLobbyPlayer.gameObject);
    }

    public void addPlayer(Dictionary<string, object> kvp)
    {
        uiLobbyPlayers.Add(Instantiate(playerPrefab, transform));
    }

    public void setPlayersLobbyInfo(string json)
    {
        var mainKvp = JsonUtil.jsonToDynamic(json);
        var listKvp = JsonUtil.jArrayToListMap(mainKvp["global"]);

        foreach (var kvp in listKvp)
        {
            var player = Instantiate(playerPrefab).GetComponent<UILobbyPlayer>();
            player.displayName = kvp["displayName"].ToString();
            player.colorId = int.Parse(kvp["colorId"].ToString());

        }
    }
}