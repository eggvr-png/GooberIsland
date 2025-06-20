using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markiplier : MonoBehaviour
{
    public GameObject doNotPressButton;
    public GameObject UI;
    public AudioSource music;

    void Start()
    {
        int chance = Random.Range(1, 10);
        if (chance == 1)
        {
            doNotPressButton.SetActive(true);
        }
    }

    public void button()
    {
        UI.SetActive(true);
        music.enabled = false;

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(18);
        Application.Quit();
    }
}
