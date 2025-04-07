using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OfflineButton : MonoBehaviour
{
    public void enableOffline(){
        PhotonNetwork.OfflineMode = true;
        PlayerPrefs.SetInt("offline", 1);
    }

    public void disableOffline(){
        PhotonNetwork.OfflineMode = false;
        PlayerPrefs.SetInt("offline", 0);
    }
}
