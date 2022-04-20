using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Interactable : CustomNetworkBehaviour, ITriggerable
{
    private Animator animator;

    public override void OnStartClient()
    {
        base.OnStartClient();

        animator = GetComponent<Animator>();
    }

    protected readonly List<NetworkConnection> conns = new List<NetworkConnection>();
    protected readonly Dictionary<NetworkConnection, List<int>> conns2 = new Dictionary<NetworkConnection, List<int>>();

    [ServerCallback]
    public void onCustomTriggerEnter(GameObject gameObject, int triggerRingId)
    {
        NetworkIdentity identity = gameObject.GetComponent<NetworkIdentity>();

        if (identity == null)
        {
            ServerLog.Warn($"identity is null at method 'onCustomTriggerEnter' at {GetType().Name}");
            return;
        }

        var playerConn = identity.connectionToClient;

        if (playerConn == null)
            return;

        if (!conns2.ContainsKey(playerConn))
        {
            var list = conns2[playerConn] = new List<int>();
            list.Add(triggerRingId);
        }
        else
        {
            var list = conns2[playerConn];
            if (list.Contains(triggerRingId))
                return;

            list.Add(triggerRingId);
        }

        ServerLog.Log($"{transform.name} entntered collision with {identity.tag} at triggerRingId={triggerRingId}");
        onCustomTriggerEnterCallback(playerConn, triggerRingId);
    }

    [ServerCallback]
    protected virtual void onCustomTriggerEnterCallback(NetworkConnection playerConn, int triggerRingId) { }

    [ServerCallback]
    public void onCustomTriggerExit(GameObject gameObject, int triggerRingId)
    {
        NetworkIdentity identity = gameObject.GetComponent<NetworkIdentity>();

        if (identity == null)
            ServerLog.Warn($"identity is null at method 'onCustomTriggerExit' at {GetType().Name}");

        var playerConn = identity.connectionToClient;

        if (playerConn == null)
            return;

        if (conns2.ContainsKey(playerConn))
        {
            var list = conns2[playerConn];
            if (!list.Contains(triggerRingId))
                return;

            list.Remove(triggerRingId);

            ServerLog.Log($"{transform.name} exited collision with {identity.tag}");
            onCustomTriggerExitCallback(playerConn, triggerRingId);
        }

        //conns.Remove(playerConn);
    }

    [ServerCallback]
    protected virtual void onCustomTriggerExitCallback(NetworkConnection playerConn, int triggerRingId) { }

    [TargetRpc]
    public void TargetShow(NetworkConnection conn, bool isShow)
    {
        bool isShowState = animator.GetBool("isShow");

        if (isShow == isShowState)
            return;

        Show(isShow);
    }

    /// <summary>
    /// A client-side operation that uses the IAnimator script to play a "show" animation
    /// </summary>
    [Client]
    public void Show(bool isShow) => animator.SetBool("isShow", isShow);
}