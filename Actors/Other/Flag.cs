using Mirror;
using System.Reflection;
using UnityEngine;

public class Flag : Interactable
{
    public NetworkConnection owner;

    [ServerCallback]
    protected override void onCustomTriggerEnterCallback(NetworkConnection playerConn, int triggerRingId)
    {
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Trigger enter {transform.name} ringId={triggerRingId}");

        FlagEvents.GetInstance().eventEnteredFlag?.Invoke(this, new EventEnteredFlag(owner, this, triggerRingId));

        TargetShow(playerConn, true); // regardless of ring, show.
    }

    [ServerCallback]
    protected override void onCustomTriggerExitCallback(NetworkConnection playerConn, int triggerRingId)
    {
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Trigger exit {transform.name} ringId={triggerRingId}");

        FlagEvents.GetInstance().eventExitedFlag?.Invoke(this, new EventExitedFlag(owner, this, triggerRingId));

        if (triggerRingId == 1)
            TargetShow(playerConn, false);
    }

    public NetworkConnection GetOwner() => owner;
    public void SetOwner(NetworkConnection owner) => this.owner = owner;
}
