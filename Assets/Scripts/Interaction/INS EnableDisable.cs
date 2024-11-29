using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class INSEnableDisable : MonoBehaviourPunCallbacks
{
    [Header("Refrences")]
    public GameObject objToChange;
    public bool objActive;
    [Header("Settings")]
    public string enablePrompt;
    public string disablePrompt;

    [PunRPC]
    public void interact(){
        Debug.Log("Interaction: " + this.gameObject.name);
        if (objActive){
            objActive = false;
            objToChange.SetActive(false);
        }
        else{
            objActive = true;
            objToChange.SetActive(true);
        }
    }
}
