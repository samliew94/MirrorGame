using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Server 
/// </summary>
public class Avalanche : ISubscribeEvents, IAsync
{
    private static readonly Lazy<Avalanche> lazy = new Lazy<Avalanche>(() => new Avalanche());
    public static Avalanche GetInstance() => lazy.Value;
    private Avalanche() { }

    private int warningsIssued = -1; // how many warningsIssued (0 is a no-sign warning)

    private CancellationTokenSource ctsScheduleAvalanche;

    public void Subscribe()
    {
        GameStateEvent gameStateEvent = GameStateEvent.GetInstance();
        gameStateEvent.eventGameStateChanged -= onGameStateChanged;
        gameStateEvent.eventGameStateChanged += onGameStateChanged;
    }

    private void onGameStateChanged(object sender, EventGameStateChanged e)
    {
        if (e.GameState == GameState.START)
        {
            //ScheduleAvalanche();
            //IssueAvalanche();
        }
        else if (e.GameState == GameState.LOSE)
        {
            ctsScheduleAvalanche?.Cancel();
        }
    }

    private async void ScheduleAvalanche()
    {
        try
        {
            ctsScheduleAvalanche = new CancellationTokenSource();

            while(true)
            {
                warningsIssued++;

                if (warningsIssued == 0) // silent warning
                    await Task.Delay(5000); // 1m30s 90000
                if (warningsIssued == 1) // shake1
                    await Task.Delay(3000); // 20s 20000
                else if (warningsIssued == 2) // shake2
                    await Task.Delay(3000); // 10s 10000
                else if (warningsIssued == 3) // shake3
                    await Task.Delay(3000); // 5s
                else if (warningsIssued == 4) // avalanche
                    await Task.Delay(3000); // 5s

                if (ctsScheduleAvalanche.IsCancellationRequested)
                    ctsScheduleAvalanche.Token.ThrowIfCancellationRequested();

                if (warningsIssued == 4) // after 4 warnings issued, create avalanche
                    IssueAvalanche();
                else
                {
                    if (warningsIssued == 0)
                        continue;

                    AvalancheEvents avalancheEvents = AvalancheEvents.GetInstance();
                    //avalancheEvents.eventAvalancheForewarn?.Invoke(this, new EventAvalancheForewarn(warningsIssued, NetworkServer.connections.Values.ToArray()));
                    ServerLog.Log($"Avalanche warningsIssued={warningsIssued}");
                }
            }
        }
        catch (Exception)
        {
            
        }   
        finally
        {
            ctsScheduleAvalanche.Dispose();
            ctsScheduleAvalanche = null;
        }
    }

    private async void IssueAvalanche()
    {
        await Task.Delay(1000);
        warningsIssued = -1; // reset

        bool killAllPlayers = false;

        var players = NetworkServer.connections.Values;

        //if (players.Count(x => x.identity.GetComponent<PlayerBrace>().GetIsBracing()) != players.Count) // someone did not activate Brace
        //    killAllPlayers = true;
        //else if (players.Select(x => x.identity.transform.position).Distinct().Count() != 1) // 1 or more players are not standing in the same position
        //    killAllPlayers = true;

        AvalancheEvents avalancheEvents = AvalancheEvents.GetInstance();
        avalancheEvents.eventAvalanche?.Invoke(this, new EventAvalanche(NetworkServer.connections.Values.ToArray())); // shake + blankBlack

        ServerLog.Log($"Avalanche triggered. killAllPlayers={killAllPlayers}");

        await Task.Delay(2000);

        if (killAllPlayers)
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"LOSE to avalanche");
            GameStateEvent.GetInstance().eventGameStateChanged?.Invoke(this, new EventGameStateChanged(GameState.LOSE));
        }
        else
        {            
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"RESUME from avalanche");
            avalancheEvents.eventAvalancheStopped?.Invoke(this, new EventAvalancheStopped(NetworkServer.connections.Values.ToArray()));
        }
    }

    public void KillAsyncFunctions() => ctsScheduleAvalanche?.Cancel();
}
