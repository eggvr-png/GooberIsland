using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    [Header("Music that can play :D")]
    public AudioClip sunset;
    public AudioClip chow;
    public AudioClip p2Radio;
    [Space]
    public AudioSource audioSourse;

    public void Choose()
    {
        int chance = Random.Range(1, 8);

        if (chance == 1 || chance == 2 || chance == 3 || chance == 4 || chance == 5){
            audioSourse.clip = sunset;
            audioSourse.Play();
        }
        if (chance == 6){
            audioSourse.clip = chow;
            audioSourse.Play();
        }
        if (chance == 7){
            audioSourse.clip = p2Radio;
            audioSourse.Play();
        }
    }

    void Start(){
        Choose();
    }

    void Update(){
        if (!audioSourse.isPlaying){
            Choose();
        }
    }
}
