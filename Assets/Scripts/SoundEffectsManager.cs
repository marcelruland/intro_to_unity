using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundEffectsManager : MonoBehaviour


{
    // Eine andere Möglichkeit den Manager zu machen
    /*// public static SoundEffectsManager instance;
    //
    // public AudioClip[] s_Soundeffectlist;
    //
    // private List<AudioSource> s_source = new List<AudioSource>();
    //
    // void Awake()
    // {
    //     instance = this;
    // }
    //
    // void Start()
    // {
    //     for (int i = 0; i < s_Soundeffectlist.Length; i++)
    //     {
    //        s_source.Add(new AudioSource());
    //        s_source[i] = gameObject.AddComponent<AudioSource>();
    //        s_source[i].clip = s_Soundeffectlist[i];
    //     }
    // }
    //
    // public void s_playsound(int s)
    // {
    //     s_source[s].Play();
    // }*/
    
    
    public AudioSource SoundEffects;
    public AudioClip sfxDrinkMilk;
    public AudioClip sfxRecharge;
    public AudioClip sfxTakeDamage;
    public AudioClip sfxDrinkDisinfectant;
    public AudioClip sfxButtonClick;
    public void PlaySoundEffect(AudioClip soundEffect)
    {
        SoundEffects.Stop();
        SoundEffects.clip = soundEffect;
        SoundEffects.Play();
    }
}
