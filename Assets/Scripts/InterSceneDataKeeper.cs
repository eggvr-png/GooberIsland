using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
//this will store all the data between scenes so that we dont use playerprefs cuz thats weird or something idk
public class InterSceneDataKeeper : MonoBehaviour
{
    public static InterSceneDataKeeper Instance { get; private set; }

    public string roomCode; //for example you can get this variable in any script by just writing InterSceneDataKeeper.Instance.roomCode

    public string playerName;

    public static string errorText;
    

    private void Awake()
    {
        Application.targetFrameRate = -1;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
    }

    public void SaveCode(string value){
        roomCode = value;
    }

    public void SaveUser(string value){
        playerName = value;
    }

    void Update()
    {
        if (errorText != null && SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene(0);
        }
    }

}