using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyPolicyAccept : MonoBehaviour
{
    public void accept(){
        PlayerPrefs.SetInt("privacyPolicyAccept", 1);
    }
}
