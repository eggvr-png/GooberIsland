using Photon.Pun;
using System.Collections;
using UnityEngine;

public class INSOrignal : MonoBehaviourPunCallbacks
{
    public bool interactable;

    public void interact()
    {
        if (interactable){
            // insert interact code here
            interactable = false;
            StartCoroutine(debounce());
        }
    }

    public IEnumerator debounce(){
        yield return new WaitForSeconds(0.1f);
        interactable = true;
    }
}