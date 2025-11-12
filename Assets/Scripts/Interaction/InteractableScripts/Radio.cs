using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

// also can be used for other audio related interactions

public class Radio : MonoBehaviourPunCallbacks, IInteractable
{
    // this is the text that shows when you look at the object
    public string interactionText => interacting ? "Turn On" : "Turn Off"; // this for example would be: "grab"
    [Header("Settings")]
    public float debounceTime = 0.1f;
    [Space]
    public bool canBeInteracted = true;
    public bool interacting; // this bool checks if the object has been interacted with or is being interacted.
    [Header("Refrences")]
    public PhotonView pv;
    public AudioSource audioSource;
    public AudioSource staticSource;
    [Header("Materials")]
    public Material[] onMaterials;
    public Material[] offMaterials;
    [Space]
    public GameObject[] screenObjects;

    public void Start()
    {
        if (InterSceneDataKeeper.Instance.currentSong != null)
        {
            audioSource.clip = InterSceneDataKeeper.Instance.currentSong;
            Debug.Log("playing custom radio music");
            audioSource.Play();
        }
    }

    [PunRPC]
    public void interact() {
        if (!interacting) {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);

            StartCoroutine(Pitch(false));

            interacting = true;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
        else {
            pv.TransferOwnership(PhotonNetwork.LocalPlayer);

            StartCoroutine(Pitch(true));

            interacting = false;
            canBeInteracted = false;
            StartCoroutine(debounce());
        }
    }

    IEnumerator debounce() {
        yield return new WaitForSeconds(debounceTime);
        canBeInteracted = true;
    }

    IEnumerator Pitch(bool up)
    {
        if (!up)
        {
            int i = 0;
            screenObjects[0].GetComponent<Renderer>().material = offMaterials[0];
            screenObjects[1].GetComponent<Renderer>().material = offMaterials[1];
            screenObjects[2].GetComponent<Renderer>().material = offMaterials[2];
            //staticSource.Play();
            while (i != 100)
            {
                ++i;
                audioSource.pitch = audioSource.pitch - 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            int i = 0;
            screenObjects[0].GetComponent<Renderer>().material = onMaterials[0];
            screenObjects[1].GetComponent<Renderer>().material = onMaterials[1];
            screenObjects[2].GetComponent<Renderer>().material = onMaterials[2];
            //staticSource.Stop();
            while (i != 100)
            {
                ++i;
                audioSource.pitch = audioSource.pitch + 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    
}
