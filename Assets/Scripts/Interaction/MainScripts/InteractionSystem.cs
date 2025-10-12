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

    public bool grabbing;
    Grabbable currentGrabable;

    public float zoomSpeed = 0.5f;
    public float rotationSpeed = 10f;

    public float maxZoom = 2f;

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
                    //lmao grabable=grabbable as Grabbable - max
                }
            }
        }

        if (grabbing && currentGrabable != null)
        {
            if (!Input.GetMouseButton(0))
            {
                currentGrabable.stop();
                grabbing = false;
                currentGrabable = null;
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
                //zoom

                float newGrabOffset = currentGrabable.grabOffset + (-Input.mouseScrollDelta.y * zoomSpeed); // use float
                if (Mathf.Abs(newGrabOffset) < maxZoom)
                    currentGrabable.grabOffset = newGrabOffset;
            }
        }
    }
}
