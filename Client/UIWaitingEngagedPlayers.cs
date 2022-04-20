using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIWaitingEngagedPlayers : MonoBehaviour
{
    private List<UIWaitingPlayersPlayerItem> playerItems;

    private void Awake()
    {
        playerItems = GetComponentsInChildren<UIWaitingPlayersPlayerItem>(true).ToList();
    }

    public void setData(string json)
    {
        var map = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        var allPlayersData = JsonUtil.jArrayToListMap(map["allPlayersData"]);

        playerItems.ForEach(x => x.gameObject.SetActive(false));

        for (int i = 0; i < allPlayersData.Count; i++)
        {
            var playerData = allPlayersData[i];
            var playerItem = playerItems[i];

            var state = (int)(long)playerData["state"];

            playerItem.setState(state);
            playerItem.setDisplayName(playerData["displayName"].ToString());
        }

    }
}
