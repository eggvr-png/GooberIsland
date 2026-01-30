using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class codeNameCheck : MonoBehaviour
{
    [Header("Refrences")]
    public TMP_InputField nameField;
    public TMP_InputField codeField;
    [Space]
    public Button joinCodeButton;
    public Button ranCodeButton;

    public void Start()
    {
        string lastUser = PlayerPrefs.GetString("playerName");
        if (lastUser != null || lastUser != "")
        {
            nameField.text = lastUser;
        }
    }

    public void Update()
    {
        if (nameField.text.Length >= 3)
        {
            ranCodeButton.interactable = true;
        }
        else
        {
            ranCodeButton.interactable = false;
        }

        if (nameField.text.Length >= 3)
        {
            joinCodeButton.interactable = true;
        }
        else
        {
            joinCodeButton.interactable = false;
        }
    }
}
