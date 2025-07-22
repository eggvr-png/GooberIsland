using System.Collections;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("Refrences")]
    public GameObject objectToSpawn;
    [Space]
    public GameObject horizontalSnapPointPrefab;
    public GameObject wallFloorSnapPointPrefab;
    public GameObject wallSnapPointPrefab;
    [Space]
    public Material previewMaterial;
    [Header("Settings")]
    public Transform rayStartPos;
    [SerializeField] private bool inPlaceMode;
    public enum objectToPlace
    {
        Floor,
        Wall
    }
    public objectToPlace typeOfObject;
    
    bool debounceEntering;
    bool debouncePlacing;
    bool debounceRotate;
    bool createdPreview;
    int wallRot;
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

        if (Input.GetKey(KeyCode.F) && !debounceEntering)
        {
            if (typeOfObject == objectToPlace.Floor)
            {
                typeOfObject = objectToPlace.Wall;
            }
            else { 
                typeOfObject = objectToPlace.Floor;
            }
            debounceEntering = true;
            StartCoroutine(debounceEnter());
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && !debounceRotate && typeOfObject == objectToPlace.Wall)
        {
            if (scroll > 0f)
            {
                wallRot = wallRot + 45;
                if (wallRot > 360)
                {
                    wallRot = 0;
                }
                debounceRotate = true;
                StartCoroutine(debounceRot());
            }
            if (scroll < 0f)
            {
                wallRot = wallRot - 45;
                if (wallRot > 360)
                {
                    wallRot = 0;
                }
                debounceRotate = true;
                StartCoroutine(debounceRot());
            }
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

        if (preview != null && typeOfObject == objectToPlace.Wall)
        {
            preview.transform.rotation = Quaternion.Euler(0, wallRot, 90);
        } 
        else if (preview != null && typeOfObject == objectToPlace.Floor)
        {
            preview.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (preview != null && inPlaceMode)
        {
            Ray ray = new Ray(rayStartPos.position, rayStartPos.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 wallPos = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                if (typeOfObject == objectToPlace.Wall)
                {
                    preview.transform.position = wallPos;
                }
                else
                {
                    preview.transform.position = hit.point;
                }
                Collider[] hits = Physics.OverlapSphere(preview.transform.position, 0.5f);

                foreach (Collider hited in hits)
                {
                    if (hited.CompareTag("SnapPoint"))
                    {
                        Vector3 snappedWallPos = new Vector3(hited.transform.position.x, hited.transform.position.y + 1, hited.transform.position.z);
                        if (typeOfObject == objectToPlace.Wall)
                        {
                            preview.transform.position = snappedWallPos;
                        }
                        else
                        {
                            preview.transform.position = hited.transform.position;
                        }
                        break;
                    }
                    else if (hited.CompareTag("SnapPointVertical") && typeOfObject == objectToPlace.Wall)
                    {
                        Vector3 snappedWallPos = new Vector3(hited.transform.position.x, hited.transform.position.y + 1, hited.transform.position.z);
                        SnapPoint sp = hited.gameObject.GetComponent<SnapPoint>();
                        if (sp.vertical == true)
                        {
                            preview.transform.position = snappedWallPos;
                        }
                        else if (sp.connectedToWall == true)
                        {
                            preview.transform.position = hited.transform.position;
                        }
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
                if (typeOfObject == objectToPlace.Floor)
                {
                    Instantiate(horizontalSnapPointPrefab, placedObject.transform);
                    Instantiate(wallFloorSnapPointPrefab, placedObject.transform);
                }
                else
                {
                    Instantiate(wallSnapPointPrefab, placedObject.transform);
                }
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
    IEnumerator debounceRot()
    {
        yield return new WaitForSeconds(0.1f);
        debounceRotate = false;
    }
}
