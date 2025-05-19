
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Animations : MonoBehaviour
{
    public Vector3 lastposition;
    public float speed;

    public Animator animator;

    void Start()
    {
        lastposition = transform.position;
    }

    void Update()
    {
        speed = (transform.position - lastposition).magnitude / Time.deltaTime;
        lastposition = transform.position;

        if (speed != 0)
            animator.SetBool("walking", true);
        else 
            animator.SetBool("walking", false);
    }
}