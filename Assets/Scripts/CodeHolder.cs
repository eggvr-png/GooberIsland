using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHolder : MonoBehaviour
{
    public string namePP;
    public string codePP;
    [Header("Code/Name")]
    public string name = "Goober";
    public string code = "CFC12423";
    [Header("New Code/Name")]
    public string newName;
    public string newCode;

    public void GetCodeAndName(){
        name = PlayerPrefs.GetString(namePP);
        code = PlayerPrefs.GetString(codePP, "RandomCode1234");
    }

    public void SetCodeAndName(){
        PlayerPrefs.SetString(namePP, newName);
        PlayerPrefs.SetString(codePP, newCode);
    }

    public void changeCode(string theNewCode){
        newCode = "Raft" + theNewCode;
    }

    public void changeName(string theNewName){
        newName = theNewName;
    }
}
