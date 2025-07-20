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
using UnityEditor;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.OpenXR.NativeTypes;
using Fragsurf.Movement;

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
    public Transform leftHand;
    public Transform rightHand;
    [Header("Connecting Screen")]
    public GameObject connectingCamera;
    public GameObject connectingCanvas;
    [Header("In Game UI")]
    public PlayerReady pr;
    [Space]
    public TextMeshProUGUI inText;
    public GameObject inUI;
    public RawImage keyIcon;
    [Space]
    public GameObject tbtHolder;
    public Image talkBox;
    public TutorialPromptHandler tph;
    public TextMeshProUGUI talktext;
    public GameObject healthbars;
    [Header("Other")]
    //public InteractionSystem inSys;
    public CheckForFirstPlay cffp;
    public pause pMenu;
    public ChunkManager chunkManager;
    public CosmeticSystem cosmeticSystem;

    Animator playerAnims;

    // connects to servers
    void Start()
    {
        Connect();
        status = connectionStatus.Connecting;
        code = InterSceneDataKeeper.Instance.roomCode;
        username = InterSceneDataKeeper.Instance.playerName;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = Application.version;

        //bad code, needed for bad quality mode
        //bad code for a bad mode? makes sense!
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

    void Update(){
        if (status == connectionStatus.InLobby){
            if (XRSettings.isDeviceActive){
                var ps = player.GetComponent<PlayerSetup>();
            ps.left.position = leftHand.position;
            ps.right.position = rightHand.position;
            
            ps.left.rotation = leftHand.rotation;
            ps.right.rotation = rightHand.rotation;
            }
        }

        if (PhotonNetwork.OfflineMode)
        {
            status = connectionStatus.InLobby; //testing purposes -max
        }
    }

    public void Connect()
    {
        Debug.Log("Connecting!");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        // turns on offline mode if player isnt connected 2 wifi!1!!
        int offlineMode = PlayerPrefs.GetInt("offline");
        if (offlineMode == 1) {
            PhotonNetwork.OfflineMode = true;
            Debug.Log("offline mode enabled!");
            return;
        }

        // just for me testing on school wifi. it hates photon
        #if UNITY_EDITOR
        
        if (System.Environment.UserName.Contains("max"))
        {
            PhotonNetwork.OfflineMode = true;
            Debug.Log("Offline mode enabled for user 'max' in Unity Editor.");
            return;
        }
    #endif


        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected!");
        status = connectionStatus.ConnectedToServers;

        int offlineMode = PlayerPrefs.GetInt("offline");
        if (offlineMode == 1) {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.JoinRandomRoom();
            return;
        } 

        #if UNITY_EDITOR
        
        if (System.Environment.UserName.Contains("max"))
        {
            Debug.Log("Offline mode enabled for user 'max' in Unity Editor.");
            PhotonNetwork.JoinRandomRoom();
            return;
        }
    #endif
        PhotonNetwork.JoinLobby();
    }

    bool pub;
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        if (code != null)
        {
            // hmm, i think its totally not obvious what this does!
            PhotonNetwork.JoinOrCreateRoom(SceneManager.GetActiveScene().name + code, null, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
            Debug.Log("Joining Room: " + SceneManager.GetActiveScene().name + code);
        }
        else
        {
            pub = true;
            PhotonNetwork.JoinRandomOrCreateRoom(null, 4, MatchmakingMode.FillRoom, new TypedLobby(SceneManager.GetActiveScene().name, LobbyType.Default));
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
        ps.keyIcon = keyIcon;
        player.GetComponent<PhotonView>().RPC("setCosmetic", RpcTarget.AllBuffered, InterSceneDataKeeper.Instance.cosmeticId);
        cosmeticSystem = player.GetComponent<CosmeticSystem>();
        // sets the interaction ui and text to the playersetup so the interaction system can use it
        ps.getInteractionUI(inUI, inText);
        // enables movement and other stuff
        pMenu.GetCamAndOther(ps.playercamera, player.GetComponent<SurfCharacter>(), ps.al, ps.playercameraholder.GetComponent<PlayerAiming>());
        ps.IsLocalPlayer();
        // checks for first play
        cffp.Check();
        ps.setNameForAll();
        StartCoroutine(finshJoin());
        if (SceneManager.GetActiveScene().name == "Raft")
        {
            pr.enabled = true;
        }
        else
        {
            healthbars.SetActive(true);
        }
        if (PhotonNetwork.IsMasterClient && !pub)
            PhotonNetwork.CurrentRoom.IsVisible = false;

        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name == "Island")
        {
            chunkManager.GetComponentInParent<PhotonView>().RPC("ShareSeed", RpcTarget.AllBuffered, chunkManager.seed);
        }

        if (SceneManager.GetActiveScene().name == "Island")
        {
            chunkManager.startGenerating();
        }
    }

    public IEnumerator finshJoin()
    {
        // adds a 0.5 second delay to joining so buffered rpcs can run!
        yield return new WaitForSeconds(0.5f);
        // finally, we delete connecting screen
        Destroy(connectingCamera);
        Destroy(connectingCanvas);
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
