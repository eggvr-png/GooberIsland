using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;

public class JoinRandom : MonoBehaviourPunCallbacks
{
    string code;
    /*
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomInfo bestRoom = null;
        foreach (RoomInfo room in roomList)
        {
            if (bestRoom == null || (room.PlayerCount > bestRoom.PlayerCount && room.PlayerCount < room.MaxPlayers))
            {
                bestRoom = room;
            }
        }
        if (bestRoom != null)
        {
            code = bestRoom.Name;
        }else{
            code = UnityEngine.Random.Range(1000, 9999).ToString();
        }
    }*/

    public void GetCodeAndName()
    {
    }

    public void JoinRandomRoom(){
        InterSceneDataKeeper.Instance.roomCode = null;
        SceneManager.LoadScene("Raft");
    }
}
