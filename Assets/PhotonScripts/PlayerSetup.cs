using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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

    void Start(){
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (photonView.IsMine)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            if (!photonView.Owner.CustomProperties.ContainsKey("color"))
            {
                props = photonView.Owner.CustomProperties;
                props["color"] = PlayerPrefs.GetInt("color",1);
                photonView.Owner.SetCustomProperties(props);
            }
            
        }
        UpdateColor();

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateColor();
    }

    void UpdateColor(){
        if (photonView.Owner.CustomProperties.ContainsKey("color"))
        {
            int savedColor = (int)photonView.Owner.CustomProperties["color"];
            if (savedColor == 0 || savedColor == 1){
                foreach (Renderer r in renderers){
                    r.material = green;
                }
            }
            else if (savedColor == 2){
                foreach (Renderer r in renderers){
                    r.material = blue;
                }
            }
            else if (savedColor == 3){
                foreach (Renderer r in renderers){
                    r.material = pink;
                }
            }
        }
    }
}
