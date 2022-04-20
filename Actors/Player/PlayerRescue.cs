using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerRescue : PlayerNetworkBehaviour
{
    private static PlayerRescue instance;
    public static PlayerRescue GetInstance() => instance;

    [SyncVar]
    private float refRescueCd;

    [SyncVar(hook = nameof(OnRescueCdChanged))]
    private float rescueCd;

    private PlayerHealth playerHealth; // SERVER

    public override void OnStartServer()
    {
        base.OnStartServer();

        rescueCd = refRescueCd = LobbySettings.GetInstance().rescueCd;

        playerHealth = GetComponent<PlayerHealth>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;
    }

    public void Update()
    {
        if (isServer)
        {
            if (rescueCd > 0)
                rescueCd -= Time.deltaTime;
        }

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Rescue();
            }
        }
    }
    [ClientCallback]
    public void Rescue()
    {
        if (IsMoving)
            return;

        CmdRescue();
    }

    [Command]
    private void CmdRescue()
    {
        if (isOnHazard || isAvalanched || playerHealth.GetHealth() <= 0)
            return;

        if (rescueCd > 0)
            return;

        rescueCd = refRescueCd;

        var rescuee = GameObject.FindGameObjectsWithTag("Player")
            .FirstOrDefault(p =>
            {
                bool b1 = p.transform.position.x >= transform.position.x - 1 && p.transform.position.x <= transform.position.x + 1;
                bool b2 = p.transform.position.y >= transform.position.y - 1 && p.transform.position.y <= transform.position.y + 1;

                var playerRescue = p.GetComponent<PlayerRescue>();
                bool b3 = playerRescue.isOnHazard; // target of rescue must be on a hazard
                bool b4 = playerRescue.connectionToClient != connectionToClient; // target of rescue cannot be self

                return b1 && b2 && b3 && b4;
            });

        if (!rescuee)
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Unable to find rescuee");
            HazardEvents.GetInstance().eventPlayerFailRescue?.Invoke(this, new EventPlayerFailRescue(connectionToClient));
            return;
        }
        var rescueeNetId = rescuee.GetComponent<NetworkIdentity>();
        if (!rescueeNetId) 
            ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(rescueeNetId));

        ServerLog.Log($"{netId} Rescuing player {rescueeNetId.netId}");
        HazardEvents.GetInstance().eventPlayerRescued?.Invoke(this, new EventPlayerRescued(connectionToClient, rescueeNetId.connectionToClient));
    }

    private void OnRescueCdChanged(float oldValue, float newValue)
    {
        if (isLocalPlayer)
            UIBtnRescue.GetInstance().SetIsInteractable(newValue > 0 ? false : true);
    }
}