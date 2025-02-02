using Photon.Pun;
using System.Collections;
using UnityEngine;

public class INSTalk : MonoBehaviourPunCallbacks
{
    public bool interactable;
    public RoomManager rm;
    public Color colorOfBox;
    public string whatToSay;
    public float talkingSecs;

    public void interact()
    {
        if (interactable){
            rm.tbtHolder.SetActive(true);
            rm.talkBox.color = colorOfBox;
            rm.talkBox.gameObject.SetActive(true);
            rm.talktext.text = whatToSay;
            rm.tbtHolder.SetActive(true);
            rm.talktext.GetComponentInParent<typewriterUI>().talk();

            interactable = false;
            StartCoroutine(debounce());
        }
    }

    public IEnumerator debounce(){
        yield return new WaitForSeconds(talkingSecs);
        interactable = true;
        rm.tbtHolder.SetActive(false);
        rm.talkBox.gameObject.SetActive(false);
        rm.tbtHolder.SetActive(false);
    }
}