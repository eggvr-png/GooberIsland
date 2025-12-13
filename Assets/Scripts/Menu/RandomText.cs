using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomText : MonoBehaviour
{
    [SerializeField] List<string> randomText;
    [SerializeField] TextMeshProUGUI tmpUG;

    private void Start()
    {
        chooseRandom();
    }

    public void chooseRandom()
    {
        int randomTextNumber = Random.Range(0, randomText.Count);
        tmpUG.text = randomText[randomTextNumber];
    }
}