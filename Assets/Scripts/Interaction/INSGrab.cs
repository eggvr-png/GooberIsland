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
    
    public float holdDistance = 3f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    Vector3 originalGrabPos;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalGrabPos = grabPoint.localPosition;
    }

    public void interact()
    {
        if (grabbed)
        {
            grabbed = false;
            PlayerMovement.allowMouseMovement = true;
            grabPoint.localPosition = originalGrabPos;
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
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                holdDistance -= scroll * 2f;
                holdDistance = Mathf.Clamp(holdDistance, minDistance, maxDistance);
            }
            grabPoint.position = cameras.transform.position + cameras.transform.forward * holdDistance;
            transform.position = Vector3.Lerp(transform.position, grabPoint.position, Time.deltaTime * 10f);
            rb.isKinematic = true;
            if (Input.GetKey(KeyCode.R))
            {
                PlayerMovement.allowMouseMovement = false;
                float rotX = Input.GetAxis("Mouse X") * 5f;
                float rotY = Input.GetAxis("Mouse Y") * 5f;
                transform.Rotate(cameras.transform.up, -rotX, Space.World);
                transform.Rotate(cameras.transform.right, rotY, Space.World);
            }else{
                PlayerMovement.allowMouseMovement = true;
            }
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