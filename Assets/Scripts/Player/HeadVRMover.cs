using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HeadVRMover : MonoBehaviour
{
    public Transform headTrans;
    public Transform vrHeadPosTrans;

    public void Awake()
    {
        if (XRSettings.isDeviceActive)
        {
            headTrans.position = vrHeadPosTrans.position;
        }
    }
}
