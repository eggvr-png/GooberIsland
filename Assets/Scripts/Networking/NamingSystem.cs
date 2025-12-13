using UnityEngine;
using Photon.Pun;
using TMPro;
/// <summary>
/// this might be one of the most simple scripts
/// in the code.
/// </summary>
public class NamingSystem : MonoBehaviourPunCallbacks
{
    [Header("Refrences")]
    public TextMeshPro username;
    public TMP_FontAsset kfont;

    [PunRPC]
    public void sendUsername()
    {
        username.text = InterSceneDataKeeper.Instance.playerName;
    }

    [PunRPC]
    public void kofiTag()
    {
        username.font = kfont;
    }
}
