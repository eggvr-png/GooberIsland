using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR.NativeTypes;

public class OpenKeyboard : MonoBehaviour
{
    public void OpenVRKeyboard(){
        if (XRSettings.isDeviceActive){
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
        }
    }
}
