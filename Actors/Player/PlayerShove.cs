using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;

public class PlayerShove : PlayerNetworkBehaviour
{
    private static PlayerShove instance;
    public static PlayerShove GetInstance() => instance;

    [SyncVar]
    private float refShoveCd;

    [SyncVar(hook = nameof(OnShoveCdChanged))]
    private float shoveCd;


    private PlayerTileRenderer playerTileRenderer;

    public override void OnStartServer()
    {
        base.OnStartServer();

        shoveCd = refShoveCd = LobbySettings.GetInstance().shoveCd;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        instance = this;

        playerTileRenderer = GetComponent<PlayerTileRenderer>();

        if(playerTileRenderer == null)
            ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(playerTileRenderer));

    }

    private void Update()
    {
        if(isServer)
        {
            if (shoveCd > 0)
            {
                shoveCd -= Time.deltaTime;
            }
        }

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Shove();
        }
    }

    [ClientCallback]
    public void Shove()
    {
        if (IsMoving)
            return;

        CmdShove(transform.position);
    }

    [Command]
    private void CmdShove(Vector2 pos)
    {
        if (shoveCd > 0 || isOnHazard || isAvalanched || health <= 5)
            return;

        shoveCd = refShoveCd;

        HealthEvents.GetInstance().eventAddHealth?.Invoke(this, new EventAddHealth(connectionToClient, -5));

        var adjacentTilesPos = GridManager.GetInstance().GetAdjacentTiles(pos).Select(t => new Vector2(t.x, t.y)).ToArray();

        TargetOnShove(adjacentTilesPos);
    }

    [TargetRpc]
    private void TargetOnShove(params Vector2[] adjacentTilesPos) => GridRenderer.GetInstance().ShowTiles(true, adjacentTilesPos);

    [ServerCallback]
    protected override void OnHealthChangedCallback(object sender, EventHealthChanged e) => TargetToggleShoveButton();


    [ServerCallback]
    protected override void OnAvalancheCallback(object sender, EventAvalanche e) => TargetToggleShoveButton();

    private void OnShoveCdChanged(float o, float v)
    {
        if (isLocalPlayer)
            UIBtnShove.GetInstance().SetCd(shoveCd, refShoveCd);
    }

    [TargetRpc]
    private void TargetToggleShoveButton()
    {
        if (isOnHazard || isAvalanched || health <= 5 || shoveCd > 0)
            UIBtnShove.GetInstance().SetIsInteractable(false);
        else
            UIBtnShove.GetInstance().SetIsInteractable(true);


    }
}
