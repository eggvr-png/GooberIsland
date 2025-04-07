using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UIElements;
using System;

public class DebugMenu : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI fpsTMP;
    public TextMeshProUGUI pingTMP;
    public TextMeshProUGUI maxPlayerTMP;

    public GameObject menu;

    private float pollingTime = 1f;
    private float time;
    private int frames;

    bool debounce;
    bool menuEnabled;

    void Update(){
        // fps
        time += Time.unscaledDeltaTime;
        frames++;
        if (time >= pollingTime){
            int frameRate = Mathf.RoundToInt(frames / time);
            fpsTMP.text = frameRate.ToString() + " FPS";
            time -= pollingTime;
            frames = 0;
        }
        // ping
        float ping;
        ping = PhotonNetwork.GetPing();
        pingTMP.text = ping.ToString() + "MS";

        OpenMenu();
    }

    void OpenMenu() {
        if (Input.GetKey(KeyCode.F3) && !debounce){
            if (menuEnabled){
                menu.SetActive(false);
                menuEnabled = false;
                debounce = true;
                StartCoroutine(debouncer());
            }
            else {
                menu.SetActive(true);
                menuEnabled = true;
                debounce = true;
                StartCoroutine(debouncer());
            }
        }
    }

    IEnumerator debouncer() {
        yield return new WaitForSeconds(0.1f);
        debounce = false;
    }
}
