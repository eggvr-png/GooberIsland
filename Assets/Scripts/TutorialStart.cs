using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialStart : MonoBehaviour
{
    public int secondsTillStart;
    public GameObject talkingFrame;

    public IEnumerator waitForStart(){
        yield return new WaitForSeconds(secondsTillStart);
        talkingFrame.SetActive(true);
    }

    void Start(){
        StartCoroutine(waitForStart());
    }
}
