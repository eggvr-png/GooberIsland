using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Room Settings")]
    [SerializeField] private string code;
    [SerializeField] private string username;
    public enum connectionStatus
    {
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
    public GameObject mainCamera;
    [Header("Connecting Screen")]
    public GameObject connectingCamera;
    public GameObject connectingCanvas;
    [Header("In Game UI")]
    public GameObject hostMenu;
    [Space]
    public TextMeshProUGUI inText;
    public GameObject inUI;
    [Space]
    public GameObject tbtHolder;
    public Image talkBox;
    public TextMeshProUGUI talktext;
    [Header("Other")]
    public InteractionSystem inSys;
    public CheckForFirstPlay cffp;
    [Space]
    public LobbySettings ls;

    // connects to servers
    void Start()
    {
        Connect();
        status = connectionStatus.Connecting;
        code = InterSceneDataKeeper.Instance.roomCode;
        username = InterSceneDataKeeper.Instance.playerName;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = Application.version;
        //bad code, needed for bad quality mode
        if (QualitySettings.GetQualityLevel() == 0)
        {
            foreach (Renderer rend in FindObjectsOfType<Renderer>())
            {
                foreach (Material mat in rend.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", mat.color);
                    }
                }
            }
        }
    }

    public void Connect()
    {
        Debug.Log("Connecting!");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
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
        if (code != null)
        {
            // hmm, i think its totally not obvious what this does!
            PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name + code, null, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
            Debug.Log("Joining Room: " + SceneManager.GetActiveScene().name + code);
        }
        else
        {
            PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
            Debug.Log("Joining Random Room!");
        }

        status = connectionStatus.Joining;

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        status = connectionStatus.InLobby;
        Debug.Log("Joined Lobby: " + PhotonNetwork.CurrentRoom.Name);
        // spawns in player
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawn.position, Quaternion.identity);
        PlayerSetup ps = player.GetComponent<PlayerSetup>();
        // enables movement and other stuff
        cameraHolder.GetComponent<MoveCamera>().player = player.transform.GetChild(2);
        cameraHolder.SetActive(true);
        cameraHolder.GetComponent<MoveCamera>().enabled = true;
        player.GetComponent<PlayerMovement>().playerCam = mainCamera.transform;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        ps.IsLocalPlayer();
        // checks for first play
        cffp.Check();
        ps.setNameForAll();
        // check if player is host
        if (PhotonNetwork.IsMasterClient)
        {
            hostMenu.SetActive(true);
        }
        StartCoroutine(finshJoin());
    }

    public IEnumerator finshJoin()
    {
        // adds a 0.5 second delay to joining so buffered rpcs can run!
        yield return new WaitForSeconds(0.5f);
        // here we check if the lobby is over player max
        if (PhotonNetwork.CurrentRoom.PlayerCount > ls.maxPlayers)
        {
            // disconnect player first since if we dont, then the player will still be in the lobby although they switched scenes
            PhotonNetwork.Destroy(player);
            PhotonNetwork.Disconnect();
            // set a playerpref to tell the client that HEY this person disconnected due to max players!
            PlayerPrefs.SetInt("RTMFMP", 1);
            Debug.Log("Disconnecting: Lobby Full");
            SceneManager.LoadScene(0);
        }
        // finally, we delete connecting screen
        Destroy(connectingCamera);
        Destroy(connectingCanvas);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        Debug.Log("Master Client Changed.");

        if (PhotonNetwork.IsMasterClient)
        {
            hostMenu.SetActive(true);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (status == connectionStatus.InLobby){
            status = connectionStatus.NotConnected;
            InterSceneDataKeeper.errorText = cause.ToString() +"\n something with photon";
            SceneManager.LoadScene(0);

        }
    }
}
