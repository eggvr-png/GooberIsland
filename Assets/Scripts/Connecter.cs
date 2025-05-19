using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Connecter : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GameObject collidedObj = other.transform.gameObject;
        Connecter script = collidedObj.GetComponent<Connecter>();
        


        if (script != null)
        {
            FixedJoint joint = this.AddComponent<FixedJoint>();
            joint.connectedBody = collidedObj.GetComponent<Rigidbody>();

            Destroy(this);
        }
    }
}
