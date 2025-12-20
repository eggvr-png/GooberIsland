using Photon.Pun;
using TMPro;
using UnityEngine;

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

    public bool grabbing;
    Grabbable currentGrabable;
    IInteractable currentInteractable;

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
                textToShow = interactable.interactionText;
                showUI = true;

                if (Input.GetKeyDown(interactKey))
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
                // only swap to grabbable text while grabbing or if there's no interactable :D
                if (grabbing || !hasInteractable)
                {
                    textToShow = grabbable.interactionText;
                    showUI = true;
                }

                if (Input.GetMouseButton(0) && !grabbing)
                {
                    grabbing = true;
                    grabbable.grab();
                    currentGrabable = grabbable as Grabbable;
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

            if (!Input.GetMouseButton(0))
            {
                currentGrabable.stop();
                grabbing = false;
                currentGrabable = null;
                interactionUI.SetActive(false);
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
    }

}
