using UnityEngine;
using Photon.Pun;
using TMPro;

public class NamingSystem : MonoBehaviourPunCallbacks
{
    [Header("Refrences")]
    public TextMeshPro username;

    [PunRPC]
    public void sendUsername()
    {
        username.text = InterSceneDataKeeper.Instance.playerName;
    }
}
