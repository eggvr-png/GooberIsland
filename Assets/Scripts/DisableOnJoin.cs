using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DisableOnJoin : MonoBehaviourPunCallbacks
{

    public override void OnJoinedRoom()
    {
        Destroy(gameObject);
    }
}

