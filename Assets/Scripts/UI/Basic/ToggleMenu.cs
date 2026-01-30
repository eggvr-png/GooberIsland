using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    bool toggled;
    public GameObject menu;

    public void pressed()
    {
        if (!toggled)
        {
            toggled = true;
            menu.SetActive(true);
        }
        else
        {
            toggled = false;
            menu.SetActive(false);
        }
    }
}
