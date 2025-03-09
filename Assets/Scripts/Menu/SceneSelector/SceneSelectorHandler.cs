using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorHandler : MonoBehaviour
{
    [Header("Scene Names")]
    public string raft = "Raft";
    public string testScene = "TestScene";
    [Header("Menus")]
    public GameObject sceneSwitcher;
    public GameObject disclaimer;
    [Space]
    public GameObject mainMenu;
    public GameObject userAndCodeSelect;

    public IntroTimer intro;
    [Header("Refrences")]
    public AudioLowPassFilter alpf;

    bool testSceneYes;

    bool ssEnabled;

    void Update()
    {
        if (!intro.introStarted && PlayerPrefs.GetInt("priv") != 0 && !ssEnabled){
            if (Input.GetKey(KeyCode.Z)){
                intro.enabled = false;
                disclaimer.SetActive(false);
                sceneSwitcher.SetActive(true);
            }
        }
    }

    public void reloadMenu(){
        SceneManager.LoadScene(0);
    }

    public void SwitchScene(){
        if (testSceneYes){
            SceneManager.LoadScene(testScene);
        }
        else {
            SceneManager.LoadScene(raft);
        }
    }

    public void openNameAndCode(bool isTestScene){
        testSceneYes = isTestScene;
        mainMenu.SetActive(false);
        userAndCodeSelect.SetActive(true);
        alpf.enabled = true;
    }

    public void goBack(){
        testSceneYes = false;
        mainMenu.SetActive(true);
        userAndCodeSelect.SetActive(false);
        alpf.enabled = false;
    }
}
