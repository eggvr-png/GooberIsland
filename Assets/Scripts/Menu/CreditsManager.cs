using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CreditsManager : MonoBehaviour
{
    public GameObject fade2;
    public AudioSource vid;

    bool pressedSpace;

    private void Awake()
    {
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    public void Update()
    {
        if (!pressedSpace)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressedSpace = true;
                fade2.SetActive(true);
                StartCoroutine(fade());
            }
        }
    }

    private IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSeconds(95f);

        if (pressedSpace)
        {
            yield break;
        }

        PlayerPrefs.SetInt("cameFromCredits", 1);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public IEnumerator fade()
    {
        int i = 0;
        while (i != 100)
        {
            vid.volume = vid.volume - 0.01f;
            i++;
            yield return new WaitForSeconds(0.01f);
        }
        PlayerPrefs.SetInt("cameFromCredits", 1);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
