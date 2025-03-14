using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOverToMain : MonoBehaviour
{
    public AudioSource og;
    public AudioSource newSource;

    bool startedMusic;
    bool switched;


    public void Update()
    {
        if (!startedMusic && og.isPlaying){
            startedMusic = true;
        }

        if (startedMusic && !switched){
            if (!og.isPlaying){
                switched = true;
                newSource.Play();
            }
        }
    }
}
