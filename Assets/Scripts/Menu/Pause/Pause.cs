using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [Header("Connected")]
    public bool isPlayerConnected;
    [Header("Refrences")]
    public PlayerMovement playerMovement; //this gets filled in by the conenction manager
    [Space]
    public GameObject pauseMenu; //the pause menu of doom and dispair
    public GameObject fadeWhiteObj;

    bool paused = false; //man i really dunno what this var is :P

    public void unpause()
    {
        playerMovement.enabled = true;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void pause()
    {
        playerMovement.enabled = false;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void leaveLobby()
    {
        PlayerPrefs.SetInt("autoSkip", 1);
        StartCoroutine(leave());
    }
    
    public IEnumerator leave()
    {
        fadeWhiteObj.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlayerConnected)
        {
            if (!paused)
            {
                paused = true;
                pause();
            }
            else
            {
                paused = false;
                unpause();
            }
        }    
    }
}
