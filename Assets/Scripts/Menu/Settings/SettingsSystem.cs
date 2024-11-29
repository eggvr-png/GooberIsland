using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSystem : MonoBehaviour
{
    [Header("Pixelation Settings")]
    public Material pixelFSS;
    [SerializeField] private float pixelyness;
    public Slider pixelSlider;
    public TMP_InputField pixelInput;

    // hey look at me not using a update function for this. this means im not a dumbo!!!!
    void Start(){
        pixelSlider.onValueChanged.AddListener(delegate {cpd();});
        loadSettings();
    }

    // pixelation settings
    public void changePixelyness(float newPixelyness){
        pixelyness = newPixelyness;
        PlayerPrefs.SetFloat("PixelDensity", newPixelyness);
        pixelFSS.SetFloat("_PS", pixelyness + 0.5f);
    }

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
    }

    // settings loader
    private void loadSettings(){
        // loads pixel density
        float loadedPixelDensity = PlayerPrefs.GetFloat("PixelDensity");
        pixelSlider.value = loadedPixelDensity;
        pixelInput.text = loadedPixelDensity.ToString();
    }
}
