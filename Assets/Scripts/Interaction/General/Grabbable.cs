using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Grabbable : MonoBehaviourPunCallbacks
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

    Rigidbody rb;
    Transform gp;

    void Start()
    {
        // gets the grabbables rigidbody since it doesnt need 2 be public
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (interacting) {
            transform.position = Vector3.Lerp(transform.position, gp.position, Time.deltaTime * 10);
        }
    }

    public void getGrabPoint(Transform grabpoint) {
        gp = grabpoint;
    }

    [PunRPC]
    public void interact() {
        if (!interacting) {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            interacting = true;
            changeGrabStatus();
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            interacting = false;
            changeGrabStatus();
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
    }

    IEnumerator debounce() {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }

    void changeGrabStatus(){
        if (interacting) {
            rb.isKinematic = true;
        }
        else {
            rb.isKinematic = false;
        }
    }
}
