using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// <para>Create a child game object</para>
/// <para>Attach a Collider and this script to it</para>
/// <para>Set the triggerRingId via inspector</para>
/// <para>This transform's root will be notified of the collision object via ITriggerable (must be implmemented) by root gameObject</para>
/// </summary>
public class CustomTriggerMB : MonoBehaviour
{
    [SerializeField] private int triggerRingId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var gameObject = collision.transform.root.gameObject;
        var customTrigger = transform.GetComponentInParent<ITriggerable>();
        customTrigger?.onCustomTriggerEnter(gameObject, triggerRingId);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var gameObject = collision.transform.root.gameObject;
        var customTrigger = transform.GetComponentInParent<ITriggerable>();
        customTrigger?.onCustomTriggerExit(gameObject, triggerRingId);
    }
}
