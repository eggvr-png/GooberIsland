using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public enum connection
    {
        Offline,
        Connecting,
        ConnectedAsRoomViewer,
        JoiningRoom,
        InGame
    }
    [Header("Connection Status")]
    public connection connectionStatus;
    [Space]
    [Header("Room Settings")]
    public string displayName;
    public string roomCode;
    [Space]
    [Header("UI Elements")]
    public GameObject loadingScreen;
    public Pause pauseMenu;
    public TutorialPrompt prompt;

    [Space]
    [Header("Raft-Specific")]
    public bool isRaft;
    [Space]
    public TextMeshProUGUI roomCodeText;
    [Space]
    public GameObject hostUi;
    public HostStart hostStart;
    [Space]
    [Header("Player Refrences")]
    public GameObject playerPub;

    string playerPrefabName = "Player";
    GameObject devIsdk;

    void Start()
    {
        int evilRaftCheck = Random.Range(0, 101);
        if (evilRaftCheck == 100)
        {
            SceneManager.LoadScene("EvilRaft");
        }
        if (InterSceneDataKeeper.Instance == null)
        {
            Debug.Log("no isdk. creating one for development.");
            devIsdk = new GameObject("DevISDK");
            InterSceneDataKeeper isdk = devIsdk.AddComponent<InterSceneDataKeeper>();
            isdk.playerName = "dev";
            isdk.roomCode = "dev";
        }
        connectToServers();
        roomCode = InterSceneDataKeeper.Instance.roomCode;
        displayName = InterSceneDataKeeper.Instance.playerName;
    }

    public void connectToServers()
    {
        Debug.Log("Connecting to servers. :)");
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "us";
        PhotonNetwork.ConnectUsingSettings();
        connectionStatus = connection.Connecting;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to servers succesfully! :D");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if (roomCode == null || roomCode == "")
        {
            roomCode = codeGenerator();
            Hashtable customSettings = new Hashtable();
            customSettings["canWearCosmetics"] = InterSceneDataKeeper.Instance.canWearCosmetics;
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = InterSceneDataKeeper.Instance.maxplayers;
            options.CustomRoomProperties = customSettings;
            options.CustomRoomPropertiesForLobby = new string[]{ "canWearCosmetics" };
            PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name + roomCode, options, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
            Debug.Log("Joining private room. Code: " + roomCode + " :0");
        }
        else if (roomCode != null && roomCode != "")
        {
            PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name + roomCode, null, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
            Debug.Log("Joining private room. Code: " + roomCode + " :0");
        }
        connectionStatus = connection.JoiningRoom;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (devIsdk != null)
        {
            Destroy(devIsdk);
            devIsdk = null;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionStatus = connection.InGame;

        if (isRaft)
            roomCodeText.text = roomCode;

        if (SceneManager.GetActiveScene().name == "Island")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int seed = UnityEngine.Random.Range(1, 10000000);
                ChunkManager.instance.GetComponent<PhotonView>().RPC("StartGeneratingRPC", RpcTarget.AllBuffered, seed);
            }
            return;
        }

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefabName, transform.position, Quaternion.identity);
        playerPub = playerObject;
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
        pauseMenu.playerMovement = playerMovement;
        playerMovement.enabled = true;
        playerMovement.playerCam = GameObject.Find("CameraHolder").transform;
        playerObject.GetComponent<PlayerSetup>().isLocal();
        playerObject.GetComponent<PhotonView>().RPC("sendUsername", RpcTarget.AllBuffered);
        playerObject.GetComponent<PhotonView>().RPC("setColor", RpcTarget.AllBuffered, PlayerPrefs.GetInt("clr"));
        playerObject.GetComponent<PhotonView>().RPC("setHat", RpcTarget.AllBuffered, PlayerPrefs.GetInt("hat"));
        if (InterSceneDataKeeper.Instance.iskofi)
            playerObject.GetComponent<PhotonView>().RPC("kofiTag", RpcTarget.AllBuffered);
        GameObject.FindGameObjectWithTag("PreviewCamera").SetActive(false);
        GameObject.Find("CameraHolder").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("CameraHolder").GetComponent<MoveCamera>().enabled = true;
        GameObject.Find("CameraHolder").GetComponent<MoveCamera>().player = playerObject.transform.GetChild(0).transform;
        Destroy(loadingScreen);
        pauseMenu.isPlayerConnected = true;
        if (isRaft)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient) {
                hostUi.SetActive(true);
                int hostFirstTime = PlayerPrefs.GetInt("hostFirst");
                if (hostFirstTime == 0)
                {
                    PlayerPrefs.SetInt("hostFirst", 1);
                    prompt.showPrompt("Host", "As a host, you can change the room settings and start the game!");
                }
            }
            
            hostStart.enabled = true;
        }
    }

    string codeGenerator()
    {
        string code = "";
        int length = 6;
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        for (int i = 0; i < length; i++)
        {
            int index = UnityEngine.Random.Range(0, chars.Length);
            code += chars[index];
        }
        return code;
    }
}