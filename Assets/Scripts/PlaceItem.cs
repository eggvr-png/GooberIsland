using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlaceItem : MonoBehaviour
{
    public EnablePlacement checker;

    public PreviewItem preview;

    public GameObject objToSpawn;

    private bool debounce;

    public void Update(){
        if (Input.GetMouseButton(1)){
            if (debounce){
                return;
            }
            else {
                if (checker.enabled == true && preview.ableToSpawn){
                    Instantiate(objToSpawn, checker.itemscript.preview.transform.position, Quaternion.identity);
                debounce = true;
                StartCoroutine(waitDebounce());
                }
            }
        }
    }

    public IEnumerator waitDebounce(){
        yield return new WaitForSeconds(0.25f);
        debounce = false;
    }
}
