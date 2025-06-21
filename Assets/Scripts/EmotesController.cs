using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EmotesController : MonoBehaviourPunCallbacks
{
    public Animator animator;
    public PhotonView View;

    public Camera mainCamera;
    public Camera emoteCam;

    public GameObject playerModel;

    public static EmotesController Instance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            Instance = this;
        }
    }

    // Save the currently playing emote names
    List<string> currentEmotes = new List<string>();


    public void PlayEmote(string emoteName)
    {
        if (View.IsMine && emoteName.Length > 0)
        {
            View.RPC("PlayEmoteRPC", RpcTarget.All, emoteName);
        }
    }

    public void StopEmote()
    {
        if (View.IsMine && currentEmotes.Count > 0)
        {
            View.RPC("StopEmoteRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void PlayEmoteRPC(string emoteName)
    {
        if (photonView.IsMine)
        {
            //GetComponent<PlayerSetup>().modelToDisable.SetActive(true);
            playerModel.SetActive(true);
            emoteCam.enabled = true;
            mainCamera.enabled = false;
        }
        // Save the currently playing emote
        currentEmotes.Add(emoteName);
        animator.Play(Resources.Load<AnimationClip>("Emotes/" + emoteName).name);
    }

    [PunRPC]
    void StopEmoteRPC()
    {
        if (photonView.IsMine)
        {
            //GetComponent<PlayerSetup>().modelToDisable.SetActive(false);
            playerModel.SetActive(false);
            emoteCam.enabled = false;
            mainCamera.enabled = true;
        }
        animator.Play("Idle");
        // Clear the list of current emotes as the emote stops
        currentEmotes.Clear();
    }
    
    public static bool IsEmoting()
    {
        return Instance.currentEmotes.Count > 0;
    }
}
