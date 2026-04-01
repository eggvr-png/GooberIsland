using System.Collections;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

interface IInteractable
{
    string interactionText { get; }
    public void interact();
}

interface IGrabbable
{
    string interactionText { get; }
    public void grab();

    public void stop();
}

public class InteractionSystem : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode interactKey;
    [Space]
    public float zoomSpeed = 0.5f;
    public float rotationSpeed = 10f;
    [Space]
    public float maxZoom = 2f;
    [Header("Refrences")]
    public Transform cameraTransform;
    [Space]
    public Transform startPos;
    public Transform endPos;
    [Space]
    public GameObject interactionUI;
    public TextMeshProUGUI iText;
    public RawImage icon;
    public Texture2D[] icons;
    [Space]
    public ConnectionManager cm;

    public bool grabbing;
    Grabbable currentGrabable;
    IInteractable currentInteractable;

    bool stashed;
    bool stashDebounce;
    public GameObject stashedObj;
    public AudioSource soundPlayer;
    public AudioClip[] sounds;
    public TextMeshProUGUI stashText;
    public TutorialPrompt prompt;

    bool showedPrompt;

    void Start()
    {
        if (PlayerPrefs.GetInt("InteractionPrompt") == 1)
        {
            showedPrompt = true;
        }
    }

    void Update()
    {
        if (Physics.Linecast(startPos.position, endPos.position, out RaycastHit hit))
        {
            string textToShow = "";
            bool showUI = false;

            bool hasInteractable = hit.collider.gameObject.TryGetComponent<IInteractable>(out var interactable);
            bool hasGrabbable = hit.collider.gameObject.TryGetComponent<IGrabbable>(out var grabbable);

            // check interactable first because interactables take priotuy
            if (hasInteractable)
            {
                if (!showedPrompt) {
                    prompt.showPrompt("Interaction", "To interact, press E. To hold grabbables, hold left click.");
                    showedPrompt = true;
                    PlayerPrefs.SetInt("InteractionPrompt", 1);
                    PlayerPrefs.Save();
                }

                icon.texture = icons[0];
                
                textToShow = interactable.interactionText;
                showUI = true;

                if (Input.GetKeyDown(interactKey) || Input.GetButtonDown("Interact"))
                {
                    PhotonView pv = hit.collider.gameObject.GetComponent<PhotonView>();
                    if (pv == null)
                    {
                        hit.collider.gameObject.TryGetComponent<IInteractable>(out var inter);
                        inter.interact();
                    }
                    else
                    {
                        pv.RPC("interact", RpcTarget.All);
                    }
                }
            }
            
            if (hasGrabbable)
            {
                if (!showedPrompt) {
                    prompt.showPrompt("Interaction", "To interact, press E. To hold grabbables, hold left click.");
                    showedPrompt = true;
                    PlayerPrefs.SetInt("InteractionPrompt", 1);
                    PlayerPrefs.Save();
                }
                
                if (grabbing || !hasInteractable)
                {
                    textToShow = grabbable.interactionText;
                    showUI = true;
                    icon.texture = icons[1];
                }

                if (Input.GetJoystickNames().Length > 0) {
                    if (Input.GetButton("Interact") && !grabbing)
                    {
                        Grabbable g = grabbable as Grabbable;
                        if (g == null || g.canBeInteracted) {
                            grabbing = true;
                            grabbable.grab();
                            currentGrabable = g;
                        }
                    }
                    else if (Input.GetMouseButton(0) && !grabbing)
                    {
                        Grabbable g = grabbable as Grabbable;
                        if (g == null || g.canBeInteracted) {
                            grabbing = true;
                            grabbable.grab();
                            currentGrabable = g;
                        }
                    }
                }
                else if (Input.GetMouseButton(0) && !grabbing)
                {
                    Grabbable g = grabbable as Grabbable;
                    if (g == null || g.canBeInteracted) {
                        grabbing = true;
                        grabbable.grab();
                        currentGrabable = g;
                    }
                }
            }

            // actually show ui because it needs to show it or else death
            interactionUI.SetActive(showUI);
            iText.text = textToShow;
        }
        else {
            interactionUI.SetActive(false);
        }

        // rotating stuff i guess, i dunno lol
        if (grabbing && currentGrabable != null)
        {
            interactionUI.SetActive(true);

            if (Input.GetJoystickNames().Length > 0)
            {
                if (!Input.GetButton("Interact"))
                {
                    currentGrabable.stop();
                    grabbing = false;
                    currentGrabable = null;
                    interactionUI.SetActive(false);
                }
                else if (!Input.GetMouseButton(0))
                {
                    currentGrabable.stop();
                    grabbing = false;
                    currentGrabable = null;
                    interactionUI.SetActive(false);
                }
            }
            else if (!Input.GetMouseButton(0))
            {
                currentGrabable.stop();
                grabbing = false;
                currentGrabable = null;
                interactionUI.SetActive(false);
            }

            if (PhotonNetwork.IsConnected) {
                if (Input.GetKeyDown(KeyCode.Q) && !stashDebounce && !stashed)
                {
                    GameObject player = cm.playerPub;
                    GameObject grabbableObj = currentGrabable.gameObject;

                    stashText.text = grabbableObj.name;

                    soundPlayer.PlayOneShot(sounds[1]);

                    currentGrabable.interacting = false;
                    
                    grabbableObj.GetComponent<PhotonView>().RPC("stash", RpcTarget.All);
                    grabbing = false;
                    grabbableObj.transform.SetParent(player.transform);
                    stashedObj = grabbableObj;

                    stashed = true;
                    stashDebounce = true;
                    StartCoroutine(stashDebouncer());

                    Debug.Log("stashed object");
                }
            }

            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime;
                Vector3 rotation = cameraTransform.up * mouseX + cameraTransform.right * -mouseY;
                currentGrabable.Rotate(rotation * rotationSpeed);
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                float newGrabOffset = currentGrabable.grabOffset + (-Input.mouseScrollDelta.y * zoomSpeed);
                if (Mathf.Abs(newGrabOffset) < maxZoom)
                    currentGrabable.grabOffset = newGrabOffset;
            }
        }

        if (!grabbing && stashed && !stashDebounce)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameObject grabbableObj = currentGrabable.gameObject;
                Debug.Log("unstashed object");
                stashedObj.transform.parent = null;
                grabbableObj.GetComponent<PhotonView>().RPC("unstash", RpcTarget.All, endPos.position.x, endPos.position.y, endPos.position.z);
                stashed = false;
                stashDebounce = true;

                stashText.text= "Nothing Stashed";

                soundPlayer.PlayOneShot(sounds[0]);

                StartCoroutine(stashDebouncer());
            }
        }
    }

    IEnumerator stashDebouncer()
    {
        yield return new WaitForSeconds(1f);
        stashDebounce = false;
    }
}
