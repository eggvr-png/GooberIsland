using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotesMenu : MonoBehaviour
{
    bool mightBeEmoting;

    EmotesController emotesController;

    void TryFindController()
    {
        foreach (EmotesController emotes in FindObjectsByType<EmotesController>(FindObjectsSortMode.None))
        {
            if (emotes.photonView.IsMine)
            {
                emotesController = emotes; //ah yes very big sense make
                break;
            }
        }
    }
    void Update()
    {
        if (!emotesController)
        {
            TryFindController();
            return;
        }
        if (Input.GetKeyDown(KeyCode.B)) mightBeEmoting = false;
        bool shouldBeOpen = Input.GetKey(KeyCode.B) && !mightBeEmoting; //wacky but works (i havent tested at the time of writing)
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf && !shouldBeOpen)
            {

                RectTransform rectTransform = child.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
                    //is the mouse hovering emote thingy
                    if (rectTransform.rect.Contains(localMousePosition))
                    {
                        emotesController.PlayEmote(child.GetComponentInChildren<TextMeshProUGUI>().text);
                        mightBeEmoting = true;
                    }

                }

                child.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
            else if (!child.gameObject.activeSelf && shouldBeOpen)
            {
                child.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (child.gameObject.activeSelf)
            {
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
                    //is the mouse hovering emote thingy
                    if (rectTransform.rect.Contains(localMousePosition))
                    {
                        child.GetComponent<Image>().color = Color.blue;
                        //the 4th nested if statement in a foreach loop im going insane
                        if (Input.GetMouseButtonDown(0))
                        {
                            emotesController.PlayEmote(child.GetComponentInChildren<TextMeshProUGUI>().text);
                            mightBeEmoting = true;

                        }
                    }
                    else
                        child.GetComponent<Image>().color = Color.white;
                }
                //bad code
            }
        }
    }

}
