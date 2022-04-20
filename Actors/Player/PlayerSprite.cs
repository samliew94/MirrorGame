using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSprite : PlayerNetworkBehaviour
{
    private Animator animator;

    public override void OnStartServer()
    {
        base.OnStartServer();

        ShowOtherPlayers(false, 3);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        animator = GetComponentInChildren<Animator>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (animator == null)
            ThrowNullError(this, MethodBase.GetCurrentMethod().Name, nameof(animator));
    }

    [ServerCallback]
    private async void ShowOtherPlayers(bool isShow, float revealDelay = 0)
    {
        await Task.Delay(TimeSpan.FromSeconds(revealDelay));

        var everyOtherPlayer = NetworkServer.connections.Values.Except(new NetworkConnection[] { connectionToClient }).Select(x=>x.identity.netId);

        TargetShowConns(isShow, everyOtherPlayer.ToArray());
    }

    [TargetRpc]
    private void TargetShowConns(bool isShow, uint [] conns)
    {
        try
        {
            var animators = GameObject.FindObjectsOfType<PlayerSprite>()
            .Where(x => conns.Contains(x.netId))
            .Select(x => x.animator)
            ;

            foreach (var animator in animators)
                animator.SetBool("isShow", isShow);
        }
        catch (Exception e)
        {
            ThrowError(this, MethodBase.GetCurrentMethod().Name, e.Message);
        }
    }

    [ServerCallback]
    protected override void OnAvalancheCallback(object sender, EventAvalanche e)
    {
        ShowOtherPlayers(true);
    }

    [ServerCallback]
    protected override void OnAvalancheStoppedCallback(object sender, EventAvalancheStopped e)
    {
        ShowOtherPlayers(false); // players didn't get killed, resume game and hide other players
    }
}
