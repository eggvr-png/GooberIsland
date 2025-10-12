using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public string sceneToLoad;

    public void SwitchScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    public IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("loaded scene to: " + progress.ToString());
            yield return null;
        }
    }
}
