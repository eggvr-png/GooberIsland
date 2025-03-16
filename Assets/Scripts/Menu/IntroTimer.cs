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
    public Animator introAnimator;
    public Animator menuAnimator;
    [Space]
    public PlayfabManager pfManager;
    public DiscordManager disManager;

    bool onWarning = true;
    bool onPriv = false;

    bool fade = false;

    void Update()
    {
        if (Input.anyKey && onWarning){
            int isPrivDone = PlayerPrefs.GetInt("test");
            if (isPrivDone == 1){
                warning.SetActive(false);
                intro.SetActive(true);
                pfManager.Login();
                disManager.InMenu();
                onWarning = false;
                onPriv = false;
            }
            else {
                warning.SetActive(false);
                priv.SetActive(true);
                onWarning = false;
                onPriv = true;
            }
        }

        AnimatorStateInfo stateInfo = introAnimator.GetCurrentAnimatorStateInfo(0);
    
        if (stateInfo.IsName("intro") && stateInfo.normalizedTime >= 1f) {
            intro.SetActive(false);
            menu.SetActive(true);
            if (!fade){
                menuAnimator.Play("FadeOut", 0, 0);
                fade = true;
                this.enabled = false;
            }
        }
    }

    public void goToIntro(){
        priv.SetActive(false);
        introMusic.Play();
        pfManager.Login();
        disManager.InMenu();
        intro.SetActive(true);
        onPriv = false;
    }
}