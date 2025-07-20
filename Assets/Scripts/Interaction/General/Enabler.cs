using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Enabler : MonoBehaviourPunCallbacks
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
    [Space]
    public bool objectOn;
    [Header("Refrences")]
    public PhotonView pv;
    public GameObject gameObj;

    [PunRPC]
    public void interact() {
        if (!interacting) {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            check();
            interacting = true;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            check();
            interacting = false;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
    }

    public void check()
    {
        if (objectOn)
        {
            objectOn = false;
            gameObj.SetActive(false);
        }
        else
        {
            objectOn = true;
            gameObj.SetActive(true);
        }
    }

    IEnumerator debounce() {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }
}
