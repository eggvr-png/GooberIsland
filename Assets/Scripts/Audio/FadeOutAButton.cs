using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAButton : MonoBehaviour
{
    public AudioSource audioSource;

    public void FadeOutButton(){
        StartCoroutine(AudioFadeout.FadeOut(audioSource, 0.5f));
    }

    public void FadeInButton(){
        StartCoroutine(AudioFadeout.FadeIn(audioSource, 0.5f));
    }
}
