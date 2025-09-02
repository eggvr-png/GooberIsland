using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    [Header("Starting")]
    public GameObject menu1;
    [Header("Ending")]
    public GameObject menu2;

    public void change()
    {
        menu1.SetActive(false);
        menu2.SetActive(true);
    }
}
