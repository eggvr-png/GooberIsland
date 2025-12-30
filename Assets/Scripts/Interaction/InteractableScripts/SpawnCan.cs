using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// also can be used for other audio related interactions

public class SpawnCan : MonoBehaviourPunCallbacks, IInteractable
{
    // this is the text that shows when you look at the object
    public string interactionText => interacting ? "Dispense Can" : "Dispense Can"; // this for example would be: "grab"
    [Header("Settings")]
    public float debounceTime = 0.1f;
    [Space]
    public bool canBeInteracted = true;
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.
    [Header("Refrences")]
    public Transform spawnPoint;
    public GameObject prefabToSpawn;

    [PunRPC]
    public void interact() {
        if (!interacting) {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

            interacting = true;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

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
