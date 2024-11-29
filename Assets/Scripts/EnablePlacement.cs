using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlacement : MonoBehaviour
{
    public PreviewItem itemscript;
    public GameObject preview;

    public bool placementOn;

    private bool debounce;

    public void Update(){
        if (Input.GetKey(KeyCode.Q)){
            if (debounce){
                return;
            }
            else {
                if (preview.active){
                    itemscript.enabled = false;
                    preview.SetActive(false);
                    debounce = true;
                    StartCoroutine(waitDebounce());
                    placementOn = false;
                }
                else{
                    itemscript.enabled = true;
                    preview.SetActive(true);
                    debounce = true;
                    StartCoroutine(waitDebounce());
                    placementOn = true;
                }
            }
        }
    }

    public IEnumerator waitDebounce(){
        yield return new WaitForSeconds(0.25f);
        debounce = false;
    }
}
