using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.XR;
using Photon.Voice.Unity.Demos;
using UnityEngine.XR.Management;
using UnityEditor;

public class SettingsSystem : MonoBehaviour
{
    public bool pausemenu;
    [Header("Pixelation Settings")]
    [SerializeField] private float pixelyness;
    [Space]
    public Material pixelFSS;
    public Slider pixelSlider;
    public TMP_InputField pixelInput;
    [Header("Graphics Settings")]
    [SerializeField] public int qualitySetting;
    public TMP_Dropdown qualityChooser;
    [Space]
    public TMP_Dropdown resChooser;
    // janky alert!1!!
    public TextMeshProUGUI resText;
    public Resolution[] resolutions;
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
    [Header("VR Settings")]
    public Toggle vrToggle;
    [Header("Joke Settings")]
    public Toggle hypercam2Toggle;

    // hey look at me not using a update function for this. this means im not a dumbo!!!!
    void Start(){
        pixelSlider.onValueChanged.AddListener(delegate {cpd();});
        qualityChooser.onValueChanged.AddListener(delegate {changeQuality(qualityChooser.value);});
        vsyncToggle.onValueChanged.AddListener(delegate {changeVSync(vsyncToggle.isOn);});
        fullscreenToggle.onValueChanged.AddListener(delegate {changeFullscreen(fullscreenToggle.isOn);});
        mvolSlider.onValueChanged.AddListener(delegate {changeMVolume(mvolSlider.value);});
        svolSlider.onValueChanged.AddListener(delegate {changeSVolume(svolSlider.value);});
        resChooser.onValueChanged.AddListener(delegate {changeRes(resChooser.value);});
        hypercam2Toggle.onValueChanged.AddListener(delegate { hypercamEnable(hypercam2Toggle.isOn); });
        // vrToggle.onValueChanged.AddListener(delegate {enableVR(vrToggle.isOn);});
        // here we get all compatable resolutions
        resolutions = Screen.resolutions;
        var resolutionList =  new List<TMP_Dropdown.OptionData>();
        foreach (Resolution resolution in resolutions){
            string resString = resolution.width + "x" + resolution.height;
            resolutionList.Add(new TMP_Dropdown.OptionData(resString));
        }
        // clear dropdown just in case i did a dummy
        resChooser.ClearOptions();
        resChooser.AddOptions(resolutionList);
        if (!pausemenu)
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
    // i cant tell if put this here or not, but idfk lol
    // nvm its for the input text by the slider i think
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

    public void changeRes(int res){
        Screen.SetResolution(resolutions[res].width, resolutions[res].height, fullscreenToggle.isOn);
        PlayerPrefs.SetInt("width", resolutions[res].width);
        PlayerPrefs.SetInt("height", resolutions[res].height);
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
        textValue = textValue * 10;
        mvolInput.text = textValue.ToString();
    }

    public void changeSVolume(float sliderVal){
        saudioMixer.SetFloat("SFX", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("sfxVol", sliderVal);
        float textValue = sliderVal * 10;
        textValue = Mathf.Round(textValue);
        textValue = textValue * 10;
        svolInput.text = textValue.ToString();
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

    // vr
    void enableVR(bool toggle){
        if (toggle){
            Debug.Log("startin vr, gimeme a few :D");
            var xrManager = XRGeneralSettings.Instance.Manager;

            xrManager.InitializeLoader();
            xrManager.StartSubsystems();
        }
        else {
            Debug.Log("stopping vr. hold on a sec :/");
            var xrManager = XRGeneralSettings.Instance.Manager;

            xrManager.StopSubsystems();
            xrManager.DeinitializeLoader();
        }
    }

    // joke settings
    void hypercamEnable(bool toggle)
    {
        if (toggle)
            PlayerPrefs.SetInt("hypercam", 1);
        else
            PlayerPrefs.SetInt("hypercam", 0);
    }

    // settings loader
    public void loadSettings(){
        if (Application.platform == RuntimePlatform.Android)
        {
            vrToggle.gameObject.SetActive(false);
        }
        // loads pixel density
        if (!XRSettings.isDeviceActive){
            float loadedPixelDensity = PlayerPrefs.GetFloat("PixelDensity");
            if (loadedPixelDensity == 0f){
                pixelSlider.value = 200f;
                pixelInput.text = "200";
            }
            else {
                pixelSlider.value = loadedPixelDensity;
                pixelInput.text = loadedPixelDensity.ToString();
            }
        {
        // loads quality
        int qSetting = PlayerPrefs.GetInt("qSetting");
        if (qSetting == 0){
            qualityChooser.value = 2;
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
            changeMVolume(savedVol);
        }
        float savedsVol = PlayerPrefs.GetFloat("sfxVol");
        if (savedsVol == 0){

        }
        else {
            svolSlider.value = savedsVol;
            changeSVolume(savedsVol);
        }
        // loads resolution
        if (PlayerPrefs.GetInt("width") != 0 && PlayerPrefs.GetInt("height") == 0){
            Screen.SetResolution(PlayerPrefs.GetInt("width"), PlayerPrefs.GetInt("height"), fullscreenToggle.isOn);
        }
        // loads joke settings
        if (PlayerPrefs.GetInt("hypercam") == 1)
        {
            hypercam2Toggle.isOn = true;
        }
    }
        }
}}
