using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void interact()
    {
        if (grabbed)
        {
            grabbed = false;
            Release();
        }
        else
        {
            grabbed = true;
            Grab();
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

    private void Grab()
    {
        rb.isKinematic = true;

    }

    private void Release()
    {
        rb.isKinematic = false;
    }
}