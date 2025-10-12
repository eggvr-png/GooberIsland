using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Renderer[] bodyPartRenderers;

    public void isLocal()
    {
        foreach (Renderer r in bodyPartRenderers) {
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }
}
