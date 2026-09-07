using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("Refrences")]
    public GameObject previewPrefab;
    public GameObject snapPointPrefabs;
    [Space]
    public Transform placeRayStart;
    public Transform placeRayEnd;
    [Space]
    public AudioSource soundPlayer;
    public AudioClip placeSound;
    public AudioClip snapSound;
    [Space]
    public Material previewMat;

    [Header("Varaibles")]
    public string tagNameOrSomething = "SnapPoints";

    private Vector3 velocity = Vector3.zero;

    bool inPlaceMode;
    bool snapped;
    bool wasSnapped;
    bool unsnapping;
    Vector3 unsnapTarget;

    GameObject previewObject;

    void Update()
    {
        // wow its so clean and stuff
        // like wowwww
        if (Input.GetKeyDown(KeyCode.R))
            buildToggle();

        if (inPlaceMode)
        {
            moveThingyToLookie();

            if (Input.GetMouseButtonDown(0))
            {
                spawnFloor();
            }
        }
    }

    public void buildToggle()
    {
        inPlaceMode = !inPlaceMode;

        if (inPlaceMode)
        {
            // do i need to explain it?
            previewObject = Instantiate(previewPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer renderer in renderers)
            {
                renderer.material = previewMat;
            }
            soundPlayer.PlayOneShot(placeSound);
        }
        else if (!inPlaceMode)
        {
            Destroy(previewObject);

            // just in case unity is dumb (alot of the time)
            previewObject = null;

            snapped = false;
            wasSnapped = false;
            unsnapping = false;
            velocity = Vector3.zero;
        }
    }

    public void moveThingyToLookie()
    {
        Ray ray = new Ray(placeRayStart.position, placeRayEnd.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            snapped = false;

            // the evil newpos function that adds height to the floor D:
            Vector3 newPos = new Vector3(hit.point.x, hit.point.y + 0.05f, hit.point.z);

            Collider[] hits = Physics.OverlapSphere(hit.point, 1f);

            foreach (Collider hitobj in hits)
            {
                if (hitobj.CompareTag(tagNameOrSomething))
                {
                    unsnapping = false;

                    // ease the thingy to the thingy
                    previewObject.transform.position = Vector3.SmoothDamp(previewObject.transform.position, hitobj.transform.position, ref velocity, 0.05f);

                    snapped = true;

                    if (!wasSnapped)
                    {
                        soundPlayer.PlayOneShot(snapSound);
                    }

                    break; // ow my bones
                }
            }

            if (!snapped)
            {
                if (wasSnapped)
                {
                    unsnapping = true;
                }

                if (unsnapping)
                {
                    // unease the thingy to the lookamabob
                    previewObject.transform.position = Vector3.SmoothDamp(previewObject.transform.position, newPos, ref velocity, 0.05f);

                    if (Vector3.Distance(previewObject.transform.position, newPos) < 0.01f)
                    {
                        // now snap to player look dir
                        previewObject.transform.position = newPos;
                        unsnapping = false;
                        velocity = Vector3.zero;
                    }
                }
                else
                {
                    previewObject.transform.position = newPos;
                    velocity = Vector3.zero;
                }
            }

            // Remember whether we were snapped this frame
            wasSnapped = snapped;
        }
    }

    public void spawnFloor()
    {
        // preview should be the spawning item so we can assume it here!
        Vector3 previewPos = previewObject.transform.position;

        GameObject floor = Instantiate(previewPrefab, previewPos, Quaternion.identity);

        floor.tag = "Placed";
        floor.GetComponent<BoxCollider>().enabled = true;

        // play le noise
        soundPlayer.PlayOneShot(placeSound);

        GameObject snapPoints = Instantiate(snapPointPrefabs, floor.transform);

        Collider[] hits = Physics.OverlapSphere(floor.transform.position, 0.5f);

        foreach (Collider hitobj in hits)
        {
            if (hitobj.CompareTag(tagNameOrSomething))
            {
                Destroy(hitobj.gameObject);
            }
        }
    }
}