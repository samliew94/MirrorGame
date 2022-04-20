using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerBrace : PlayerNetworkBehaviour
{
    private static PlayerBrace instance;
    public static PlayerBrace GetInstance() => instance;

    [SyncVar]
    private float refBraceCd;

    [SyncVar(hook = nameof(OnBraceCdChanged))]
    private float braceCd;

    [SyncVar]
    private bool isBracing;

    public override void OnStartServer()
    {
        base.OnStartServer();

        braceCd = refBraceCd = LobbySettings.GetInstance().braceCd;
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
            if (braceCd > 0)
                braceCd -= Time.deltaTime;
        }

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Brace();
            }
        }
    }
    [ClientCallback]
    public void Brace()
    {
        if (IsMoving)
            return;

        CmdBrace();
    }

    [Command]
    private void CmdBrace()
    {
        if (braceCd > 0)
            return;

        braceCd = refBraceCd;

        isBracing = true;

        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Bracing");
    }

    private void OnBraceCdChanged(float oldValue, float newValue)
    {
        if (isLocalPlayer)
            UIBtnBrace.GetInstance().SetIsInteractable(newValue > 0 ? false : true);
    }

    public bool GetIsBracing() => isBracing; // can be used by both client and server
}