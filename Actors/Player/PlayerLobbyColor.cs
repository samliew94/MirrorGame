using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerLobbyColor : NetworkBehaviour
{
    private static PlayerLobbyColor instance;
    public static PlayerLobbyColor getInstance() => instance;

    [SyncVar]
    private Color color;

    private List<Color> colorList = new List<Color> { Color.red, Color.green, Color.blue };

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

        if (playerInfo.GetColor() == Color.clear) // no color
        {
            var availableColors = colorList.Except(playerManager.GetPlayerInfos().Select(x => x.GetColor()));
            var randomizedSelectedColor = availableColors.OrderBy(x => RNG.GetInstance().Random().Next()).FirstOrDefault();

            if (randomizedSelectedColor == null)
                throw new Exception($"{GetType().Name} {MethodBase.GetCurrentMethod().Name} can't find any availableColors.Count={availableColors.Count()}" );

            playerInfo.SetColor(randomizedSelectedColor);
        }

        color = playerInfo.GetColor(); // if you have existing color, use it
    }

    [Command]
    public void CmdChangeColor(int colorId)
    {
        
    }
}