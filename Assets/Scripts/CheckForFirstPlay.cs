using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForFirstPlay : MonoBehaviour
{
    public string firstPlayerPP;
    public TutorialPromptHandler tph;

    public void Check(){
        if (PlayerPrefs.GetInt(firstPlayerPP) == 0){
            tph.sendTutorialPrompt("Fresh Start", "hiya! use wasd to move, space to jump, and crtl to crouch");
            PlayerPrefs.SetInt(firstPlayerPP, 1);
        }
    }
}
