using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
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
    public bool canBeInteracted = true; // basically the debounce varaible.
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.
    public bool otherPlayerHolding;

    Rigidbody rb;
    public Transform gp;

    bool isRotating;
    void Start()
    {
        // gets the grabbables rigidbody since it doesnt need 2 be public
        rb = this.gameObject.GetComponent<Rigidbody>();
        //   if (preset != null )
        //   {
        //     rb.mass = preset.mass;
        //     dampFactor = preset.damping;
        //     force = preset.force;
        //   }
    }

    void Update()
    {
        if (interacting)
        {
            Vector3 direction = gp.position - transform.position;
            rb.AddForce(direction * force);

            rb.velocity *= dampFactor;
            rb.angularVelocity *= dampFactor;
        }
    }

    public void getGrabPoint(Transform grabpoint)
    {
        gp = grabpoint;
    }

    public void grab()
    {
        interacting = true;
        changeGrabStatus();
        canBeInteracted = false;
        StartCoroutine(debounce());
    }

    public void stop()
    {
        //pv.TransferOwnership(PhotonNetwork.LocalPlayer);
        interacting = false;
        changeGrabStatus();
        canBeInteracted = false;
        StartCoroutine(debounce());
    }

    IEnumerator debounce()
    {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }

    void changeGrabStatus()
    {
        if (interacting)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
