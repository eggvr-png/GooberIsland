using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarnToPriv : MonoBehaviour
{
    public GameObject privacyPolicy;
    public GameObject warning;

    public GameObject music;
    public IntroTimer intro;
    public GameObject introGameObj;

    public PlayfabManager pf;

    public IEnumerator fadeToPriv(){
        yield return new WaitForSeconds(3);
        int acceptedPriv = PlayerPrefs.GetInt("priv");
        if (acceptedPriv == 0){
            warning.gameObject.SetActive(false);
            privacyPolicy.SetActive(true);
        }
        else{
            warning.gameObject.SetActive(false);
            introGameObj.SetActive(true);
            pf.Login();
            intro.startIntro();
            music.SetActive(true);
        }
    }

    void Start(){
        StartCoroutine(fadeToPriv());
    }
}
