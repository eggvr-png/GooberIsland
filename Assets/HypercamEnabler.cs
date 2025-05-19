using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypercamEnabler : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public GameObject uiObj;
    public bool ui;
    public bool player;

    private void Start()
    {
        if (PlayerPrefs.GetInt("hypercam") == 1)
        {
            if (ui)
            {
                uiObj.SetActive(true);
            }
            else if (player)
            {
                cam.targetTexture = rt;
            }
        }
    }
}
