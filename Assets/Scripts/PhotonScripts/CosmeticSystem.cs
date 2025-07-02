using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class CosmeticSystem : MonoBehaviourPunCallbacks
{
    [Header("Cosmetics")]
    public GameObject[] cosmetics;
    public int cosmeticId;

    private void Start()
    {
        cosmeticId = InterSceneDataKeeper.Instance.cosmeticId;
    }

    [PunRPC]
    public void setCosmetic(int cosmeticId)
    {
        var newCosmeticId = cosmeticId - 1;
        
        if (newCosmeticId == -1)
        {
            return;
        }
        else
        {
            GameObject cosmeticToWear = cosmetics[newCosmeticId];
            cosmeticToWear.SetActive(true);
        }
    }
}
