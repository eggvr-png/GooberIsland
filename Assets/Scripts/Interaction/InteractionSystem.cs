// decided to finally remake my crummy ass system for interaction instead of using the piece of trash that was the old one. is it still bad, yes. is it better kinda. yes.
using UnityEngine;
using TMPro;
using PlayFab;
using Photon.Pun;

namespace GooberInteraction
{
    // note: all interactables must have a photon view set to takeover, and tag
    // oh yea can you comment more, ive been tryin to do that more lately :D
    public class InteractionSystem : MonoBehaviour
    {
        [Header("Refrences")]
        public Transform startPoint;
        public Transform endPoint;
        public Transform grabPoint;
        [Space]
        public GameObject interactionUi;
        public TextMeshProUGUI interactionTextPrompt;
        [Space]
        public PlayerSetup playerSetup;
        [Header("Settings")]
        public string[] tags;
        /// <summary>
        /// tag 1: Radio
        /// tag 2: Grabbable
        /// </summary>
        bool interacting;

        bool isGrabbing;
        Grabbable lastGrabbable;

        Vector3 ogGrabPoint;

        public float minScrollDistance = 1;
        public float maxScrollDistance = 3f;

        void Awake()
        {
            interactionUi = playerSetup.interactionUI;
            interactionTextPrompt = playerSetup.interactionText;

            ogGrabPoint = grabPoint.localPosition;
        }

        void Update()
        {
            CheckForInteractables();

            if (isGrabbing)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0f)
                {
                    Vector3 direction = (transform.position - grabPoint.position).normalized;


                    float distance = Vector3.Distance(transform.position, grabPoint.position + (direction * scroll * 2f));



                    if (distance > minScrollDistance && distance < maxScrollDistance)
                    {
                        grabPoint.position += direction * scroll * 2f;
                    }

                }


                if (Input.GetMouseButtonDown(0))
                {;
                    if (lastGrabbable != null)
                    {

                        Vector3 direction = (transform.position - grabPoint.position).normalized;

                        

                        //ungrab it
                        TryGrab(lastGrabbable.GetComponent<Grabbable>());

                        //fling it
                        lastGrabbable.GetComponent<Rigidbody>().AddForce(-direction * 500); //idk why it needs so much force but it works :/ -max
                    }
                }
            }
            else if (grabPoint.localPosition != ogGrabPoint)
            {
                grabPoint.localPosition = ogGrabPoint;
            }
        }

        // heres where all the code for handling the interactables go
        void CheckForInteractables()
        {
            if (Physics.Linecast(startPoint.position, endPoint.position, out RaycastHit objectInfo))
            {
                GameObject objectHit = objectInfo.collider.gameObject;

                // Radio
                if (objectInfo.collider.gameObject.tag == tags[0])
                {
                    interacting = true;
                    interactionUi.SetActive(true);
                    Radio interactionScript = objectHit.GetComponent<Radio>();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (interactionScript.canBeInteracted)
                        {
                            interactionScript.GetComponent<PhotonView>().RPC("interact", RpcTarget.All);
                        }
                    }

                    if (interactionScript.interacting)
                    {
                        interactionTextPrompt.text = interactionScript.interaction2;
                    }
                    else
                    {
                        interactionTextPrompt.text = interactionScript.interaction1;
                    }
                    interactionUi.SetActive(true);

                    return;
                }
                // Grabbable
                else if (objectInfo.collider.gameObject.tag == tags[1])
                {
                    interacting = true;
                    interactionUi.SetActive(true);

                    Grabbable interactionScript = objectHit.GetComponent<Grabbable>();

                    if (Input.GetKeyDown(KeyCode.E))
                    TryGrab(interactionScript);


                    if (interactionScript.interacting)
                    {
                        interactionTextPrompt.text = interactionScript.interaction2;
                    }
                    else
                    {
                        interactionTextPrompt.text = interactionScript.interaction1;
                    }

                    return;
                }
                else
                {
                    interactionUi.SetActive(false);
                    interacting = false;
                }
            }
            else
            {
                interactionUi.SetActive(false);
            }
        }
        void TryGrab(Grabbable interactionScript)
        {


            if (interactionScript.canBeInteracted)
            {
                interactionScript.getGrabPoint(grabPoint);
                interactionScript.GetComponent<PhotonView>().RPC("interact", RpcTarget.All);


                lastGrabbable = interactionScript;

                if (interactionScript.interacting)
                {
                    isGrabbing = true;
                }
                else
                {
                    isGrabbing = false;
                }
            }




        }
    }
}