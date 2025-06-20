using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using JetBrains.Annotations;
using UnityEngine.InputSystem.Composites;

public class PlayfabManager : MonoBehaviour
{
    public TextMeshProUGUI playfabId;
    [Header("Player Data")]
    [SerializeField] private string playerName;
    [Header("Player Data Objects")]
    public GameObject kofiColor;
    public TMP_InputField nameField;
    [Header("Community")]
    public RawImage[] artRawImages;
    [Header("Menu Stuff")]
    public GameObject bannedMenu;
    public IntroTimer intro;
    [Space]
    public TextMeshProUGUI banTimeText;

    string art1;
    string art2;
    string art3;

    public void Login(){
        RuntimePlatform currentPlatform = Application.platform;

        if (currentPlatform != RuntimePlatform.WebGLPlayer)
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
        }
        else
        {
            Debug.Log("user not logged in due to playing on the WEBGL build :/");
        }
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

    void GetTitleData(){
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataReceived, OnError);
    }

    // Errors & Success
    void OnError(PlayFabError error){
        Debug.Log("SHOOT, A PLAYFAB ERROR: " + error.GenerateErrorReport() + "  >:(");
    }

    void OnLoginError(PlayFabError error){
        if (error.Error == PlayFabErrorCode.AccountBanned){
            Debug.Log("player is banned >:0");
            StartCoroutine(musicPitchDown());
            bannedMenu.SetActive(true);
        }
        else {
            Debug.Log("SHOOT, A PLAYFAB ERROR: " + error.GenerateErrorReport() + "  >:(");
        }
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
        GetTitleData();
    }

    void OnNameChanged(UpdateUserTitleDisplayNameResult result){
        Debug.Log("Playfab Display Name changed");
    }

    void OnPlayerDataReceived(GetUserDataResult result){
        Debug.Log("Recived player data :D");
        if (result.Data != null && result.Data.ContainsKey("KOFI")){
            if (result.Data["KOFI"].Value.ToString() == "1"){
                Debug.Log("Player is Kofi supporter");
                kofiColor.SetActive(true);
            }
        }
        else {
            Debug.Log("No data :(");
        }
    }

    void OnTitleDataReceived(GetTitleDataResult result){
        if (result.Data == null){
            Debug.Log("No public data (MOTD or Art)");
            return;
        }
        art1 = result.Data["Art1"];
        art2 = result.Data["Art2"];
        art3 = result.Data["Art3"];

        StartCoroutine(getArtViaLink(artRawImages[0], art1));
        StartCoroutine(getArtViaLink(artRawImages[1], art2));
        StartCoroutine(getArtViaLink(artRawImages[2], art3));
    }

    // art stuff :D
    IEnumerator getArtViaLink(RawImage rawImage, string url){
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            Texture image = ((DownloadHandlerTexture)request.downloadHandler).texture;
            rawImage.texture = image;
        }
    }

    IEnumerator musicPitchDown(){
        yield return new WaitForSeconds(10.5f);
        int i = 0;
        if (!intro.introSkipped){
            while (i != 100) {
                intro.introMusic.pitch = intro.introMusic.pitch - 0.01f;
                ++i;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

}
