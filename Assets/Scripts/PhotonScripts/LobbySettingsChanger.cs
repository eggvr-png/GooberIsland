using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class LobbySettingsChanger : MonoBehaviour
{
    public RoomManager rm;
    public LobbySettings ls;
    [Header("UI Refrences")]
    public GameObject menu1;
    public GameObject menu2;
    [Space]
    public TMP_InputField playerMaxField;
    [Header("Settings")]
    public KeyCode menuKey;

    bool menuOpen;
    bool menuDebounce;

    // adds listensers so theres not 100 things in the update func
    void Start(){
        playerMaxField.onSubmit.AddListener(delegate {checkIfMaxPlayerInvalid(playerMaxField.text);});
    }
    
    void Update(){
        OpenCloseMenu();
    }
    // locks mouse
    public void LockMouse(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // unlocks mouse
    public void UnlockMouse(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // open menu code
    private void OpenCloseMenu(){
        if (Input.GetKeyDown(menuKey)){
            if (!menuDebounce) {
                if (!menuOpen){
                    if (PhotonNetwork.IsMasterClient){
                        UnlockMouse();
                        rm.player.GetComponent<PlayerMovement>().enabled = false;
                        menu1.SetActive(false);
                        menu2.SetActive(true);
                        menuOpen = true;
                        menuDebounce = true;
                        StartCoroutine(debounceMenu());
                    }
                }
                else {
                    if (PhotonNetwork.IsMasterClient){
                        LockMouse();
                        rm.player.GetComponent<PlayerMovement>().enabled = true;
                        menu1.SetActive(true);
                        menu2.SetActive(false);
                        menuOpen = false;
                        menuDebounce = true;
                        StartCoroutine(debounceMenu());
                    }
                }
            }
        } 
    }

    // debounce for menu opener
    private IEnumerator debounceMenu(){
        yield return new WaitForSeconds(0.1f);
        menuDebounce = false;
    }

    // max player code
    private void checkIfMaxPlayerInvalid(string input){
        int result;
        bool parseSuccess = int.TryParse(input, out result);
        if (parseSuccess){
            if (result < PhotonNetwork.CountOfPlayers){
                playerMaxField.text = PhotonNetwork.CountOfPlayers.ToString();
                PhotonNetwork.RemoveBufferedRPCs(0,"changeMaxPlayer");
                this.GetComponentInParent<PhotonView>().RPC("changeMaxPlayer", RpcTarget.AllBuffered, PhotonNetwork.CountOfPlayers);
            }
            else if (result > 6){
                playerMaxField.text = "6";
                PhotonNetwork.RemoveBufferedRPCs(0,"changeMaxPlayer");
                this.GetComponentInParent<PhotonView>().RPC("changeMaxPlayer", RpcTarget.AllBuffered, 6);
            }
            else {
                PhotonNetwork.RemoveBufferedRPCs(0,"changeMaxPlayer");
                this.GetComponentInParent<PhotonView>().RPC("changeMaxPlayer", RpcTarget.AllBuffered, result);
            }
        }
        else {
            playerMaxField.text = PhotonNetwork.CountOfPlayers.ToString();
        }
    }

    [PunRPC]
    public void changeMaxPlayer(int newMax){
        ls.maxPlayers = newMax;
    }
}
