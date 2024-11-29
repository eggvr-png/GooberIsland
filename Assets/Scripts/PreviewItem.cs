using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PreviewItem : MonoBehaviour
{
private Ray ray;
RaycastHit hit;
public LayerMask Ground;
public GameObject preview;

public Material able;
public Material unable;

public bool ableToSpawn;

public void Start(){
}

public void Update(){
ray = new Ray(transform.position, transform.forward);
checkForCollison();
}
public void checkForCollison(){
if (Physics.Raycast(ray, out hit)) {
if (hit.collider.transform.gameObject.layer == 8){
preview.transform.position = hit.point;
preview.transform.position = new UnityEngine.Vector3(preview.transform.position.x, preview.transform.position.y, preview.transform.position.z);
preview.transform.GetChild(0).GetComponent<Renderer>().material = able;
ableToSpawn = true;
}
else {
preview.transform.position = hit.point;
preview.transform.position = new UnityEngine.Vector3(preview.transform.position.x, preview.transform.position.y, preview.transform.position.z);
preview.transform.GetChild(0).GetComponent<Renderer>().material = unable;
ableToSpawn = false;
}
}
}
}

