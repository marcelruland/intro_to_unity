using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Script : MonoBehaviour
{
    private SoundEffectsManager _soundEffectsManager;
    private void Start()
    {
        _soundEffectsManager = FindObjectOfType<SoundEffectsManager>();
    }

    public void play_sound(int s)
    {
        _soundEffectsManager.PlaySoundEffect(_soundEffectsManager.sfxButtonClick);
    }
}
