using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchOnButton : MonoBehaviour
{
    public AudioSource audioSource;

    void buttonPressUp()
    {
        StartCoroutine(Pitch(true));
    }

    void buttonPressDown()
    {
        StartCoroutine(Pitch(false));
    }

    IEnumerator Pitch(bool up)
    {
        if (!up)
        {
            int i = 0;
            //staticSource.Play();
            while (i != 100)
            {
                ++i;
                audioSource.pitch = audioSource.pitch - 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            int i = 0;
            //staticSource.Stop();
            while (i != 100)
            {
                ++i;
                audioSource.pitch = audioSource.pitch + 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
