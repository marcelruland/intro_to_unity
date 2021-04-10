using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8)
            return;
        GameEvents.current.EntranceTriggerEnter();
    }
}
