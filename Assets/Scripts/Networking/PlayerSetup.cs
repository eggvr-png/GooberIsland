using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Renderer[] bodyPartRenderers;
    public Renderer[] limbRenderers;
    [Space]
    public GameObject[] hats;
    [Space]
    public Material blue;
    public Material pink;

    public void isLocal()
    {
        foreach (Renderer r in bodyPartRenderers)
        {
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    [PunRPC]
    public void setColor(int colorVal)
    {
        if (colorVal == 0)
        {
            return;
        }
        else if (colorVal == 1)
        {
            foreach (Renderer r in limbRenderers)
            {
                r.material = blue;
            }
        }
        else if (colorVal == 2)
        {
            foreach (Renderer r in limbRenderers)
            {
                r.material = pink;
            }
        }
    }
    
    [PunRPC]
    public void setHat(int hatId)
    {
        if (hatId == 0)
        {
            return;
        }
        else
        {
            hats[hatId - 1].SetActive(true);
        }
    }
}
