using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInteraction : MonoBehaviourPunCallbacks
{
    // this is the text that shows when you look at the object
    [Header("Interaction Text")]
    public string interaction1; // this for example would be: "grab"
    [Header("Settings")]
    public float debounceTime = 0.1f;
    [Space]
    public bool canBeInteracted = true;
    [Space]
    public bool notSynced;
    [Header("Refrences")]
    public PhotonView pv;
    public UnityEvent objEvent;

    [PunRPC]
    public void interact()
    {
        if (!notSynced)
        {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        objEvent.Invoke();
        canBeInteracted = false;
        StartCoroutine(debounce());
    }

    IEnumerator debounce()
    {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }
}
