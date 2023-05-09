using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static void PlaySound(AudioSource audio)
    {
        audio.volume = MenuManager.soundBarValue;
        audio.Play();
    }
}
