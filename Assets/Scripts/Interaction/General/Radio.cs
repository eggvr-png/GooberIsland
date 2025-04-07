using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// also can be used for other audio related interactions

public class Radio : MonoBehaviourPunCallbacks
{
    // this is the text that shows when you look at the object
    [Header("Interaction Text")]
    public string interaction1; // this for example would be: "grab"
    public string interaction2; // this for example would be: "drop"
    [Header("Settings")]
    public float debounceTime = 0.1f;
    [Space]
    public bool canBeInteracted = true;
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.
    [Header("Refrences")]
    public PhotonView pv;
    public AudioSource audioSource;

    [PunRPC]
    public void interact() {
        if (!interacting) {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);

            StartCoroutine(Pitch(false));

            interacting = true;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);

            StartCoroutine(Pitch(true));

            interacting = false;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
    }

    IEnumerator debounce() {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }

    IEnumerator Pitch(bool up){
        if (!up){
            int i = 0;
            while (i != 100){
                ++i;
                audioSource.pitch = audioSource.pitch - 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else {
            int i = 0;
            while (i != 100){
                ++i;
                audioSource.pitch = audioSource.pitch + 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
