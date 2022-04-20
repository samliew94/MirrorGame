using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Hazard : Interactable
{
    protected override void onCustomTriggerEnterCallback(NetworkConnection conn, int triggerRingId)
    {
        ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Trigger enter {transform.name} ringId={triggerRingId}");
        HazardEvents.GetInstance().eventPlayerOnHazard?.Invoke(this, new EventPlayerOnHazard(conn));
    }
}
