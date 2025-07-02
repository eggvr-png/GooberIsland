using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotesMenu : MonoBehaviour
{
    bool mightBeEmoting;

    public Color selectColor;

    void Update()
    {
        if (!EmotesController.Instance)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.B)) mightBeEmoting = false;
        bool shouldBeOpen = Input.GetKey(KeyCode.B) && !mightBeEmoting; //wacky but works (i havent tested at the time of writing)
        transform.GetChild(0).gameObject.SetActive(shouldBeOpen);
        foreach (Transform child in transform.GetChild(0))
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
                        EmotesController.Instance.PlayEmote(child.GetComponentInChildren<TextMeshProUGUI>().text);
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
                        child.GetComponent<Image>().color = selectColor;
                        //the 4th nested if statement in a foreach loop im going insane
                        if (Input.GetMouseButtonDown(0))
                        {
                            EmotesController.Instance.PlayEmote(child.GetComponentInChildren<TextMeshProUGUI>().text);
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
