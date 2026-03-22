using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.SlotRacer.Utils;
using UnityEngine;

public class InvisDisappearAfterLine : MonoBehaviour
{
    public AudioSource voicelinesEmitter;
    public AudioClip lineToDisable;

    void Update()
    {
        if (voicelinesEmitter.isPlaying == false && voicelinesEmitter.clip == lineToDisable)
        {
            this.gameObject.SetActive(false);
        }
    }
}
