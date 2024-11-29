using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class NamingSystem : MonoBehaviourPunCallbacks
{
    [Header("Name")]
    public string playerName;
    [Header("Refrences")]
    public TextMeshPro nameText;
    public string namePP;

    void Start(){
        playerName = PlayerPrefs.GetString(namePP);
    }

    [PunRPC]
    public void setName(string nname){
        nameText.text = nname;
    }
}
