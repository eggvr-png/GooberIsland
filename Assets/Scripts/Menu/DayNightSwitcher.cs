using System;
using System.Collections;
using UnityEngine;

public class DayNightSwitcher : MonoBehaviour
{
    public float nightColor;
    public Material nightSky;
    public Light DirectionalLight;
    public AudioSource music;

    public GameObject water;
    public Material waterMat;

    public StartingScreens ss;

    bool night;

    void Update()
    {
        DateTime currentTime = DateTime.Now;
        int currentHour = currentTime.Hour;

        if (currentHour >= 19 && !night && ss.screenOn == StartingScreens.screen.None)
        {
            DirectionalLight.colorTemperature = nightColor;
            RenderSettings.skybox = nightSky;
            water.GetComponent<Renderer>().material = waterMat;
            StartCoroutine(changePitch());
            night = true;
        }
        else
        {
            return;
        }

        Debug.Log(currentHour);
    }

    IEnumerator changePitch()
    {
        int i = 0;
        while (i != 20)
        {
            music.pitch = music.pitch - 0.01f;
            ++i;
            yield return new WaitForSeconds(0.01f);
        }
    }
}