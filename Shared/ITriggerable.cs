using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ITriggerable
{
    void onCustomTriggerEnter(GameObject gameObject, int triggerRingId);
    void onCustomTriggerExit(GameObject gameObject, int triggerRingId);
}
