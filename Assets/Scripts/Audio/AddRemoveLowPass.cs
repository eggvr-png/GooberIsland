using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveLowPass : MonoBehaviour
{
    public bool add;
    public bool remove;

    public GameObject audio;

    public void addAudioLowPass(){
        if (add){
            AudioLowPassFilter lowpass = audio.AddComponent<AudioLowPassFilter>();
            AudioReverbFilter reverb = audio.AddComponent<AudioReverbFilter>();
            reverb.reverbPreset = AudioReverbPreset.Psychotic;
            return;
        }
        if (remove){
            if (audio.GetComponent<AudioLowPassFilter>()){
                Destroy(audio.GetComponent<AudioLowPassFilter>());
                Destroy(audio.GetComponent<AudioReverbFilter>());
            }
        }
    }
}
