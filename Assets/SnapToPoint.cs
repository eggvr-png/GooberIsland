using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPoint : MonoBehaviour
{
    public bool touchedSnap;
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.8f);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("SnapPoint"))
            {
                transform.position = hit.transform.position;
                break; // stop after the first one we find!
            }
        }
    }
}
