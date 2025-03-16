using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInrto : MonoBehaviour
{
    public Animator animator;
    public AudioSource music;

    void OnEnable() {
        animator.enabled = false;
        music.Play();
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation() {
        yield return null; // wait one frame to make sure Unity chills out
        animator.enabled = true;
        animator.Play("intro", 0, 0f);
    }
}
