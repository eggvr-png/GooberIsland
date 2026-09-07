using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

public class GoToCredits : MonoBehaviour
{
    public GameObject fadeToBlack;
    public AudioSource menuMusic;

    public void onPress()
    {
        fadeToBlack.SetActive(true);
        StartCoroutine(fade());
    }

    public IEnumerator fade()
    {
        int i = 0;
        while (i != 100)
        {
            menuMusic.volume = menuMusic.volume - 0.01f;
            i++;
            yield return new WaitForSeconds(0.01f);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
}
