using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Photon.Pun;
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
    [Space]
    public AudioListener defualtls;
    public AudioListener listener;
    public AudioSource music;

    public bool paused = false; //man i really dunno what this var is :P

    public void unpause()
    {
        playerMovement.enabled = true;
        defualtls.enabled = true;
        listener.enabled = false;

        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        music.pitch = 1;
        StartCoroutine(Pitch(false));
    }

    public void pause()
    {
        StopCoroutine(Pitch(false));
        music.pitch = 1;
        playerMovement.enabled = false;
        defualtls.enabled = false;
        listener.enabled = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        music.pitch = 0;
        StartCoroutine(Pitch(true));
    }

    public void leaveLobby()
    {
        PlayerPrefs.SetInt("autoSkip", 1);
        PhotonNetwork.Disconnect();
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

    IEnumerator Pitch(bool up)
    {
        if (!up)
        {
            int i = 0;
            //staticSource.Play();
            while (i != 100)
            {
                ++i;
                music.pitch = music.pitch - 0.01f;
                yield return new WaitForSeconds(0.001f);
            }
        }
        else
        {
            int i = 0;
            //staticSource.Stop();
            while (i != 100)
            {
                ++i;
                music.pitch = music.pitch + 0.01f;
                yield return new WaitForSeconds(0.001f);
            }
        }
    }
}
