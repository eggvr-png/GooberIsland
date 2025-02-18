using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.InputSystem.Composites;

public class PlayfabManager : MonoBehaviour
{
    public TextMeshProUGUI playfabId;
    [Header("Player Data")]
    [SerializeField] private string playerName;
    [Header("Player Data Objects")]
    public GameObject kofiColor;
    public TMP_InputField nameField;

    public void Login(){
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void getPlayerData(){
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnPlayerDataReceived, OnError);
    }

    public void submitPlayerName(string newName){
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = newName
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnNameChanged, OnError);
    }

    // Errors & Success
    void OnError(PlayFabError error){
        Debug.Log("FUCK, AN PLAYFAB ERROR: " + error.GenerateErrorReport() + "  >:(");
    }

    void OnLoginSuccess(LoginResult result){
        Debug.Log("Logged into Playfab :D");
        playfabId.text = result.PlayFabId;

        getPlayerData();

        string playerName = null;
        if (result.InfoResultPayload.PlayerProfile != null) {
            playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
            nameField.text = playerName;
        }  
    }

    void OnNameChanged(UpdateUserTitleDisplayNameResult result){
        Debug.Log("Playfab Display Name changed");
    }

    void OnPlayerDataReceived(GetUserDataResult result){
        Debug.Log("Recived player data :D");
        if (result.Data != null){
            if (result.Data["KOFI"].Value.ToString() == "1"){
                Debug.Log("Player is Kofi supporter");
                kofiColor.SetActive(true);
            }
        }
    }
}
