using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ChangeCosmeticMid : MonoBehaviourPunCallbacks
{
    public RoomManager roomManager;
    public int cosmeticId;

    public void AddId()
    {
        cosmeticId++;
        roomManager.cosmeticSystem.gameObject.GetComponent<PhotonView>().RPC("setCosmetic", RpcTarget.AllBuffered, cosmeticId);
    }

    public void RemoveId()
    {
        cosmeticId--;
        roomManager.cosmeticSystem.gameObject.GetComponent<PhotonView>().RPC("setCosmetic", RpcTarget.AllBuffered, cosmeticId);
    }
}