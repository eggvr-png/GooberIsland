using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class VRMenuSwitch : MonoBehaviour
{
    public Canvas[] canvases;
    public Canvas vrcanvasPos;

    public GameObject defaultRaft;
    public GameObject xrRaft;

    public Material pixelFSS;

    public GameObject standeredCam;
    public GameObject xr;

    public bool forceVR;

    public GameObject[] disableInVR;

    void Start(){
        if (XRSettings.isDeviceActive || forceVR) {
            Debug.Log("Player in VR");
            foreach (Canvas c in canvases){
                c.renderMode = RenderMode.WorldSpace;
                c.transform.position = vrcanvasPos.transform.position;
                c.transform.localRotation = vrcanvasPos.transform.localRotation;
                c.transform.localScale = vrcanvasPos.transform.localScale;
            }

            foreach (GameObject o in disableInVR)
            {
                o.SetActive(false);
            }

            pixelFSS.SetFloat("_PS", 600.5f);

            standeredCam.SetActive(false);
            xr.SetActive(true);

            defaultRaft.SetActive(false);
            xrRaft.SetActive(true);
        }
    }
}
