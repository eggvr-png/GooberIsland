using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsSystem : MonoBehaviour
{
    [Header("Pixelation Settings")]
    [SerializeField] private float pixelyness;
    [Space]
    public Material pixelFSS;
    public Slider pixelSlider;
    public TMP_InputField pixelInput;
    [Header("Graphics Settings")]
    [SerializeField] public int qualitySetting;
    public TMP_Dropdown qualityChooser;
    [Header("VSync Settings")]
    public Toggle vsyncToggle;
    [Header("Fullscreen Settings")]
    public Toggle fullscreenToggle;
    [Header("Music Volume Settings")]
    public Slider mvolSlider;
    public TMP_InputField mvolInput;
    public AudioMixer maudioMixer;
    [Header("SFX Volume Settings")]
    public Slider svolSlider;
    public TMP_InputField svolInput;
    public AudioMixer saudioMixer;

    // hey look at me not using a update function for this. this means im not a dumbo!!!!
    void Start(){
        pixelSlider.onValueChanged.AddListener(delegate {cpd();});
        qualityChooser.onValueChanged.AddListener(delegate {changeQuality(qualityChooser.value);});
        vsyncToggle.onValueChanged.AddListener(delegate {changeVSync(vsyncToggle.isOn);});
        fullscreenToggle.onValueChanged.AddListener(delegate {changeFullscreen(fullscreenToggle.isOn);});
        mvolSlider.onValueChanged.AddListener(delegate {changeMVolume(mvolSlider.value);});
        svolSlider.onValueChanged.AddListener(delegate {changeSVolume(svolSlider.value);});
        loadSettings();
    }

    // pixelation settings
    public void changePixelyness(float newPixelyness){
        pixelyness = newPixelyness;
        PlayerPrefs.SetFloat("PixelDensity", newPixelyness);
        // 0.5 is added so u dont get black lines at the top and sides of your screen.
        pixelFSS.SetFloat("_PS", (512 - pixelyness)+ 0.5f);
    }

    // tbh i dunno why this is here lol
    private void cpd(){
        changePixelyness(pixelSlider.value);
        pixelInput.text = pixelyness.ToString();
    }

    public void changePixelynessWithInput(string input){
        int newPixelyness;
        int.TryParse(input, out newPixelyness);
        pixelSlider.value = newPixelyness;
        if (newPixelyness > 500){
            pixelInput.text = "500";
        }
        if (newPixelyness < 50){
            pixelInput.text = "50";
        }
    }

    // quality
    public void changeQuality(int qSetting){
        PlayerPrefs.SetInt("qSetting", qSetting + 1);
        QualitySettings.SetQualityLevel(qSetting);
        qualitySetting = qSetting;
    }

    // vsync
    public void changeVSync(bool changeTo){
        if (changeTo){
            QualitySettings.vSyncCount = 1;
            Debug.Log("VSync On");
            PlayerPrefs.SetInt("vsyncSetting", 1);
        }
        else {
            QualitySettings.vSyncCount = 0;
            Debug.Log("VSync Off");
            PlayerPrefs.SetInt("vsyncSetting", 0);
        }
    }

    // fullscreen
    public void changeFullscreen(bool changeTo){
        Screen.fullScreen = changeTo;
        Debug.Log("Fullscreen " + changeTo.ToString());
        if (changeTo){
            PlayerPrefs.SetInt("fullsSettings", 0);
        }
        else {
            PlayerPrefs.SetInt("fullsSettings", 1);
        }
    }


    // volume
    public void changeMVolume(float sliderVal){
        maudioMixer.SetFloat("Music", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("musicVol", sliderVal);
        float textValue = sliderVal * 10;
        textValue = Mathf.Round(textValue);
        textValue = textValue / 10;
        mvolInput.text = textValue.ToString();
    }

    public void changeSVolume(float sliderVal){
        saudioMixer.SetFloat("SFX", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("sfxVol", sliderVal);
        float textValue = sliderVal * 10;
        textValue = Mathf.Round(textValue);
        textValue = textValue / 10;
        svolInput.text = sliderVal.ToString();
    }

    public void changeMVolWithInput(string input){
        int newPixelyness;
        int.TryParse(input, out newPixelyness);
        if (newPixelyness > 0){
            mvolSlider.value = 0.001f;
        }
        else if (newPixelyness < 1){
            mvolInput.text = "1";
            mvolSlider.value = 1;
        }
        else {
            mvolSlider.value = newPixelyness;
        }
    }

    public void changeSVolWithInput(string input){
        int newPixelyness;
        int.TryParse(input, out newPixelyness);
        if (newPixelyness > 0){
            svolSlider.value = 0.001f;
        }
        else if (newPixelyness < 1){
            svolInput.text = "1";
            svolSlider.value = 1;
        }
        else {
            svolSlider.value = newPixelyness;
        }
    }


    // settings loader
    private void loadSettings(){
        // loads pixel density
        float loadedPixelDensity = PlayerPrefs.GetFloat("PixelDensity");
        if (loadedPixelDensity == 0f){
            pixelSlider.value = 200f;
            pixelInput.text = "200";
        }
        else {
            pixelSlider.value = loadedPixelDensity;
            pixelInput.text = loadedPixelDensity.ToString();
        }
        // loads quality
        int qSetting = PlayerPrefs.GetInt("qSetting");
        if (qSetting == 0){
            qualityChooser.value = 1;
        }
        else {
            qualityChooser.value = qSetting - 1;
            QualitySettings.SetQualityLevel(qSetting - 1);
            qualitySetting = qSetting - 1;
        }
        // loads vsync
        int vsync = PlayerPrefs.GetInt("vsyncSetting");
        if (vsync == 0){
            vsyncToggle.isOn = false;
        }
        else {
            vsyncToggle.isOn = true;
        }
        // loads fullscreen
        int fullscreen = PlayerPrefs.GetInt("fullsSettings");
        if (fullscreen == 0){
            fullscreenToggle.isOn = true;
        }
        else {
            fullscreenToggle.isOn = false;
        }
        // loads volume
        float savedVol = PlayerPrefs.GetFloat("musicVol");
        if (savedVol == 0){

        }
        else {
            mvolSlider.value = savedVol;
        }
        float savedsVol = PlayerPrefs.GetFloat("sfxVol");
        if (savedsVol == 0){

        }
        else {
            svolSlider.value = savedsVol;
        }
    }
}
