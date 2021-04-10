using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.EntranceTriggerEnter();
    }
}
