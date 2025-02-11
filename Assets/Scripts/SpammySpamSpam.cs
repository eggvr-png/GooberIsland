using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpammySpamSpam : MonoBehaviour
{
    public GameObject spammy;
    public Transform spammypos;
    void Update()
    {
        Instantiate(spammy, spammypos.position, quaternion.identity);
    }
}
