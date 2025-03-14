using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButonOnSV1 : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Toggle toggleToEnable;
    public Button buttonToEnable;

    void Update()
    {
        if(scrollbar.value <= 0){
            toggleToEnable.interactable = true;
        }
        else{
            toggleToEnable.interactable = false;
        }

        if (toggleToEnable.isOn){
            buttonToEnable.interactable = true;
        }
        else{
            buttonToEnable.interactable = false;
        }
    }
}
