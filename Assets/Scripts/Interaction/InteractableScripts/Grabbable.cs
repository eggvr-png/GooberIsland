using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Grabbable : MonoBehaviourPunCallbacks, IGrabbable
{
    public string interactionText => interacting ? "Drop" : "Grab"; // this for example would be: "grab"
    [Header("Settings")]
    public float debounceTime = 0.1f;
    [Space]
    public float dampFactor = 0.98f;
    public float force = 100;
    [Space]
    public bool canBeInteracted = true; // basically the debounce varaible.
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.

    Rigidbody rb;
    public Transform gp;
    public PhotonView pv;


    public float grabOffset; //for zoom  <-- changed from Vector3 to float

    Vector3 lastMousePos;
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

        if (gp == null)
        {
            gp = GrabbableSetter.Instance.grabPoint;
        }
    }

    void Update()
    {
        if (interacting)
        {
            Vector3 targetPos = gp.position + gp.forward * grabOffset; // automatically use direction of gp
            Vector3 direction = targetPos - transform.position;
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
        if (PhotonNetwork.IsConnected)
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
        interacting = true;
        changeGrabStatus();
        canBeInteracted = false;
        StartCoroutine(debounce());
    }
    
    public void stop()
    {
        if (PhotonNetwork.IsConnected)
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);
        interacting = false;
        changeGrabStatus();
        canBeInteracted = false;
        StartCoroutine(debounce());
        grabOffset = 0f; // reset for float
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

    [PunRPC]
    public void stash()
    {
        this.gameObject.SetActive(false);
    }

    [PunRPC]
    public void unstash(float xpos, float ypos, float zpos)
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = new Vector3(xpos,ypos,zpos);
    }

    public void Rotate(Vector3 rot)
    {
        if (interacting)
        {
            rb.AddTorque(rot * force);
        }
    }
}
