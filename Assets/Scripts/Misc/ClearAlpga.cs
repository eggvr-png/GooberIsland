using UnityEngine;

public class ClearAlpga : MonoBehaviour
{
    void OnPreRender()
    {
        GL.Clear(true, true, new Color(0,0,0,0));
    }
}