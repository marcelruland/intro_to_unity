using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BackgroundMusic;
    public AudioClip musicMenu;
    public AudioClip musicLobby;
    public AudioClip musicRound;

    public void ChangeBackgroundMusic(AudioClip newBackgroundMusic)
    {
        BackgroundMusic.Stop();
        BackgroundMusic.clip = newBackgroundMusic;
        BackgroundMusic.Play();
    }
}
