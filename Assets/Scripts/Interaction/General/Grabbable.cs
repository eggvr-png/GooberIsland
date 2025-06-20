using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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
    public float dampFactor = 0.98f;
    public float force = 100;
    [Space]
    public bool canBeInteracted = true;
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.
    [Header("Refrences")]
    public PhotonView pv;

    Rigidbody rb;
    Transform gp;

    bool isRotating;
    void Start()
    {
        // gets the grabbables rigidbody since it doesnt need 2 be public
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (interacting) {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            Vector3 direction = gp.position - transform.position;
            rb.AddForce(direction * force);

            if (Input.GetKey(KeyCode.Q)){ // i was gonna make rotation r but the place ment system uses r, fix later -max
                PlayerAiming.allowMouseMovement = false;
                isRotating = true;

                float xMovement = Input.GetAxisRaw("Mouse X");
		        float yMovement = Input.GetAxisRaw("Mouse Y");

                transform.Rotate(gp.up, xMovement * 5f, Space.World);
                transform.Rotate(gp.right, yMovement * 5f, Space.World);
                
            }
            else {
                if (isRotating){
                    PlayerAiming.allowMouseMovement = true;
                }

                isRotating = false;
            }

            rb.velocity *= dampFactor;
            rb.angularVelocity *= dampFactor;
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
            rb.isKinematic = false;
        }
        else {
            rb.isKinematic = false;
        }
    }
}
