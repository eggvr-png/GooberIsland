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
    
    // is this a good way to do the color system. no. do i care? no.
    public Material green;
    public Material blue;
    public Material pink;

    public Renderer[] renderers;

    public void IsLocalPlayer(){
        modelToDisable.SetActive(false);
    }

    public void setNameForAll(){
        ns.GetComponentInParent<PhotonView>().RPC("setName", RpcTarget.AllBuffered, InterSceneDataKeeper.Instance.playerName);
    }

    [PunRPC]
    public void changePlayerColor(){
        foreach (Renderer renderer in renderers){
            // still gonna use player prefs here since the color should save per instance.
            int savedColor = PlayerPrefs.GetInt("color");
            if (savedColor == 0 || savedColor == 1){
                renderer.material = green;
            }
            else if (savedColor == 2){
                renderer.material = blue;
            }
            else if (savedColor == 3){
                renderer.material = pink;
            }
        }
    }
}
