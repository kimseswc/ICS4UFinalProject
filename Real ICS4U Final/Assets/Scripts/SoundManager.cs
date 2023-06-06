using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGMplayer;

    public static void PlaySound(AudioSource audio, AudioClip clip)
    {
        audio.volume = MenuManager.soundBarValue;
        audio.PlayOneShot(clip);
    }

    void Start()
    {
        BGMplayer.clip = GameAssets.i.BGM;
        BGMplayer.loop = true;
        BGMplayer.Play();

    }
}
