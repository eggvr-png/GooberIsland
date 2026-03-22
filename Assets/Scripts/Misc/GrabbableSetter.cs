using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Collections;
//this will store all the data between scenes so that we dont use playerprefs cuz thats weird or something idk
public class GrabbableSetter : MonoBehaviour
{
    public static GrabbableSetter Instance { get; private set; }

    public Transform grabPoint;

    void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
}
}