using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Goal : Interactable
{
    [ServerCallback]
    protected override void onCustomTriggerEnterCallback(NetworkConnection conn, int triggerRingId)
    {
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Trigger enter {transform.name} ringId={triggerRingId}");
        GoalEvents.GetInstance().eventPlayerLandedOnGoal?.Invoke(this, new EventPlayerLandedOnGoal(conn, true));
    }

    [ServerCallback]
    protected override void onCustomTriggerExitCallback(NetworkConnection conn, int triggerRingId)
    {
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Trigger exit {transform.name} ringId={triggerRingId}");
        GoalEvents.GetInstance().eventPlayerLandedOnGoal?.Invoke(this, new EventPlayerLandedOnGoal(conn, false));
    }
}
