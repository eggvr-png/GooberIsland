using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class VRMenuSwitch : MonoBehaviour
{
    public Canvas[] canvases;
    public Canvas vrcanvasPos;

    public Material pixelFSS;

    public GameObject standeredCam;
    public GameObject xr;

    public bool ismenu;
    public bool forever;

    void Update(){
        if (forever){
            foreach (Canvas c in canvases){
                c.renderMode = RenderMode.WorldSpace;
                c.transform.position = vrcanvasPos.transform.position;
                c.transform.localScale = vrcanvasPos.transform.localScale;
                c.transform.rotation = vrcanvasPos.transform.rotation;
            }
        }
    }

    void Start(){
        if (XRSettings.isDeviceActive) {
            Debug.Log("Player in VR");
            foreach (Canvas c in canvases){
                c.renderMode = RenderMode.WorldSpace;
                c.transform.position = vrcanvasPos.transform.position;
                c.transform.localScale = vrcanvasPos.transform.localScale;
            }

            pixelFSS.SetFloat("_PS", 650.5f);
            if (ismenu)
                standeredCam.SetActive(false);
                xr.SetActive(true);
        }
    }
}
