using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessController : MonoBehaviour
{
    public Collider entranceCollider;
    public Collider exitCollider;
    
    private AudioManager _audioManager;
    
    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        GameEvents.current.onEntranceTriggerEnter += OnEntranceStartRound;
        GameEvents.current.onExitTriggerEnter += OnExitEndRound;
        GameEvents.current.onChambersTriggerEnter += OnEntranceChambers;
        GameEvents.current.onChambersTriggerExit += OnExitChambers;
    }

    private void OnEntranceStartRound()
    {
        Debug.Log("at entrance");
        
        // lock entrance and open exit
        exitCollider.enabled = false;
        entranceCollider.enabled = true;
        GameManager.Instance.InitiateCountdown(GameManager.Instance.timePerRound);
        _audioManager.ChangeBackgroundMusic(_audioManager.musicRound);
    }

    private void OnExitEndRound()
    {
        Debug.Log("at exit");
        GameManager.Instance.SwitchState(GameManager.State.LEVELCOMPLETED);
    }

    private void OnEntranceChambers()
    {
        _audioManager.ChangeBackgroundMusic(_audioManager.musicChambers);
        GameManager.Instance.UnlockAchievement("Found the holy chambers!");
    }

    private void OnExitChambers()
    {
        _audioManager.ChangeBackgroundMusic(_audioManager.musicLobby);
    }
}
