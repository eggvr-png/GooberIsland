using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoints : MonoBehaviour
{
    public void Awake()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider hitobj in hits)
        {
            if (hitobj.CompareTag("Placed"))
            {
                Debug.Log("kill me.. later");
                Destroy(this.gameObject);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }
}
