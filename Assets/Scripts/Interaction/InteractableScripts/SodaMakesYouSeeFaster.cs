using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// also can be used for other audio related interactions

public class SodaMakesYouSeeFaster : MonoBehaviourPunCallbacks, IInteractable
{
    // this is the text that shows when you look at the object
    public string interactionText => interacting ? "Drink" : "Drink"; // this for example would be: "grab"
    [Header("Settings")]
    public float debounceTime = 0.1f;
    [Space]
    public bool canBeInteracted = true;
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.

    [PunRPC]
    public void interact() {
        if (!interacting) {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            Camera.main.fieldOfView = Camera.main.fieldOfView * 1.2f;
            
            interacting = true;
            canBeInteracted = false;
            Destroy(this.gameObject);
        }
        else {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            Camera.main.fieldOfView = Camera.main.fieldOfView * 1.2f;

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
