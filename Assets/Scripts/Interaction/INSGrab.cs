using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class INSGrab : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// i took this script from the old broken files of goober island
    /// it should still work and ngl its kinda good :)
    /// </summary>
    
    public bool grabbed;
    public Transform grabPoint;
    private Rigidbody rb;
    public string grabPrompt;
    public string putdownPrompt;
    public GameObject cameras;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void interact()
    {
        if (grabbed)
        {
            grabbed = false;
            this.GetComponentInParent<PhotonView>().RPC("Release", RpcTarget.All);
        }
        else
        {
            grabbed = true;
            this.GetComponentInParent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            this.GetComponentInParent<PhotonView>().RPC("Grab", RpcTarget.All);
        }
    }

    private void Update()
    {
        if (grabbed)
        {
            transform.position = Vector3.Lerp(transform.position, grabPoint.position, Time.deltaTime * 10f);
            rb.isKinematic = true;
        }
    }
    [PunRPC]
    private void Grab()
    {
        rb.isKinematic = true;

    }
    [PunRPC]
    private void Release()
    {
        rb.isKinematic = false;
    }
    public void ThrowRelease(){
        grabbed = false;
        this.GetComponentInParent<PhotonView>().RPC("Release", RpcTarget.All);

        Vector3 camPosOrSmth = cameras.transform.forward;
        camPosOrSmth.Normalize();

        rb.AddForce(camPosOrSmth * 600, ForceMode.Force);
    }
}