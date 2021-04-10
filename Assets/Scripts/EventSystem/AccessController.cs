using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessController : MonoBehaviour
{
    private void Start()
    {
        GameEvents.current.onEntranceTriggerEnter += OnEntranceOpen;
        GameEvents.current.onExitTriggerEnter += OnExitOpen;
    }

    private void OnEntranceOpen()
    {
        Debug.Log("at entrance");
    }

    private void OnExitOpen()
    {
        Debug.Log("at exit");
    }
}
