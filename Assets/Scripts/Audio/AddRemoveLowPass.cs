using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveLowPass : MonoBehaviour
{
    public bool add;
    public bool remove;

    public GameObject audioObj;

    public void addAudioLowPass(){
        if (add){
            AudioLowPassFilter lowpass = audioObj.AddComponent<AudioLowPassFilter>();
            AudioReverbFilter reverb = audioObj.AddComponent<AudioReverbFilter>();
            reverb.reverbPreset = AudioReverbPreset.Psychotic;
            return;
        }
        if (remove){
            if (audioObj.GetComponent<AudioLowPassFilter>()){
                Destroy(audioObj.GetComponent<AudioLowPassFilter>());
                Destroy(audioObj.GetComponent<AudioReverbFilter>());
            }
        }
    }
}
