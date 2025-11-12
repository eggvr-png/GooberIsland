using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // private stuff
    string playerPrefabName = "Player";

    // the start of the actual code. like functions and stuff.
    void Start()
    {
        if (InterSceneDataKeeper.Instance == null)
        {
            Debug.Log("no isdk. creating one for development.");
            GameObject devIsdk = new GameObject("DevISDK");
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
        // do i need to comment this?
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
            // joins a random room for the player i think. just a guess. more of a hypothesis
            PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
            Debug.Log("Joining a random room! :0");
        }
        else if (roomCode != null || roomCode != "")
        {
            // the same thing but... code!1!1!!
            PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name + roomCode, null, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
            Debug.Log("Joining private room. Code: " + roomCode + " :0");
        }

        connectionStatus = connection.JoiningRoom;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionStatus = connection.InGame;
        // spawns player prefab in :0
        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefabName, transform.position, Quaternion.identity);
        // if the movement was enabled it would make the game be buggy, so we need to enable it right here instead.
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
        // real quick we put the player movement in the pause script
        pauseMenu.playerMovement = playerMovement;
        // then we enable movement
        playerMovement.enabled = true;
        playerMovement.playerCam = GameObject.Find("CameraHolder").transform;
        // disable rendering the parts so they dont get in the way of the camera
        playerObject.GetComponent<PlayerSetup>().isLocal();
        // username stuff
        playerObject.GetComponent<PhotonView>().RPC("sendUsername", RpcTarget.AllBuffered);
        // camera stuff lol. again if we just kept these enabled, the game would die.
        GameObject.FindGameObjectWithTag("PreviewCamera").SetActive(false);
        GameObject.Find("CameraHolder").transform.GetChild(0).gameObject.SetActive(true); // WHY ARE YOU LIKE THIS
        GameObject.Find("CameraHolder").GetComponent<MoveCamera>().enabled = true;
        // really long camera thing
        GameObject.Find("CameraHolder").GetComponent<MoveCamera>().player = playerObject.transform.GetChild(0).transform;
        // destory the loading screen cuz fuck the loading screen
        Destroy(loadingScreen);
        // since we dont want the pause menu enabling while loading, a var controls ability to pause
        pauseMenu.isPlayerConnected = true;
    }
}

