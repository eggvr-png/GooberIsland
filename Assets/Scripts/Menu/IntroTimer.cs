using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTimer : MonoBehaviour
{
    public float introTime;
    public GameObject intro;
    public GameObject menu;

    public AudioLowPassFilter filter;
    public AudioReverbFilter filter2;

    private IEnumerator Intro(){
        yield return new WaitForSeconds(introTime);
        intro.SetActive(false);
        menu.SetActive(true);
        Destroy(filter);
        Destroy(filter2);
    }

    public void startIntro() {
        StartCoroutine(Intro());
    }
}
