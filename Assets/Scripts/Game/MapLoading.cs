using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// i made this back in 2024 for a gtag fan game :/
public class MapLoading : MonoBehaviour
{
    [Header("Options")]
    public bool load;
    public bool unload;
    [Space]
    public bool multipleMaps;
    [Space]
    public string handTag;
    [Header("Map")]
    public GameObject map;
    [Header("Maps (use if multipleMaps checked)")]
    public GameObject[] maps;

    public void triggered() {
        if (multipleMaps) {
            if (load) {
                foreach (GameObject zone in maps){
                    zone.SetActive(true);
                }
            }
            else if (unload) {
                foreach (GameObject zone in maps){
                    zone.SetActive(false);
                }
            }
            else {
                Debug.LogWarning("Load or Unload is not selected.");
            }
        }
        else {
            if (load) {
                map.SetActive(true);
            }
            else if (unload) {
                map.SetActive(false);
            }
            else {
                Debug.LogWarning("Load or Unload is not selected.");
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == handTag) {
            triggered();
        }
    }
}
