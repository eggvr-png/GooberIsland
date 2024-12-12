using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseStore : MonoBehaviour
{
    public string open = "Open";
    public string close = "Close";

    public Animator animator;

    public bool opened;

    public AudioReverbFilter arf;
    public AudioLowPassFilter alpf;
    public AudioHighPassFilter ahpf;

    public void interact(){
        if (opened == true){
            animator.SetBool("Opened", false);
            opened = false;
            arf.enabled = true;
            alpf.enabled = true;
            ahpf.enabled = true;
            return;
        }
        else {
            animator.SetBool("Opened", true);
            opened = true;
            arf.enabled = false;

            ahpf.enabled = false;
            return;
        }
    }
}
