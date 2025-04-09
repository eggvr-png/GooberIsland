using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class cuberaycaster : MonoBehaviour
{
    public GameObject cubeprefab;
    public bool canraycast;

    public int unplaceableLayer;

    public Material red;
    public Material white;

    GameObject preview;
    bool unplaceable;

    // Start is called before the first frame update
    void Start()
    {
        canraycast = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            canraycast = !canraycast;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canraycast == true)
            {
                Vector3 ScreenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit))
                {
                    if (!unplaceable){
                        Instantiate(cubeprefab, hit.point, Quaternion.identity);
                    }
                    Debug.Log ("hit: " + hit.collider.gameObject.name);
                
                }
                else
                {
                    Debug.Log("no hit");
                }
            }
        }

        if (canraycast) {
            if (preview == null) {
                preview = Instantiate(cubeprefab, new Vector3(0,0,0), Quaternion.identity);
                Destroy(preview.GetComponent<Collider>());
            }
            else {
                Vector3 ScreenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)){
                    preview.transform.position = hit.point;
                    if (hit.collider.gameObject.layer == unplaceableLayer){
                        unplaceable = true;
                        preview.GetComponent<Renderer>().material = red;
                    }
                    else {
                        unplaceable = false;
                        preview.GetComponent<Renderer>().material = white;
                    }
                }
            }
        }
        else {
            if (preview != null){
                Destroy(preview);
            }
        }
    }
}
