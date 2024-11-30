using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSystem : MonoBehaviour
{
    [Header("Pixelation Settings")]
    [SerializeField] private float pixelyness;
    [Space]
    public Material pixelFSS;
    public Slider pixelSlider;
    public TMP_InputField pixelInput;
    [Header("Control Settings")]
    [SerializeField] private int mouseSens;
    public Slider mSSlider;
    public TMP_InputField mSInput;
    [Space]
    public KeyCode crouchKey;
    public KeyCode sprintKey;

    // hey look at me not using a update function for this. this means im not a dumbo!!!!
    void Start(){
        pixelSlider.onValueChanged.AddListener(delegate {cpd();});
        loadSettings();
    }

    // pixelation settings
    public void changePixelyness(float newPixelyness){
        pixelyness = newPixelyness;
        PlayerPrefs.SetFloat("PixelDensity", newPixelyness);
        pixelFSS.SetFloat("_PS", (512 - pixelyness)+ 0.5f);
    }

    private void cpd(){
        changePixelyness(pixelSlider.value);
        pixelInput.text = pixelyness.ToString();
    }

    public void changePixelynessWithInput(string input){
        int newPixelyness;
        int.TryParse(input, out newPixelyness);
        pixelSlider.value = newPixelyness;
        if (newPixelyness > 512){
            pixelInput.text = "512";
        }
    }

    // controls

    // settings loader
    private void loadSettings(){
        // loads pixel density
        float loadedPixelDensity = PlayerPrefs.GetFloat("PixelDensity",256);
        if (loadedPixelDensity == 0f){
            pixelSlider.value = 200f;
            pixelInput.text = "200";
        }
        else {
            pixelSlider.value = loadedPixelDensity;
            pixelInput.text = loadedPixelDensity.ToString();
        }
    }
}
