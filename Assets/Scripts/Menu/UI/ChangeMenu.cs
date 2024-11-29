using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    public GameObject orignalMenu;
    public GameObject newMenu;

    public void Switch(){
        newMenu.SetActive(true);
        orignalMenu.SetActive(false);
    }
}
