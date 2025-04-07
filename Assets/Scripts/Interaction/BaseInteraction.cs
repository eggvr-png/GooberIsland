using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BaseInteraction : MonoBehaviourPunCallbacks
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

    [PunRPC]
    public void interact() {
        if (!interacting) {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            // insert code for when you first interact (example: disable the radio)
            interacting = true;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            // insert code for when you interact with it again (example: turn back on the radio)
            interacting = false;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
    }

    IEnumerator debounce() {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }
}
