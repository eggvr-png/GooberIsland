using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicCS : MonoBehaviour
{
    public void buttonPress(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
