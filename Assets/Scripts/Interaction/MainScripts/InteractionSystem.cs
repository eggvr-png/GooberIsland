using UnityEngine;

interface IInteractable
{
    public void interact();
}

interface IGrabbable
{
    public void grab();

    public void stop();
}

public class InteractionSystem : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode interactKey;
    [Header("Refrences")]
    public Transform cameraTransform;
    [Space]
    public Transform startPos;
    public Transform endPos;

    bool grabbing;
    Grabbable currentGrabable;

    void Update()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Linecast(startPos.position, endPos.position, out RaycastHit hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                if (Input.GetKeyDown(interactKey))
                {
                    interactable.interact();
                }
            }

            if (hit.collider.gameObject.TryGetComponent<IGrabbable>(out var grabbable))
            {
                if (Input.GetMouseButton(0) && !grabbing)
                {
                    grabbing = true;
                    grabbable.grab();
                    currentGrabable = grabbable as Grabbable; // thanks gee pee tee for this part right here
                }
            }
        }

        if (grabbing)
        {
            if (!Input.GetMouseButton(0))
            {
                currentGrabable.stop();
                grabbing = false;
                currentGrabable = null;
            }
        }
    }
}
