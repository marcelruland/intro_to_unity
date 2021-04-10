using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessController : MonoBehaviour
{
    private void Start()
    {
        GameEvents.current.onEntranceTriggerEnter += OnEntranceOpen;
    }

    private void OnEntranceOpen()
    {
        Debug.Log("at entrance");
    }
}
