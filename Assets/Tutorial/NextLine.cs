using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextLine : MonoBehaviour
{
    public List<string> lines;
    public List<GameObject> lineSfx;

    public TextMeshProUGUI text;
    public GameObject icon;

    public int currentLine;

    bool debounce;

    public PlayerMovement movement;

    public void onEPressed(){
        if (this.gameObject.activeSelf){
            if (!debounce){
                if (Input.GetKey(KeyCode.E)){
                    if (currentLine == 3){
                        movement.moveSpeed = 1000;
                        text.text = lines[currentLine + 1];
                        icon.SetActive(false);
                        lineSfx[currentLine].SetActive(false);
                        lineSfx[currentLine + 1].SetActive(true);
                        currentLine = currentLine + 1;
                        debounce = true;
                        StartCoroutine(debounceFor5());
                    }
                    else {
                        text.text = lines[currentLine + 1];
                        icon.SetActive(false);
                        lineSfx[currentLine].SetActive(false);
                        lineSfx[currentLine + 1].SetActive(true);
                        currentLine = currentLine + 1;
                        debounce = true;
                        StartCoroutine(debounceCo());
                    }
                }
            }
        }
    }
    
    void Update(){
        onEPressed();
    }

    private IEnumerator debounceCo(){
        yield return new WaitForSeconds(2.5f);
        debounce = false;
        icon.SetActive(true);
    }

    private IEnumerator debounceFor5(){
        yield return new WaitForSeconds(8f);
        debounce = false;
        icon.SetActive(true);
    }
}
