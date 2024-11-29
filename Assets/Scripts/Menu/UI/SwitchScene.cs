using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SwitchScene : MonoBehaviourPunCallbacks
{
    public string sceneName;

    public void switchScene(){
        SceneManager.LoadScene(sceneName);
    }

    public void switchSceneDelayed(float seconds){
        StartCoroutine(delayed(seconds));
    }

    public void ssdm(float seconds){
        PlayerPrefs.SetInt("RTMFG", 1);
        switchSceneDelayed(seconds);
    }

    private IEnumerator delayed(float delayedfor){
        yield return new WaitForSeconds(delayedfor);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }
}
