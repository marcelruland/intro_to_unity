using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public AudioSource SoundEffects;
    public AudioClip sfxDrink;

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        // SoundEffects.Stop();
        SoundEffects.clip = soundEffect;
        SoundEffects.Play();
    }
}
