using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string sceneName;

    public void switchScene(){
        SceneManager.LoadScene(sceneName);
    }

    public void switchSceneDelayed(float seconds){
        StartCoroutine(delayed(seconds));
    }

    private IEnumerator delayed(float delayedfor){
        yield return new WaitForSeconds(delayedfor);
        SceneManager.LoadScene(sceneName);
    }
}
