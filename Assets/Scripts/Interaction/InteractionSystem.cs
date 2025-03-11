using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviourPunCallbacks
{
    // never add the interaction in here. i swear, i was going crazy on why i couldnt replace
    // with the old script since the rotation script thingy made gave lot of bugs.

    Ray ray;
    [Header("References")]
    public Transform rayLength;
    public RoomManager rm;
    public TutorialPromptHandler tph;
    public GameObject[] keycodes;
    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;

    public float waitTime = 0.2f;
    public bool abletoInteract;

    INSGrab lastGrababble;

    Gamepad gamepad;

    void Start(){
        gamepad = Gamepad.current;

        CheckIfController();
    }

    private void Update()
    {
        CheckForCollision();
    }


    void CheckIfController(){
        if (gamepad != null){
            keycodes[0].SetActive(false);
            keycodes[1].SetActive(true);
        }
    }

    void CheckForCollision()
    {
        if (Physics.Linecast(transform.position, rayLength.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "INS ED")
            {
                rm.inUI.SetActive(true);
                INSEnableDisable script = hit.collider.transform.gameObject.GetComponent<INSEnableDisable>();

                if (script.objActive)
                {
                    rm.inText.text = script.disablePrompt;
                }
                else
                {
                    rm.inText.text = script.enablePrompt;
                }

                if (PlayerPrefs.GetInt("FI") == 0)
                {
                    tph.sendTutorialPrompt("Interactions", "To interact with an interactable, use E");
                    PlayerPrefs.SetInt("FI", 1);
                }

                if (Input.GetKeyDown(interactKey) || Input.GetKeyDown(KeyCode.JoystickButton3))
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
            else if (hit.collider.gameObject.tag == "INS Grab")
            {
                rm.inUI.SetActive(true);
                INSGrab script = hit.collider.transform.gameObject.GetComponent<INSGrab>();

                if (script.grabbed)
                {
                    rm.inText.text = script.putdownPrompt;
                }
                else
                {
                    rm.inText.text = script.grabPrompt;
                }

                if (PlayerPrefs.GetInt("FI") == 0)
                {
                    tph.sendTutorialPrompt("Interactions", "To interact with an interactable, use E");
                    PlayerPrefs.SetInt("FI", 1);
                }

                if (Input.GetKeyDown(interactKey) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    if (abletoInteract)
                    {
                        script.interact(rayLength);
                        StartCoroutine(wait());

                        if (script.grabbed != false){
                            lastGrababble = script;
                        }
                        else {
                            lastGrababble = null;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.JoystickButton4)){
                    if (abletoInteract)
                    {
                        if(script.grabbed){
                            script.ThrowRelease();
                            StartCoroutine(wait());
                            if (script.grabbed != false){
                                lastGrababble = script;
                            }
                            else {
                                lastGrababble = null;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (hit.collider.gameObject.tag == "INS Talk")
            {
                rm.inUI.SetActive(true);
                INSTalk script = hit.collider.transform.gameObject.GetComponent<INSTalk>();

                rm.inText.text = "Talk";

                if (PlayerPrefs.GetInt("FI") == 0)
                {
                    tph.sendTutorialPrompt("Interactions", "To interact with an interactable, use E");
                    PlayerPrefs.SetInt("FI", 1);
                }

                if (Input.GetKeyDown(interactKey) || Input.GetKeyDown(KeyCode.JoystickButton3))
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
            else if (hit.collider.gameObject.tag == "Store")
            {
                rm.inUI.SetActive(true);

                OpenCloseStore script = hit.collider.transform.gameObject.GetComponent<OpenCloseStore>();

                if (script.opened)
                {
                    rm.inText.text = script.close;
                }
                else
                {
                    rm.inText.text = script.open;
                }
                

                if (PlayerPrefs.GetInt("FI") == 0)
                {
                    tph.sendTutorialPrompt("Interactions", "To interact with an interactable, use E");
                    PlayerPrefs.SetInt("FI", 1);
                }

                if (Input.GetKeyDown(interactKey) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    if (abletoInteract)
                    {
                        script.interact();
                        StartCoroutine(wait());
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        else
        {
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