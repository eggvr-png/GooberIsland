using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class NameSystem : MonoBehaviour
{
    [SerializeField] private string name;
    private string savedName;
    [Header("In Game Settings")]
    public TextMeshPro nametag;
    [Header("Menu Settings")]
    string namePlayerPref = "playerName";
    public PlayfabManager playfab;

    public void SaveName(string newName){
        name = newName;
        PlayerPrefs.SetString(namePlayerPref,name);
    }

    public void GetName(){
        savedName = PlayerPrefs.GetString(namePlayerPref);
    }

    public void SetNametag(){
        if (savedName == null || savedName == ""){
            savedName = PlayerPrefs.GetString(namePlayerPref);
            if (savedName == null || savedName == ""){
                PlayerPrefs.SetString(namePlayerPref, "Goober");
            }
        }
    }
}
