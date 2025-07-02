using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Unity.VisualScripting;

public class IntroTimer : MonoBehaviour
{
    [Header("Refrences")]
    public GameObject warning;
    public GameObject priv;
    public GameObject intro;
    [Space]
    public GameObject menu;
    public GameObject wifidownMenu;
    [Space]
    public AudioSource introMusic;
    public Animator introAnimator;
    public Animator menuAnimator;
    [Space]
    public PlayfabManager pfManager;
    //public DiscordManager discord;

    public float introLength;

    public bool onWarning = true;
    public bool onPriv = false;
    public bool onIntro = false;

    bool ableToSkip = false;

    public bool introSkipped = false;

    bool fade = false;

    int isPrivDone;

    private void Start()
    {
        RuntimePlatform currentPlatform = Application.platform;
        
        if (currentPlatform == RuntimePlatform.WebGLPlayer)
        {
            isPrivDone = 1;
        }
        else
        {
            isPrivDone = PlayerPrefs.GetInt("privacyPolicyAccept");
        }
    }

    void Update()
    {
        if (Input.anyKey && !Input.GetKey(KeyCode.Z) && onWarning){
            if (isPrivDone == 1){
                PlayerPrefs.SetInt("offline", 0);
                warning.SetActive(false);
                StartCoroutine(checkIfConnected2Wifi());
                StartCoroutine(skipDebounce());
                intro.SetActive(true);
                pfManager.Login();
                onWarning = false;
                onPriv = false;
                onIntro = true;

                //if (discord.discordRunning){
                    //discord.InMenu();
                //}
            }
            else {
                PlayerPrefs.SetInt("offline", 0);
                warning.SetActive(false);
                priv.SetActive(true);
                onWarning = false;
                onPriv = true;
                onIntro = true;
            }
        }

        AnimatorStateInfo stateInfo = introAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("intro") && stateInfo.normalizedTime >= 1f)
        {
            intro.SetActive(false);
            menu.SetActive(true);
            onIntro = false;
            if (!fade)
            {
                menuAnimator.Play("FadeOut", 0, 0);
                fade = true;
                this.enabled = false;
            }
        }

        skipIntro();
    }

    public void goToIntro(){
        PlayerPrefs.SetInt("offline", 0);
        StartCoroutine(checkIfConnected2Wifi());
        StartCoroutine(skipDebounce());
        priv.SetActive(false);
        introMusic.Play();
        pfManager.Login();
        intro.SetActive(true);
        onPriv = false;

        //if (discord.discordRunning){
            //discord.InMenu();
        //}
    }

    public void skipIntro(){
        if (!onWarning && !onPriv && onIntro && ableToSkip){
            if (Input.GetKey(KeyCode.Space)) {
                introMusic.time = introLength;
                intro.SetActive(false);
                menu.SetActive(true);
                onIntro = false;
                introSkipped = true;

                if (!fade){
                    menuAnimator.Play("FadeOut", 0, 0);
                    fade = true;
                    this.enabled = false;
                }

                if (noWifi) {
                    StopCoroutine(musicPitchDown());
                    StartCoroutine(musicPitchQuick());
                }
            }
        }

        if (PlayerPrefs.GetInt("RTMFG") == 1)
        {
            PlayerPrefs.SetInt("RTMFG", 0);
            PlayerPrefs.SetInt("offline", 0);
            pfManager.Login();
            introMusic.Play();
            introMusic.time = introLength;
            warning.SetActive(false);
            intro.SetActive(false);
            menu.SetActive(true);
            onIntro = false;
            introSkipped = true;

            if (!fade)
            {
                menuAnimator.Play("FadeOut", 0, 0);
                fade = true;
                this.enabled = false;
            }

            if (noWifi)
            {
                StopCoroutine(musicPitchDown());
                StartCoroutine(musicPitchQuick());
            }
        }
    }

    bool noWifi;

    IEnumerator checkIfConnected2Wifi() {
        RuntimePlatform currentPlatform = Application.platform;
        
        if (currentPlatform != RuntimePlatform.WebGLPlayer)
        {
            using (UnityWebRequest www = UnityWebRequest.Head("https://www.google.com"))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("player is connected to wifi :D");
                }
                else
                {
                    Debug.Log("player isn't connected to wifi D:");
                    pfManager.enabled = false;
                    noWifi = true;
                    wifidownMenu.SetActive(true);
                    StartCoroutine(musicPitchDown());
                }
            }
        }
    }

    IEnumerator musicPitchDown(){
        yield return new WaitForSeconds(introLength);
        int i = 0;
        if (!introSkipped){
            while (i != 100) {
                introMusic.pitch = introMusic.pitch - 0.01f;
                ++i;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    IEnumerator musicPitchQuick(){
        int i = 0;
        while (i != 100) {
            introMusic.pitch = introMusic.pitch - 0.01f;
            ++i;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator musicPitchUp(){
        int i = 0;
        while (i != 100) {
            introMusic.pitch = introMusic.pitch + 0.01f;
            ++i;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void pitchBackUp(){
        StartCoroutine(musicPitchUp());
    }

    IEnumerator skipDebounce(){
        yield return new WaitForSeconds(1);
        ableToSkip = true;
    }
}