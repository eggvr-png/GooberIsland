using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayfabManager : MonoBehaviour
{
    public TextMeshProUGUI playfabId;
    public void Login(){
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    // Errors & Success
    void OnError(PlayFabError error){
        Debug.Log("FUCK, AN PLAYFAB ERROR: " + error.GenerateErrorReport());
    }

    void OnLoginSuccess(LoginResult result){
        Debug.Log("Logged into Playfab :D");

        playfabId.text = result.PlayFabId;
    }
}
