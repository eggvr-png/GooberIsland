using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;

public class VRCamEnabler : MonoBehaviour
{
    public TrackedPoseDriver mover;

    public void Awake()
    {
        if (XRSettings.isDeviceActive)
        {
            mover.enabled = true;
        }
    }
}
