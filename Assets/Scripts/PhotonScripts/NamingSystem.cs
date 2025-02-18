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

    void Start(){
        playerName = InterSceneDataKeeper.Instance.name;
    }

    [PunRPC]
    public void setName(string nname){
        nameText.text = nname;
    }
}
