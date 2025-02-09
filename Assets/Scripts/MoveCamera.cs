using UnityEngine;
using UnityEngine.XR;

public class MoveCamera : MonoBehaviour {

    public Transform player;

    void Update() {
        if (XRSettings.isDeviceActive){
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
        }
        else {
            transform.position = player.position;
        }
    }
}
