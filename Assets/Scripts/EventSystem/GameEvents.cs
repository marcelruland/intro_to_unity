using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onEntranceTriggerEnter;
    public event Action onExitTriggerEnter;

    public void EntranceTriggerEnter()
    {
        // roughly equivalent to if (bla != null)
        onEntranceTriggerEnter?.Invoke();
    }

    public void ExitTriggerEnter()
    {
        onExitTriggerEnter?.Invoke();
    }
}
