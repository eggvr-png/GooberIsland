using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRCamSwitcher : MonoBehaviour
{
    public RoomManager rm;

    public GameObject xrOrigin;
    public GameObject mainCam;

    public void Start(){
       if (XRSettings.isDeviceActive){
         rm.cameraHolder = xrOrigin;
        rm.mainCamera = mainCam;
        xrOrigin.SetActive(true);
        this.gameObject.SetActive(false);
       }
    }
}
