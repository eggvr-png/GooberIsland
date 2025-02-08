using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class CodeHolder : MonoBehaviour
{
    public TMP_InputField codeInput;
    public TMP_InputField nameInput;
    public Button joinButton;
    public Button joinRandomButton;

    public TextMeshProUGUI nameThingy;

    public PlayfabManager pfm;

    void Start()
    {
        InterSceneDataKeeper.Instance.playerName = PlayerPrefs.GetString("playerUsername");
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
        if (nameInput.text.Length < 3){
            nameThingy.text = "name must be 3-25 characters";
        }
        else {
            InterSceneDataKeeper.Instance.playerName = theNewName;
            nameThingy.text = "";
            PlayerPrefs.SetString("playerUsername", theNewName);
            pfm.submitPlayerName(theNewName);
        }
    }
}
