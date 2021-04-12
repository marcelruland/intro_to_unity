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
    public event Action onChambersTriggerEnter;
    public event Action onChambersTriggerExit;

    public void EntranceTriggerEnter()
    {
        // roughly equivalent to if (bla != null)
        onEntranceTriggerEnter?.Invoke();
    }

    public void ExitTriggerEnter()
    {
        onExitTriggerEnter?.Invoke();
    }
    
    public void ChambersTriggerEnter()
    {
        // roughly equivalent to if (bla != null)
        onChambersTriggerEnter?.Invoke();
    }

    public void ChambersTriggerExit()
    {
        onChambersTriggerExit?.Invoke();
    }
    
}
