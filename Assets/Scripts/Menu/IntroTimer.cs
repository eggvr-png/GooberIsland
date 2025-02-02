using System.Collections;
using UnityEngine;
public class IntroTimer : MonoBehaviour
{
    public float introTime;
    public GameObject intro;
    public GameObject menu;
    public GameObject maxPlayers;
    public GameObject allStartingScreens;

    public AudioLowPassFilter filter;
    public AudioReverbFilter filter2;
    public AudioSource introMusic;

    private bool introSkipped = false;

    private IEnumerator Intro()
    {
        float timer = 0f;

        while (timer < introTime && !introSkipped)
        {
            timer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                introSkipped = true;
            }

            yield return null;
        }
        if (introSkipped)
        {
            intro.SetActive(false);
            menu.SetActive(true);
            Destroy(filter);
            Destroy(filter2);

            if (introMusic != null)
            {
                introMusic.time = introTime; 
            }
        }
        if (timer >= introTime){
            intro.SetActive(false);
            menu.SetActive(true);
            Destroy(filter);
            Destroy(filter2);
        }
    }

    public void SkipAll(){
        introMusic.gameObject.SetActive(true);
        Destroy(filter);
        Destroy(filter2);
        introMusic.time = introTime;
        allStartingScreens.SetActive(false);
        menu.SetActive(true);
    }

    void Start(){
        if (PlayerPrefs.GetInt("RTMFG") == 1){
            PlayerPrefs.SetInt("RTMFG", 0);
            SkipAll();
        }
        if (PlayerPrefs.GetInt("RTMFMP") == 1){
            PlayerPrefs.SetInt("RTMFMP", 0);
            SkipAll();
            maxPlayers.SetActive(true);
        }
    }

    public void startIntro() 
    {
        StartCoroutine(Intro());
    }
}