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
    public GameObject hatsholer;
    [Space]
    public Material blue;
    public Material pink;
    public Material kofi;

    public void isLocal()
    {
        foreach (Renderer r in bodyPartRenderers)
        {
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        hatsholer.SetActive(false);
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
        else if (colorVal == 3)
        {
            foreach (Renderer r in limbRenderers)
            {
                r.material = kofi;
            }
        }
    }
    
    [PunRPC]
    public void setHat(int hatId)
    {
        bool canWearCosmetics = false;

        Debug.Log(hatId.ToString());

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("canWearCosmetics"))
        {
            canWearCosmetics = (bool)PhotonNetwork.CurrentRoom.CustomProperties["canWearCosmetics"];
            Debug.Log(canWearCosmetics.ToString());
            if (canWearCosmetics){
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
        else
        {
            Debug.Log("Custom Prop 4 Cosmetics not found");
        }
    }
}
