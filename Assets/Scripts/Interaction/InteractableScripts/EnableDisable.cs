using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

// also can be used for other audio related interactions

public class EnableDisable : MonoBehaviourPunCallbacks, IInteractable
{
    // this is the text that shows when you look at the object
    public string interactionText => interacting ? "Turn On" : "Turn Off"; // this for example would be: "grab"
    [Header("Settings")]
    public float debounceTime = 0.1f;
    [Space]
    public bool canBeInteracted = true;
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.
    [Header("Refrences")]
    public GameObject[] objToEnable;
    public GameObject[] objToDisable;

    [PunRPC]
    public void interact() {
        if (!interacting) {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            foreach (GameObject obj in objToDisable)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in objToEnable)
            {
                obj.SetActive(true);
            }

            interacting = true;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            foreach (GameObject obj in objToDisable)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in objToEnable)
            {
                obj.SetActive(false);
            }

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
