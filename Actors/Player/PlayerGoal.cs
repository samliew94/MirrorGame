using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerGoal : PlayerNetworkBehaviour
{
    private static PlayerGoal instance;
    public static PlayerGoal GetInstance() => instance;

    [SyncVar]
    private bool isLandedOnGoal;

    public override void OnStartServer()
    {
        base.OnStartServer();

        var goalEvents = GoalEvents.GetInstance();
        goalEvents.eventPlayerLandedOnGoal -= OnPlayerLandedOnGoal;
        goalEvents.eventPlayerLandedOnGoal += OnPlayerLandedOnGoal;
    }

    private void OnPlayerLandedOnGoal(object sender, EventPlayerLandedOnGoal e)
    {
        if (e.Conn == connectionToClient)
        {
            isLandedOnGoal = e.isLandedOnGoal;

            if (isLandedOnGoal)
            {
                var totalPlayers = NetworkServer.connections.Count;
                var playersLandedOnGoal = NetworkServer.connections.Values.Count(x => x.identity.GetComponent<PlayerGoal>().isLandedOnGoal);

                if (totalPlayers == playersLandedOnGoal) // players win
                {
                    ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"VICTORY!");
                    GameStateEvent.GetInstance().eventGameStateChanged?.Invoke(this, new EventGameStateChanged(GameState.WIN));
                }
            }
        }
    }
}
