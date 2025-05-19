using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorHandler : MonoBehaviour
{
    public GameObject sceneSwitcher;
    public GameObject disclaimer;

    public IntroTimer intro;

    private void Update()
    {
        int priv = PlayerPrefs.GetInt("privacyPolicyAccept");
        if (priv != 0 && intro.onWarning)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                sceneSwitcher.SetActive(true);
                disclaimer.SetActive(false);
            }
        }
    }
}
