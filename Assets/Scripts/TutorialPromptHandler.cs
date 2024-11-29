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

    public void sendTutorialPrompt(string promptTitle, string promptBody)
    {
        title.text = promptTitle;
        body.text = promptBody;
        tutorialWin.SetActive(true);  
        StartCoroutine(holdTime());
    }
    
    public IEnumerator holdTime(){
        yield return new WaitForSeconds(7f);
        tutorialWin.SetActive(false);
    }
}
