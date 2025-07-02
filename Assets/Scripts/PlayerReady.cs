using System.Collections;
using UnityEngine;
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

    bool debounce;

    bool readyed;

    void Start()
    {
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

        if (playersReady >= numberOfPlayersNeededToStart && PhotonNetwork.IsMasterClient)
        {
            hostMenuNormal.SetActive(false);
            hostMenuReady.SetActive(true);

            canStart = true;
        }
        else if (playersReady != numberOfPlayersNeededToStart && PhotonNetwork.IsMasterClient)
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
}
