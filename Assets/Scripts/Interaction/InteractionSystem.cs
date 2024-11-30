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
    [Header("References")]
    public Transform rayLength;
    public RoomManager rm;
    public TutorialPromptHandler tph;
    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;

    public float waitTime = 0.2f;
    public bool abletoInteract;

    // Variables for handling grabbed object
    private GameObject grabbedObject;
    private float holdDistance = 2.0f;
    private float minHoldDistance = 1.0f;
    private float maxHoldDistance = 5.0f;

    // Public properties to check from other scripts
    public bool IsObjectHeld
    {
        get { return grabbedObject != null; }
    }

    public bool IsRotatingObject
    {
        get { return grabbedObject != null && Input.GetKey(KeyCode.R); }
    }

    private void Update()
    {
        CheckForCollision();

        // Handle input for the grabbed object
        if (grabbedObject != null)
        {
            HandleGrabbedObject();
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

                if (Input.GetKey(interactKey))
                {
                    if (abletoInteract)
                    {
                        script.interact();
                        StartCoroutine(wait());

                        // Update grabbedObject reference
                        if (script.grabbed)
                        {
                            grabbedObject = hit.collider.gameObject;
                            holdDistance = Vector3.Distance(transform.position, grabbedObject.transform.position);
                        }
                        else
                        {
                            grabbedObject = null;
                        }
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

    private void HandleGrabbedObject()
    {
        // Handle scroll wheel to adjust hold distance
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            holdDistance += scrollInput * 2.0f; // Adjust multiplier as needed
            holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
        }

        // Update position of the grabbed object
        grabbedObject.transform.position = transform.position + transform.forward * holdDistance;

        // Handle rotation when 'R' is held
        if (Input.GetKey(KeyCode.R))
        {
            float rotationSpeed = 5f; // Adjust as needed
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

            grabbedObject.transform.Rotate(transform.up, mouseX, Space.World);
            grabbedObject.transform.Rotate(transform.right, mouseY, Space.World);
        }
    }

    IEnumerator wait()
    {
        abletoInteract = false;
        yield return new WaitForSeconds(waitTime);
        abletoInteract = true;
    }
}