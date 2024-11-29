using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Pixelation Settings")]
    public Material pixelFSS;
    public float pixelyness;

    public void changePixelyness(float newPixelyness){
        pixelyness = newPixelyness;
        PlayerPrefs.SetFloat("PixelDensity", newPixelyness);
        pixelFSS.SetFloat("PS", pixelyness + 0.5f);
    }
}
