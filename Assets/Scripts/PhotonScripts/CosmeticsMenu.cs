using UnityEngine;

public class CosmeticsMenu : MonoBehaviour
{
    private void Start()
    {
        var savedCosmeticId = PlayerPrefs.GetInt("cosmetic");
        InterSceneDataKeeper.Instance.cosmeticId = savedCosmeticId;
    }

    public void changeCosmetic(int cosmeticId)
    {
        InterSceneDataKeeper.Instance.cosmeticId = cosmeticId;
        PlayerPrefs.SetInt("cosmetic", cosmeticId);
    }
}