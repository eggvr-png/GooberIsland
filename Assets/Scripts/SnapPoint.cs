using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public bool vertical;
    public bool connectedToWall;

    public void Awake()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Placed") && !vertical)
            {
                Destroy(this.gameObject);
                break;
            }

            if (hit.CompareTag("SnapPoint"))
            {
                if(hit.gameObject != this.gameObject)
                {
                    if (hit.gameObject.GetComponent<SnapPoint>().vertical == false && vertical == false)
                    {
                        Destroy(hit.gameObject);
                    }
                }
            }
        }

        this.gameObject.GetComponent<Renderer>().enabled = false;
    }

    void OnDrawGizmos()
    {
        if (!vertical)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}
