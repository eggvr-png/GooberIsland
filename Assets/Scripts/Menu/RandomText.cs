using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomText : MonoBehaviour
{
    public List<string> randomText;

    public TextMeshProUGUI tmpUG;

    public int randomTextNumber;

    public int minNum;
    public int maxNum;

    public void chooseRandom(){
        randomTextNumber = Random.Range(minNum, maxNum);

        tmpUG.text = randomText[randomTextNumber];
    }

    void Start(){
        randomTextNumber = Random.Range(minNum, maxNum);
        chooseRandom();
    }
}
