using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
using Unity.VisualScripting;
using UnityEngine;

// also can be used for other audio related interactions

public class SubtitlesOnInteract : MonoBehaviourPunCallbacks, IInteractable
{
    // this is the text that shows when you look at the object
    public string interactionText => interacting ? "Show a new one" : "Show"; // this for example would be: "grab"
    [Header("Settings")]
    public float debounceTime = 1f;
    [Space]
    public bool canBeInteracted = true;
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.
    [Header("Refrences")]
    public string characterName;
    public string lineNum;
    public string lineNum2;
    [Space]
    public Color color;
    [Space]
    public Subtitles sys;
    [PunRPC]
    public void interact() {
        if (!interacting) {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            sys.displaySubtitle(characterName, lineNum, color);

            interacting = true;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            if (this.gameObject.GetComponent<PhotonView>())
                this.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

            sys.displaySubtitle(characterName, lineNum2, color);

            interacting = false;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
    }

    IEnumerator debounce() {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }

}
