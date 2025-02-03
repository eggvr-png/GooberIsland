using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject pauseAudioListener;
    public GameObject pauseMenu;
    public AudioListener mainAudioListener;
    public RoomManager pm;

    public KeyCode pauseKey;

    bool paused;

    bool debounce;

    public void LockMouse(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update(){
        if (Input.GetKeyDown(pauseKey)){
            if (pm.status == RoomManager.connectionStatus.InLobby){
                if (!paused){
                    if (!debounce){
                        pauseMenu.SetActive(true);
                        mainAudioListener.enabled = false;
                        pm.player.GetComponent<PlayerMovement>().enabled = false;
                        pauseAudioListener.SetActive(true);
                        Camera.main.clearFlags = CameraClearFlags.Nothing; // Stop clearing old frames
                        paused = true;
                        UnlockMouse();
                        StartCoroutine(debouncer());
                    }
                }
                if (paused){
                    if (!debounce){
                        pauseMenu.SetActive(false);
                        mainAudioListener.enabled = true;
                        pm.player.GetComponent<PlayerMovement>().enabled = true;
                        pauseAudioListener.SetActive(false);
                        Camera.main.clearFlags = CameraClearFlags.Skybox; // Resume clearing old frames
                        paused = false;
                        LockMouse();
                        StartCoroutine(debouncer());
                    }
                }
            }
        }
    }

    private IEnumerator debouncer(){
        debounce = true;
        yield return new WaitForSeconds(0.2f);
        debounce = false;
    }

    public void unpauseButton(){
        pauseMenu.SetActive(false);
        mainAudioListener.enabled = true;
        pm.player.GetComponent<PlayerMovement>().enabled = true;
        pauseAudioListener.SetActive(false);
        Camera.main.clearFlags = CameraClearFlags.Skybox; // Resume clearing old frames
        paused = false;
        LockMouse();
    }
}
