using UnityEngine;
//this will store all the data between scenes so that we dont use playerprefs cuz thats weird or something idk
public class InterSceneDataKeeper : MonoBehaviour
{
    public static InterSceneDataKeeper Instance { get; private set; }

    public string roomCode; //for example you can get this variable in any script by just writing InterSceneDataKeeper.Instance.roomCode

    public string playerName;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}