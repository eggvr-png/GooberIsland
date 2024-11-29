using UnityEngine;
using Photon.Pun;

public class INSGrab : MonoBehaviourPun
{
    public bool grabbed = false;
    public string grabPrompt = "Press E to Grab";
    public string putdownPrompt = "Press E to Put Down";

    public Transform grabPoint;

    private Transform playerCamera; // Reference to the player's camera
    private float rotationSpeed = 5f; // Adjust this value as needed

    private void Start()
    {
        // Assuming the player's camera is tagged as "MainCamera"
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        if (grabbed)
        {
            HandleRotation();
        }
    }

    public void interact()
    {
        if (!grabbed)
        {
            GrabItem();
        }
        else
        {
            PutDownItem();
        }
    }

    private void GrabItem()
    {
        grabbed = true;
        // Parent the item to the player or camera so it moves with the player
        transform.position = Vector3.Lerp(transform.position, grabPoint.position, Time.deltaTime * 10f);
        // Reset rotation and position relative to the camera
        transform.localPosition = new Vector3(0, 0, 2); // Adjust as needed
        transform.localRotation = Quaternion.identity;
        // Disable physics while holding
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void PutDownItem()
    {
        grabbed = false;
        // Unparent the item
        transform.SetParent(null);
        // Re-enable physics
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.R))
        {
            // Get mouse movement
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Apply rotation based on mouse movement
            transform.Rotate(playerCamera.up, -mouseX, Space.World); // Rotate around Y-axis
            transform.Rotate(playerCamera.right, mouseY, Space.World); // Rotate around X-axis
        }
    }
}