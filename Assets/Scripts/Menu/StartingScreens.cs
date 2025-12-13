using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartingScreens : MonoBehaviour
{
    public enum screen
    {
        Epilepsy,
        Privacy,
        Intro,
        None
    }

    [Header("Epilepsy Warning")]
    public GameObject warning;
    [Header("Privacy Policy")]
    public GameObject privacy;
    public Scrollbar scrollbar;
    public Toggle toggle;
    public GameObject button;
    [Header("Intro")]
    public GameObject intro;
    public AudioSource music;
    [Space]
    public Animator introAnimator;
    [Header("Menu")]
    public GameObject menu;
    public Animator menuFade;
    [Header("What are screens on")]
    public screen screenOn = screen.Epilepsy;

    public PlayfabManager pm;

    public bool introSkipped;

    bool fade = false;

    bool deboucne2Int = false;

    int acceptedPrivacy;

    public void Start()
    {
        acceptedPrivacy = PlayerPrefs.GetInt("privAcceptNew");

        int autoSkip = PlayerPrefs.GetInt("autoSkip");

        if (autoSkip == 1)
        {
            music.Play();
            warning.SetActive(false);
            menu.SetActive(true);
            screenOn = screen.None;
            music.time = 10.5f;
            if (!fade)
            {
                menuFade.Play("FadeOut", 0, 0);
                fade = true;
                this.enabled = false;
            }

            PlayerPrefs.DeleteKey("autoSkip");
        }
    }

    private void Update()
    {
        if (screenOn == screen.Epilepsy)
        {
            if (Input.anyKeyDown)
            {
                screenOn = screen.Privacy;
                if (acceptedPrivacy == 0)
                {
                    warning.SetActive(false);
                    privacy.SetActive(true);
                }
                else
                {
                    GoToIntro();
                }
            }
        }

        if (screenOn == screen.Privacy)
        {
            float value = scrollbar.value;
            if (value <= 0.02f)
            {
                toggle.interactable = true;
            }
            else
            {
                toggle.interactable = false;
            }

            if (toggle.isOn)
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }
        }

        if (screenOn == screen.Intro)
        {
            AnimatorStateInfo stateInfo = introAnimator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.normalizedTime >= 1f)
            {
                intro.SetActive(false);
                menu.SetActive(true);
                screenOn = screen.None;
                if (!fade)
                {
                    menuFade.Play("FadeOut", 0, 0);
                    fade = true;
                    this.enabled = false;
                }
            }
        }

        if (screenOn == screen.Intro)
        {
            if (Input.GetKeyDown(KeyCode.Space) && deboucne2Int != true)
            {
                introSkipped = true;
                intro.SetActive(false);
                menu.SetActive(true);
                screenOn = screen.None;
                music.time = 10.5f;
                if (!fade)
                {
                    menuFade.Play("FadeOut", 0, 0);
                    fade = true;
                    this.enabled = false;
                }
            }
        }
    }

    public void GoToIntro()
    {
        screenOn = screen.Intro;
        pm.Login();
        warning.SetActive(false);
        privacy.SetActive(false);
        intro.SetActive(true);
        music.Play();
        deboucne2Int = true;
        StartCoroutine(deboucne()); // debounce of doom and despair
    }

    public void GoToIntroPriv()
    {
        PlayerPrefs.SetInt("privAcceptNew", 1);
        pm.Login();
        screenOn = screen.Intro;
        warning.SetActive(false);
        privacy.SetActive(false);
        intro.SetActive(true);
        music.Play();
        
    }

    public IEnumerator deboucne()
    {
        yield return new WaitForSeconds(1);
        deboucne2Int = false;
    }
}
