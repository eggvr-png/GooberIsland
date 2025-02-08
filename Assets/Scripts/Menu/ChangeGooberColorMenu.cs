using System.Collections;
using System.Collections.Generic;
using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.UI;

public class ChangeGooberColorMenu : MonoBehaviour
{
    public Color green;
    public Color blue;
    public Color pink;

    public Color yellow;

    public RawImage limbs;

    void Start(){
        int savedColor = PlayerPrefs.GetInt("color");

        if (savedColor == 0 || savedColor == 1){
            limbs.color = green;
        }
        else if (savedColor == 2){
            limbs.color = blue;
        }
        else if (savedColor == 3){
            limbs.color = pink;
        }
        else if (savedColor == 4){
            limbs.color = yellow;
        }
    }

    public void changeGreen(){
        limbs.color = green;
        PlayerPrefs.SetInt("color", 1);
    }

    public void changeBlue(){
        limbs.color = blue;
        PlayerPrefs.SetInt("color", 2);
    }

    public void changePink(){
        limbs.color = pink;
        PlayerPrefs.SetInt("color", 3);
    }

    public void changeYellow(){
        limbs.color = yellow;
        PlayerPrefs.SetInt("color", 4);
    }
}

