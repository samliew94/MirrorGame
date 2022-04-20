using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerFlare : PlayerNetworkBehaviour
{
    private static PlayerFlare instance;
    public static PlayerFlare GetInstance() => instance;

    [SyncVar]
    private float refFlareCd;

    [SyncVar(hook = nameof(OnFlareCdChanged))]
    private float flareCd;

    [SyncVar]
    private int remainingFlares;

    [SyncVar]
    private int refRemainingFlares;

    private PlayerHealth playerHealth;

    public override void OnStartServer()
    {
        base.OnStartServer();

        flareCd = refFlareCd = LobbySettings.GetInstance().flareCd;
        remainingFlares = refRemainingFlares = LobbySettings.GetInstance().remainingFlares;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;

        playerHealth = GetComponent<PlayerHealth>();
        if (!playerHealth)
            ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(playerHealth));
    }

    private void Update()
    {
        if(isServer)
        {
            if (flareCd > 0)
                flareCd -= Time.deltaTime;
        }

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ShootFlare();
            }
        }
    }

    [ClientCallback]
    public void ShootFlare()
    {
        if (!IsMoving)
            CmdShootFlare(transform.position);
    }

    [Command]
    private void CmdShootFlare(Vector2 pos)
    {
        if (isOnHazard || isAvalanched)
            return;

        if (flareCd > 0)
        {
            ServerLog.Log($"flareCd = {flareCd}");
            return;
        }

        if (remainingFlares <= 0)
        {
            ServerLog.Log($"Insufficient flares. remainingFlares = {remainingFlares}");
            return;
        }

        flareCd = refFlareCd;
        remainingFlares -= 1;
        ServerLog.Log($"Planted a Flag. Remaining Flags = {remainingFlares}/{refRemainingFlares}");

        var flagPrefab = Instantiate(MyNetwork.getInstance().findSpawnPrefabByName("Flag"));
        flagPrefab.transform.position = pos;
        NetworkServer.Spawn(flagPrefab);
    }

    [ClientCallback]
    private void OnFlareCdChanged(float oldCd, float newCd)
    {
        if (isLocalPlayer)
        {
            UIBtnFlare.GetInstance().SetIsInteractable(newCd > 0 ? false : true);
        }
    }
}
