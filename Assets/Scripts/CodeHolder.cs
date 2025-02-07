using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeHolder : MonoBehaviour
{
    public TMP_InputField codeInput;
    public TMP_InputField nameInput;
    public Button joinButton;
    public Button joinRandomButton;

    void Start()
    {
        InterSceneDataKeeper.Instance.playerName = PlayerPrefs.GetString("playerName");
        if (nameInput)
        nameInput.text = InterSceneDataKeeper.Instance.playerName;
    }
    void Update(){
        if (joinButton == null || joinRandomButton == null) return;
        joinButton.enabled = codeInput.text.Length > 0 && nameInput.text.Length > 0;
        joinRandomButton.enabled = nameInput.text.Length > 0;
    }
    public void changeCode(string theNewCode){
        InterSceneDataKeeper.Instance.roomCode = theNewCode;
    }

    public void changeName(string theNewName){
        InterSceneDataKeeper.Instance.playerName = theNewName;
        PlayerPrefs.SetString("playerName", theNewName);
    }
}
