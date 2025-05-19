using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Fragsurf.Movement;
using UnityEngine.UI;
using GooberInteraction;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject[] modelToDisable;

    public GameObject interactionUI;
    public InteractionSystem interactionSystem;
    public TextMeshProUGUI interactionText;
    public NamingSystem ns;
    
    // is this a good way to do the color system. no. do i care? no.
    public Material green;
    public Material blue;
    public Material pink;
    public Material yellow;

    public RawImage keyIcon;

    public Renderer[] renderers;

    public Transform left;
    public Transform right;

    public Animator animator;

    public SurfCharacter playerMovement;
    public GameObject playercameraholder;
    public Camera playercamera;
    public AudioListener al;

    public void IsLocalPlayer(){
        foreach (GameObject parts in modelToDisable){
            parts.SetActive(false);
        }
        playercameraholder.SetActive(true);
        playerMovement.enabled = true;
    }

    public void setNameForAll(){
        ns.GetComponentInParent<PhotonView>().RPC("setName", RpcTarget.AllBuffered, InterSceneDataKeeper.Instance.playerName);
    }

    void Start(){
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

        interactionSystem.keyIcon = keyIcon;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateColor();
    }

    void Update(){
        
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
            else if (savedColor == 4){
                foreach (Renderer r in renderers){
                    r.material = yellow;
                }
            }
        }
    }

    public void getInteractionUI(GameObject iUi, TextMeshProUGUI iTxt){
        interactionUI = iUi;
        interactionText = iTxt;
    }
}
