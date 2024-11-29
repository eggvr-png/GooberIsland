using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class InteractionSystem : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// this is a old script from the orignal goober island
    /// well besides the new parts of it
    /// im not really planning on changing how it works
    /// i will try to optimize it at a later date
    /// dont call me yandere dev because of that. and what i mean is that yandere did optimize code to good
    /// im not a good coder so my code is that optimized
    /// sorry
    /// and also hi asset ripper
    /// </summary>

    Ray ray;
    [Header("Refrences")]
    public Transform rayLength;
    public RoomManager rm;
    public TutorialPromptHandler tph;
    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;

    public float waitTime = 0.2f;
    public bool abletoInteract;

    private void Update()
    {
        CheckForCollison();
    }

    void CheckForCollison()
    {
        if (Physics.Linecast(transform.position, rayLength.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "INS ED")
            {
                rm.inUI.SetActive(true);
                INSEnableDisable script = hit.collider.transform.gameObject.GetComponent<INSEnableDisable>();
                
                if (script.objActive){
                    rm.inText.text = script.disablePrompt;
                }
                else{
                    rm.inText.text = script.enablePrompt;
                }

                if (PlayerPrefs.GetInt("FI") == 0){
                    tph.sendTutorialPrompt("Interactions", "To interact with a interactable, use E");
                    PlayerPrefs.SetInt("FI", 1);
                }
                
                if (Input.GetKey(interactKey))
                {
                    if (abletoInteract)
                    {
                        script.GetComponentInParent<PhotonView>().RPC("interact", RpcTarget.All);
                        StartCoroutine(wait());
                    }
                    else
                    {
                        return;
                    }
                }
            }
            if (hit.collider.gameObject.tag == "INS Grab")
            {
                rm.inUI.SetActive(true);
                INSGrab script = hit.collider.transform.gameObject.GetComponent<INSGrab>();
                
                if (script.grabbed){
                    rm.inText.text = script.putdownPrompt;
                }
                else{
                    rm.inText.text = script.grabPrompt;
                }

                if (PlayerPrefs.GetInt("FI") == 0){
                    tph.sendTutorialPrompt("Interactions", "To interact with a interactable, use E");
                    PlayerPrefs.SetInt("FI", 1);
                }
                
                if (Input.GetKey(interactKey))
                {
                    if (abletoInteract)
                    {
                        script.interact();
                        StartCoroutine(wait());
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        else {
            rm.inUI.SetActive(false);
            rm.inText.text = "";
        }
    }

    IEnumerator wait()
    {
        abletoInteract = false;
        yield return new WaitForSeconds(waitTime);
        abletoInteract = true;
    }
}