using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
//thank you chatgpt - max 09/21/25
[RequireComponent(typeof(PhotonView))]
public class PlacementSystem : MonoBehaviourPunCallbacks
{
    [Header("Refrences")]
    public GameObject objectToSpawn;
    [Space]
    public GameObject horizontalSnapPointPrefab;
    [Space]
    public Material previewMaterial;
    [Header("Settings")]
    public Transform rayStartPos;
    [SerializeField] private bool inPlaceMode;

    // debounce variables to prevent spamming
    bool debounceEntering;
    bool debouncePlacing;
    bool createdPreview;
    GameObject preview;

    // History of placed objects for synchronization
    struct PlacementData { public Vector3 position; public Quaternion rotation; }
    private List<PlacementData> placementHistory = new List<PlacementData>();

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
            /*
            // not working yet
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
            {
                Ray ray = new Ray(rayStartPos.position, rayStartPos.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    PlacementData? dataToRemove = null;

                    foreach (var data in placementHistory)
                    {
                        if (data.position == hitObject.transform.position && data.rotation == hitObject.transform.rotation)
                        {
                            dataToRemove = data;
                            break;
                        }
                    }

                    if (dataToRemove.HasValue)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            photonView.RPC("Destroy", RpcTarget.All, hitObject, dataToRemove.Value);
                        }
                        else
                        {
                            photonView.RPC("RequestDestroy", RpcTarget.MasterClient, hitObject, dataToRemove.Value);
                        }
                    }
                }
            }
            else */if (Input.GetMouseButton(0))
            {
                debouncePlacing = true;
                Vector3 pos = preview.transform.position;
                Quaternion rot = preview.transform.rotation;
                // Master places directly, others request placement
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("SpawnAt", RpcTarget.All, pos, rot);
                }
                else
                {
                    photonView.RPC("RequestPlace", RpcTarget.MasterClient, pos, rot);
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

    [PunRPC]
    void SpawnAt(Vector3 position, Quaternion rotation)
    {
        GameObject placedObject = Instantiate(objectToSpawn, position, rotation);
        placedObject.tag = "Placed";
        placedObject.GetComponent<BoxCollider>().enabled = true;
        Instantiate(horizontalSnapPointPrefab, placedObject.transform);
    }

    [PunRPC]
    void RequestPlace(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        if (info.Sender != PhotonNetwork.LocalPlayer)
        {
            photonView.RPC("SpawnAt", info.Sender, position, rotation);
        }
    }
    /*

    [PunRPC]
    void Destroy(GameObject hitObject, PlacementData data)
    {
        if (placementHistory.Contains(data))
        {
            placementHistory.Remove(data);
            Destroy(hitObject);
        }
    }

    [PunRPC]
    void RequestDestroy(GameObject hitObject, PlacementData data, PhotonMessageInfo info)
    {
        if (info.Sender != PhotonNetwork.LocalPlayer)
        {
            photonView.RPC("Destroy", info.Sender, hitObject, data);
        }
    }*/
}
