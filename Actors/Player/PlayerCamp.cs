using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

public class PlayerCamp : PlayerNetworkBehaviour
{
    private static PlayerCamp instance;
    public static PlayerCamp GetInstance() => instance;

    private Camp camp;

    [SyncVar]
    private float campCd;

    public override void OnStartLocalPlayer() => instance = this;

    private List<CancellationTokenSource> ctses;
    private CancellationTokenSource cts_broadcastCampData;


    public void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.C))
                CmdShowCampData();
        }

        if (isServer)
        {
            if (campCd > 0)
                campCd -= Time.deltaTime;
        }
    }

    [Command]
    private void CmdShowCampData()
    {
        if (campCd > 0)
            return;

        campCd = 1;

        if (cts_broadcastCampData != null) // maybe user activate too fast after closing it?
            return;

        BroadcastCampData();
    }

    [ServerCallback]
    private async void BroadcastCampData()
    {
        cts_broadcastCampData = new CancellationTokenSource();

        try
        {
            while (true)
            {
                await Task.Delay(1000);

                if (cts_broadcastCampData.IsCancellationRequested)
                    cts_broadcastCampData.Token.ThrowIfCancellationRequested();

                if (!camp)
                {
                    ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Attempting to get players distance when camp==null");
                    return;
                }

                var listMap = new List<Dictionary<string, object>>();

                foreach (var conn in NetworkServer.connections.Values)
                {
                    var map = new Dictionary<string, object>();

                    var playerName = PlayerManager.GetInstance().GetPlayerInfo(conn).GetPlayerName();
                    var playerPosition = conn.identity.transform.position;
                    var playerHealth = conn.identity.GetComponent<PlayerHealth>().GetHealth();

                    map["playerName"] = playerName;
                    map["distanceAwayFromCamp"] = (int)Vector2.Distance(playerPosition, camp.transform.position);
                    map["playerHealth"] = playerHealth;

                    listMap.Add(map);
                }

                var json = JsonUtil.toJson(listMap);
                TargetShowCampTools(json);

            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            cts_broadcastCampData.Dispose();
            cts_broadcastCampData = null;
        }
    }

    [ServerCallback]
    protected override void OnEnteredCampCallback(object sender, EventEnteredCamp e)
    {
        camp = e.Camp;
        TargetEnableCampButton(true);
    }

    [ServerCallback]
    protected override void OnExitedCampCallback(object sender, EventExitedCamp e)
    {
        camp = null;
        TargetEnableCampButton(false);
    }

    [TargetRpc]
    private void TargetEnableCampButton(bool v) => UIBtnCamp.GetInstance().SetIsInteractable(v);

    /// <summary>
    /// shows vital signs of players and distance from camp
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    [TargetRpc]
    private void TargetShowCampTools(string json) => UICampData.GetInstance().ShowCampTools(json);

    [TargetRpc]
    private void TargetHideCampTools() => UICampData.GetInstance().Hide();

    [Client]
    public void HideCampTools()
    {
        TargetHideCampTools();
        CmdHideCampTools();
    }

    [Command]
    private void CmdHideCampTools() => cts_broadcastCampData?.Cancel();

    protected override void OnDestroyServer() => cts_broadcastCampData?.Cancel();
}
