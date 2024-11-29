using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPromptHandler : MonoBehaviour
{
    public GameObject tutorialWin;
    public TextMeshProUGUI title;
    public TextMeshProUGUI body;

    public KeyCode hideKey;

    bool isTutPromptActive;

    public void sendTutorialPrompt(string promptTitle, string promptBody)
    {
        title.text = promptTitle;
        body.text = promptBody;
        tutorialWin.SetActive(true); 
        isTutPromptActive = true;
    }

    // is this good for preformence? no. probally not. does it work? yea. yea it does
    void Update(){
        if (Input.GetKey(hideKey)){
            if (isTutPromptActive){
                isTutPromptActive = false;
                tutorialWin.SetActive(false);
            }
        }
    }
}
