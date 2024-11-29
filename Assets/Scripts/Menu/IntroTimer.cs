using System.Collections;
using UnityEngine;

public class IntroTimer : MonoBehaviour
{
    public float introTime;
    public GameObject intro;
    public GameObject menu;

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

        if (introSkipped || timer >= introTime)
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
    }

    public void startIntro() 
    {
        StartCoroutine(Intro());
    }
}