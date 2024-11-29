using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButonOnSV1 : MonoBehaviour
{
    public Scrollbar scrollbar;
    public GameObject buttonToEnable;

    void Update()
    {
        if(scrollbar.value <= 0){
            buttonToEnable.SetActive(true);
        }
        else{
            buttonToEnable.SetActive(false);
        }
    }
}
