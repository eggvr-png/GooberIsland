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

    void Start(){
        if (XRSettings.isDeviceActive) {
            Debug.Log("Player in VR");
            foreach (Canvas c in canvases){
                c.renderMode = RenderMode.WorldSpace;
                c.transform.position = vrcanvasPos.transform.position;
                c.transform.localScale = vrcanvasPos.transform.localScale;
            }

            pixelFSS.SetFloat("_PS", 600.5f);

            standeredCam.SetActive(false);
            xr.SetActive(true);
        }
    }
}
