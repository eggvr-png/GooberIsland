using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerReady : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public GameObject normalPlayerMenu;
    [Space]
    public GameObject hostMenuNormal;
    public GameObject hostMenuReady;
    public GameObject startingMenu;
    [Space]
    public GameObject raftObjects;
    [Space]
    public TextMeshProUGUI[] playerCountTexts;
    public TextMeshProUGUI[] readyTexts;
    [Header("Players")]
    public int playerCount;
    public float numberOfPlayersNeededToStart;
    public int playersReady;
    [Space]
    public bool canStart;
    [Header("Master")]
    public bool master;
    public bool normal;
    [Header("Loader")]
    public LoadScene sceneLoader;

    bool debounce;

    bool readyed;

    bool started;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Raft")
        {
            raftObjects.SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            master = true;
            hostMenuNormal.SetActive(true);
        }
        else
        {
            normal = true;
            normalPlayerMenu.SetActive(true);
        }

        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        numberOfPlayersNeededToStart = playerCount * 0.75f;
        numberOfPlayersNeededToStart = Mathf.Round(numberOfPlayersNeededToStart);

        foreach (TextMeshProUGUI countText in playerCountTexts)
        {
            countText.text = "[" + playersReady.ToString() + "/" + playerCount.ToString() + "] people ready";
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            if (!debounce)
            {
                if (!readyed)
                {
                    debounce = true;
                    this.GetComponentInParent<PhotonView>().RPC("ready", RpcTarget.All);
                    StartCoroutine(debouncer());
                    readyed = true;
                }
                else if (readyed)
                {
                    debounce = true;
                    this.GetComponentInParent<PhotonView>().RPC("unready", RpcTarget.All);
                    StartCoroutine(debouncer());
                    readyed = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.N) && canStart && !started)
        {
            started = true;
            this.GetComponentInParent<PhotonView>().RPC("startGame", RpcTarget.All);
        }

        if (playersReady >= numberOfPlayersNeededToStart && PhotonNetwork.IsMasterClient && !started)
        {
            hostMenuNormal.SetActive(false);
            hostMenuReady.SetActive(true);

            canStart = true;
        }
        else if (playersReady != numberOfPlayersNeededToStart && PhotonNetwork.IsMasterClient && !started)
        {
            hostMenuNormal.SetActive(true);
            hostMenuReady.SetActive(false);

            canStart = false;
        }
    }

    [PunRPC]
    public void ready()
    {
        playersReady++;

        foreach (TextMeshProUGUI countText in playerCountTexts)
        {
            countText.text = "[" + playersReady.ToString() + "/" + playerCount.ToString() + "] people ready";
        }
    }

    [PunRPC]
    public void unready()
    {
        playersReady--;

        foreach (TextMeshProUGUI countText in playerCountTexts)
        {
            countText.text = "[" + playersReady.ToString() + "/" + playerCount.ToString() + "] people ready";
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        numberOfPlayersNeededToStart = playerCount * 0.75f;

        foreach (TextMeshProUGUI countText in playerCountTexts)
        {
            countText.text = "[" + playersReady.ToString() + "/" + playerCount.ToString() + "] people ready";
        }

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        numberOfPlayersNeededToStart = playerCount * 0.75f;

        foreach (TextMeshProUGUI countText in playerCountTexts)
        {
            countText.text = "[" + playersReady.ToString() + "/" + playerCount.ToString() + "] people ready";
        }
    }

    IEnumerator debouncer()
    {
        yield return new WaitForSeconds(0.5f);
        debounce = false;
    }

    [PunRPC]
    public void startGame()
    {
        if (canStart)
        {
            Debug.Log("starting game :D");
            hostMenuNormal.SetActive(false);
            hostMenuReady.SetActive(false);
            startingMenu.SetActive(true);
            StartCoroutine(waitTime(2.5f));
        } else
        {
            Debug.Log("cant start just yet D:");
        }
    }

    IEnumerator waitTime(float wait)
    {
        yield return new WaitForSeconds(wait);
        sceneLoader.SwitchScene();
    }
}
