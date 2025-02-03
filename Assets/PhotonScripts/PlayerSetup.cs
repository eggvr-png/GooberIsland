using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject modelToDisable;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    public InteractionSystem INs;
    public NamingSystem ns;
    public Color green;
    public Color blue;
    public Color pink;
    public Material color;
    public void IsLocalPlayer(){
        modelToDisable.SetActive(false);
    }

    public void setNameForAll(){
        ns.GetComponentInParent<PhotonView>().RPC("setName", RpcTarget.AllBuffered, PlayerPrefs.GetString("name"));
    }
}
