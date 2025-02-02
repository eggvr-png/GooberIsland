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

    public LobbySettings ls;

    private float pollingTime = 1f;
    private float time;
    private int frames;

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
        // lobby max
        maxPlayerTMP.text = ls.maxPlayers.ToString() + " player max";
    }
}
