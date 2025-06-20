using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLPopup : MonoBehaviour
{
    public GameObject ui;

    public void Start()
    {
        RuntimePlatform currentPlatform = Application.platform;
        
        if (currentPlatform == RuntimePlatform.WebGLPlayer)
        {
            ui.SetActive(true);
        }
    }
}
