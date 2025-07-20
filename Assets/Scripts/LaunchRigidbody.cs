using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchRigidbody : MonoBehaviour
{
    public Rigidbody body;

    public void launch()
    {
        body.AddExplosionForce(1000, body.gameObject.transform.position, 100);
    }
}
