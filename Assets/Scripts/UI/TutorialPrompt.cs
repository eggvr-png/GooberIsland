using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPrompt : MonoBehaviour
{
    public GameObject prompt;
    public TextMeshProUGUI title;
    public TextMeshProUGUI body;

    public void showPrompt(string titleText, string bodyText)
    {
        title.text = titleText;
        body.text = bodyText;

        prompt.SetActive(true);
        StartCoroutine(disableAfter());
    }

    IEnumerator disableAfter()
    {
        yield return new WaitForSeconds(11);
        prompt.SetActive(false);
    }
}
