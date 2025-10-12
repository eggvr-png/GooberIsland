using UnityEngine;
using System.IO;
using System.Collections;
using System.IO.Compression;
using System.Linq;
using SFB;

public class RadioMusicImporter : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("autoplays music")] public bool enableDebugMode; // added this since i need to test if i can hear it.

    // a cool class.
    [System.Serializable]
    public class SongInfo
    {
        public string title;
        public string filename;
        public string type;
    }

    InterSceneDataKeeper isdk;

    public void Start()
    {
        isdk = this.gameObject.GetComponent<InterSceneDataKeeper>();
    }

    public void importZip(string fileLocation)
    {
        string pathToExtractAt = Path.Combine(Application.persistentDataPath, "radio_songs");
        Directory.CreateDirectory(pathToExtractAt); // creates path 4 the audio files so they dont have 2 be reuploaded.

        string tempFolder = Path.Combine(pathToExtractAt, "temp_unpack");
        if (Directory.Exists(tempFolder))
        {
            Directory.Delete(tempFolder, true);
        }
        Directory.CreateDirectory(tempFolder); // creates path 4 the files when unzipping? unpacking? idfk.

        ZipFile.ExtractToDirectory(fileLocation, tempFolder); // does the thing. you know. the thing.

        // gets the name json, and audio file since thats what it does.... yeah.
        string notCoolJsonPath = Directory.GetFiles(tempFolder, "*.json")[0];
        string coolMusicPath = Directory.GetFiles(tempFolder, "*.*").First(f => f.EndsWith(".mp3") || f.EndsWith(".wav") || f.EndsWith(".ogg")); // idk what this means but yeah.

        // here we load teh json thing since yes.
        string jsonTextStuff = File.ReadAllText(notCoolJsonPath);
        SongInfo song = JsonUtility.FromJson<SongInfo>(jsonTextStuff);

        // saves the .oggs or whatevers 2 a file so they can be used on relaunch!
        string theFinalFolderOfDoom = Path.Combine(pathToExtractAt, song.title);
        Directory.CreateDirectory(theFinalFolderOfDoom);
        File.Copy(notCoolJsonPath, Path.Combine(theFinalFolderOfDoom, Path.GetFileName(notCoolJsonPath)), true);
        File.Copy(coolMusicPath, Path.Combine(theFinalFolderOfDoom, Path.GetFileName(coolMusicPath)), true);

        // deletes the temp folder since fuck the temp folder
        Directory.Delete(tempFolder, true);

        // load the audio because the audio is cool and epic and probally pretty coool.
        StartCoroutine(loadAudio(Path.Combine(theFinalFolderOfDoom, Path.GetFileName(coolMusicPath)), song));
    }

    private IEnumerator loadAudio(string path, SongInfo info)
    {
        string url = "file://" + path;
        using (var www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                AudioClip clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);

                // plays back the music if debug mode is on since yeah.
                AudioSource previewSource = this.gameObject.GetComponent<AudioSource>();
                if (previewSource && enableDebugMode)
                {
                    previewSource.clip = clip;
                    previewSource.Play();
                }

                isdk.currentSong = clip;
                isdk.currentSongTitle = info.title;

                // save last song since it would be convient for the epik gamor
                PlayerPrefs.SetString("lastSongTitle", info.title);
                PlayerPrefs.Save();

                Debug.Log("loaded song: " + info.title);
            }
            else
            {
                Debug.LogError("audio load failed: " + www.error);
            }
        }
    }

    public void importButton()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select zip", "", "zip", false);

        if (paths.Length > 0)
        {       
            string path = paths[0];  // get the first (and only) selected file
            importZip(path);
        }
    }
}