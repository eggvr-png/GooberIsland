using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSnapping : MonoBehaviour
{
    // just incase i hate myself and change the tag name
    public string tagNameOrSomething = "SnapPoints";

    public float sizeOfThingamagig = 1;

    private Vector3 velocity = Vector3.zero;

    public void Update()
    {
        // god i love physics (gilp)
        Collider[] hits = Physics.OverlapSphere(transform.position, sizeOfThingamagig);

        bool foundSnapPoint = false;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(tagNameOrSomething))
            {
                transform.position = Vector3.SmoothDamp(transform.position,hit.transform.position,ref velocity,0.15f);

                foundSnapPoint = true;
                break; // ow my bones
            }
        }

        if (!foundSnapPoint)
        {
            velocity = Vector3.zero;
        }
    }

    // rubbing my big gizmo filled belly
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sizeOfThingamagig);
    }
}