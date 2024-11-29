using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Room Settings")]
    [SerializeField]private string code;
    [SerializeField]private string name;
    public enum connectionStatus{
        NotConnected,
        Connecting,
        ConnectedToServers,
        Joining,
        InLobby
    }
    [Space]
    public connectionStatus status;
    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Transform spawn;
    [Space]
    public GameObject player;
    [Space]
    public GameObject cameraHolder;
    public GameObject camera;
    [Header("Code & Name")]
    public CodeHolder codeAndNameHolder;
    [Header("Connecting Screen")]
    public GameObject connectingCamera;
    public GameObject connectingCanvas;
    [Header("Other")]
    public TextMeshProUGUI inText;
    public GameObject inUI;

    public InteractionSystem inSys;
    public CheckForFirstPlay cffp;

    void Start(){
        Connect();
        status = connectionStatus.Connecting;
        codeAndNameHolder.GetCodeAndName();
        code = codeAndNameHolder.code;
        name = codeAndNameHolder.name;
    }

    public void Connect(){
        Debug.Log("Connecting!");
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected!");
        status = connectionStatus.ConnectedToServers;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom(code, null, null);
        status = connectionStatus.Joining;
        Debug.Log("Joining Room: " + code);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        status = connectionStatus.InLobby;
        Debug.Log("Joined Lobby");
        Destroy(connectingCamera);
        Destroy(connectingCanvas);
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawn.position, Quaternion.identity);
        PlayerSetup ps = player.GetComponent<PlayerSetup>();
        cameraHolder.GetComponent<MoveCamera>().player = player.transform.GetChild(2);
        cameraHolder.SetActive(true);
        cameraHolder.GetComponent<MoveCamera>().enabled = true;
        player.GetComponent<PlayerMovement>().playerCam = camera.transform;
        player.GetComponent<PlayerMovement>().enabled = true;
        ps.GetComponent<PlayerSetup>().IsLocalPlayer();
        cffp.Check();
        ps.GetComponent<PlayerSetup>().setNameForAll();
    }
}
