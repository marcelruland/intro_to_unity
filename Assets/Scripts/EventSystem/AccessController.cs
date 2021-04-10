using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessController : MonoBehaviour
{
    public Collider EntranceCollider;
    public Collider ExitCollider;
    private void Start()
    {
        GameEvents.current.onEntranceTriggerEnter += OnEntranceOpen;
        GameEvents.current.onExitTriggerEnter += OnExitOpen;
    }

    private void OnEntranceOpen()
    {
        Debug.Log("at entrance");
        ExitCollider.enabled = false;
    }

    private void OnExitOpen()
    {
        Debug.Log("at exit");
        EntranceCollider.enabled = false;
    }
}
