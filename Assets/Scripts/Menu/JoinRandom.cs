using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JoinRandom : MonoBehaviour
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
        code = PlayerPrefs.GetString(codePP);
    }

    public void JoinRandomRoom(){
        PlayerPrefs.SetString(codePP, "RandomCode1234");
        SceneManager.LoadScene("Raft");
    }
}
