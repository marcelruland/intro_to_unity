using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyChambersExitTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8)
            return;
        GameEvents.current.ChambersTriggerExit();
    }
}
