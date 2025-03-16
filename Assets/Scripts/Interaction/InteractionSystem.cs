// decided to finally remake my crummy ass system for interaction instead of using the piece of trash that was the old one.
using UnityEngine;
using TMPro;

namespace GooberInteraction{
    // note: all interactables must have a photon view set to takeover, and tag
    // not neccsary (exepct for E&Ds), but adding a InteractableInfo script lets you not use the default prompts
    // oh yea can you comment more, ive been tryin to do that more lately :D
    public class InteractionSystem : MonoBehaviour{
        [Header("Refrences")]
        public Transform endPoint;
        [Space]
        public GameObject interactionUi;
        public TextMeshProUGUI interactionTextPrompt;
        [Header("Settings")]
        public string[] tags;
        bool interacting;

        void Update(){
            CheckForInteractables();
        }

        // heres where all the code for handling the interactables go
        void CheckForInteractables() {
            if (Physics.Linecast(transform.forward, endPoint.position, out RaycastHit objectInfo)){
                GameObject objectHit = objectInfo.collider.gameObject;
                if (objectInfo.collider.gameObject.tag == tags[0]){
                    interactionUi.SetActive(true);
                }
                else {
                    interactionUi.SetActive(false);
                }
            }
            else {
                interactionUi.SetActive(false);
            }
        }

        // down here is where all of the interactable code goes
    }
}