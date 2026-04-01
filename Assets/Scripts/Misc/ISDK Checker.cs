using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISDKChecker : MonoBehaviour
{
    public InterSceneDataKeeper main;

    public void Start()
    {
        if (InterSceneDataKeeper.Instance != main)
        {
            Destroy(main.gameObject);
        }
    }
}
