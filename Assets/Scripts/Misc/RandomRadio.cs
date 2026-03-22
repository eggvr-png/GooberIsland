using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRadio : MonoBehaviour
{
    [Header("Music that can play :D")]
    public AudioClip sunset;
    public AudioClip chow;
    public AudioClip p2Radio;
    public AudioClip complment;
    public AudioClip story;
    [Space]
    public AudioSource audioSourse;

    public void Choose()
    {
        int chance = Random.Range(1, 10);

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
        if (chance == 8)
        {
            audioSourse.clip = complment;
            audioSourse.Play();
        }
        if (chance == 9)
        {
            audioSourse.clip = story;
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