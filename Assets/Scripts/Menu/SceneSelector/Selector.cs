using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using ExitGames.Demos.DemoPunVoice;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tmp;
    string lastText;

    public void OnPointerEnter(PointerEventData eventData){
        lastText = tmp.text;
        tmp.text = "> " + lastText;
    }

    public void OnPointerExit(PointerEventData eventData){
        tmp.text = lastText;
        lastText = "";
    }
}
