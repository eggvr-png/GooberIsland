using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuberaycaster : MonoBehaviour
{
    public GameObject cubeprefab;
    public bool canraycast;
    // Start is called before the first frame update
    void Start()
    {
        canraycast = true;
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
                    Instantiate(cubeprefab, hit.point, Quaternion.identity);
                    Debug.Log ("hit");
                
                }
                else
                {
                    Debug.Log("no hit");
                }
            }
        }
    }
}
