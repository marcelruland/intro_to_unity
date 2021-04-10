using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8)
            return;
        GameEvents.current.ExitTriggerEnter();
    }
}
