using UnityEngine;

public class IntroTimer : MonoBehaviour
{
    [Header("Refrences")]
    public GameObject warning;
    public GameObject priv;
    public GameObject intro;
    [Space]
    public GameObject menu;
    [Space]
    public AudioSource introMusic;
    public AudioSource menuMusic;

    bool onWarning = true;
    bool onPriv;

    void Update()
    {
        if (Input.anyKey && onWarning){
            int isPrivDone = PlayerPrefs.GetInt("priv");
            if (isPrivDone == 1){
                warning.SetActive(false);
                intro.SetActive(true);
                introMusic.Play();
            }
        }
    }
}