using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fragsurf.Movement;

public class StaminaBar : MonoBehaviour
{
    public Slider bar;
    [SerializeField]private float staminaLeft = 1;

    Vector3 lastposition;

    private void Update()
    {
        float speed;

        speed = (transform.position - lastposition).magnitude / Time.deltaTime;
        lastposition = transform.position;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            bar.value = bar.value - 0.001f;
            staminaLeft = staminaLeft - 0.001f;
        }
        lastposition = transform.position;
    }
}
