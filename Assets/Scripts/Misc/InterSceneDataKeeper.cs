using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Collections;
//this will store all the data between scenes so that we dont use playerprefs cuz thats weird or something idk
public class InterSceneDataKeeper : MonoBehaviour
{
    public static InterSceneDataKeeper Instance { get; private set; }

    public string roomCode; //for example you can get this variable in any script by just writing InterSceneDataKeeper.Instance.roomCode

    public string playerName;

    public int cosmeticId;

    public static string errorText;

    [Space]

    public AudioClip currentSong;
    public string currentSongTitle;



    private void Awake()
    {
        if (InterSceneDataKeeper.Instance != null)
        {
            Destroy(this.gameObject);
        }
        
        Application.targetFrameRate = -1;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadlastSong();
        }
        else
        {
            Destroy(Instance);
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadlastSong();
        }

        string lastUser = PlayerPrefs.GetString("playerName");
        if (lastUser != null || lastUser != "")
        {
            playerName = lastUser;
        }
    }


    
    public void loadlastSong()
    {
        string lastTitle = PlayerPrefs.GetString("lastSongTitle");
        if (!string.IsNullOrEmpty(lastTitle))
        {
            string songFolder = Path.Combine(Application.persistentDataPath, "radio_songs", lastTitle);

            // find audio file inside that folder
            if (Directory.Exists(songFolder))
            {
                string coolAudioPath = Directory.GetFiles(songFolder).FirstOrDefault(f => f.EndsWith(".mp3") || f.EndsWith(".wav") || f.EndsWith(".ogg"));

                string notCoolJsonPath = Path.Combine(songFolder, lastTitle + ".json");

                if (File.Exists(coolAudioPath) && File.Exists(notCoolJsonPath))
                {
                    string jsonText = File.ReadAllText(notCoolJsonPath);
                    var info = JsonUtility.FromJson<RadioMusicImporter.SongInfo>(jsonText);

                    StartCoroutine(LoadAudioClipAtStartup(coolAudioPath, info));
                }
            }
        }
    }

    public void SaveCode(string value){
        roomCode = value;
    }

    public void SaveUser(string value){
        playerName = value;
        PlayerPrefs.SetString("playerName", value);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (errorText != null && SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator LoadAudioClipAtStartup(string path, RadioMusicImporter.SongInfo info)
    {
        string url = "file://" + path;
        using (var www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                AudioClip clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);
                currentSong = clip;
                currentSongTitle = info.title;
                
                Debug.Log("loaded last song: " + info.title);
            }
            else
            {
                Debug.LogError("failed to load last song: " + www.error);
            }
        }
    }
}