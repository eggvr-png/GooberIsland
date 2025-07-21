using System.Collections;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("Refrences")]
    public GameObject objectToSpawn;
    [Space]
    public GameObject horizontalSnapPointPrefab;
    [Space]
    public Material previewMaterial;
    [Header("Settings")]
    public Transform rayStartPos;
    [SerializeField]private bool inPlaceMode;
    
    bool debounceEntering;
    bool debouncePlacing;
    bool createdPreview;
    GameObject preview;

    private void Update()
    {
        // theres way to many if statements in here holy shit :sob:
        if (Input.GetKey(KeyCode.R) && !debounceEntering)
        {
            inPlaceMode = !inPlaceMode; // js learned you can do this :sob:
            debounceEntering = true;
            StartCoroutine(debounceEnter());
        }

        // manages the preview. this should work i hope :P
        if (inPlaceMode && preview == null)
        {
            preview = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity);
            preview.GetComponent<Renderer>().material = previewMaterial;
        }
        if (!inPlaceMode && preview != null)
        {
            Destroy(preview);
        }

        if (preview != null && inPlaceMode)
        {
            Ray ray = new Ray(rayStartPos.position, rayStartPos.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                preview.transform.position = hit.point;
                Collider[] hits = Physics.OverlapSphere(preview.transform.position, 0.5f);

                foreach (Collider hited in hits)
                {
                    if (hited.CompareTag("SnapPoint"))
                    {
                        preview.transform.position = hited.transform.position;
                        preview.transform.rotation = hited.transform.rotation;
                        break;
                    }
                }
            }
        }

        if (preview != null && inPlaceMode && !debouncePlacing)
        {
            if (Input.GetMouseButton(0))
            {
                debouncePlacing = true;
                GameObject placedObject = Instantiate(objectToSpawn, preview.transform.position, preview.transform.rotation);
                placedObject.tag = "Placed";
                placedObject.GetComponent<BoxCollider>().enabled = true;
                Instantiate(horizontalSnapPointPrefab, placedObject.transform);
                StartCoroutine(debouncePlace());
            }
        }
    }

    IEnumerator debounceEnter()
    {
        yield return new WaitForSeconds(0.2f);
        debounceEntering = false;
    }
    IEnumerator debouncePlace()
    {
        yield return new WaitForSeconds(1);
        debouncePlacing = false;
    }
}
