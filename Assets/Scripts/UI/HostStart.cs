using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostStart : MonoBehaviourPunCallbacks
{
    public GameObject loadingScreen;
    public ConnectionManager cm;
    bool isHost;
    bool alreadyPressed;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            isHost = true;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !alreadyPressed)
        {
            alreadyPressed = true;
            Debug.Log("tryin to start");
            if (isHost || PhotonNetwork.IsMasterClient)
            {
                this.gameObject.GetComponent<PhotonView>().RPC("SwitchScene", RpcTarget.All);
                Debug.Log("game started");
            }
            else
            {
                Debug.Log("oof you cant, not host.");
            }
        }
    }

    [PunRPC]
    public void SwitchScene()
    {
        InterSceneDataKeeper.Instance.roomCode = cm.roomCode;
        StartCoroutine(LoadSceneAsync());
    }

    public IEnumerator LoadSceneAsync()
    {
        PhotonNetwork.Disconnect();
        AsyncOperation operation = SceneManager.LoadSceneAsync("Island");
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("loaded scene to: " + progress.ToString());
            yield return null;
        }
    }
}
