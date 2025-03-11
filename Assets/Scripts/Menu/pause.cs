using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Fragsurf.Movement;
using Photon.Realtime;
using Unity.VisualScripting;

public class pause : MonoBehaviour
{
    public GameObject pauseAudioListener;
    public GameObject pauseMenu;
    public AudioListener mainAudioListener;
    public RoomManager pm;

    public Camera main;

    public KeyCode pauseKey;

    public static bool paused; // from now you can see if you are paused by just typing pause.paused and its a bool (i didnt name them)

    bool debounce;

    SurfCharacter playermovement;
    PlayerAiming pa;

    public void LockMouse(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GetCamAndOther(Camera cam, SurfCharacter pm, AudioListener playerListener, PlayerAiming pAim){
        main = cam;
        playermovement = pm;
        mainAudioListener = playerListener;
        pa = pAim;
    }

    void Update(){
        if (main == null){
            main = Camera.main;
        }
        if (Input.GetKeyDown(pauseKey)){
            if (pm.status == RoomManager.connectionStatus.InLobby){
                if (!paused){
                    if (!debounce){
                        pauseMenu.SetActive(true);
                        mainAudioListener.enabled = false;
                        pauseAudioListener.SetActive(true);
                        main.clearFlags = CameraClearFlags.Nothing; // Stop clearing old frames
                        paused = true;
                        playermovement.enabled = false;
                        pa.enabled = false;
                        UnlockMouse();
                        StartCoroutine(debouncer());
                    }
                }
                if (paused){
                    if (!debounce){
                        pauseMenu.SetActive(false);
                        mainAudioListener.enabled = true;
                        pauseAudioListener.SetActive(false);
                        main.clearFlags = CameraClearFlags.Skybox; // Resume clearing old frames
                        paused = false;
                        playermovement.enabled = true;
                        pa.enabled = true;
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
        mainAudioListener.enabled = true;;
        pauseAudioListener.SetActive(false);
        main.clearFlags = CameraClearFlags.Skybox;
        playermovement.enabled = true;
                        pa.enabled = true; // Resume clearing old frames
        paused = false;
        LockMouse();
    }
}
