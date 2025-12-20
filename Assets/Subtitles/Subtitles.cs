using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class Subtitles : MonoBehaviour
{
    public TextAsset subtitlesJson;
    [Space]
    public GameObject subtitleMain;
    public TextMeshProUGUI text;
    string jsonText;
    Dictionary<string, Dictionary<string, string>> subtitles;
    
    void Awake()
    {
        string jsonText = subtitlesJson.text;

        subtitles = JsonConvert.DeserializeObject<
            Dictionary<string, Dictionary<string, string>>
        >(subtitlesJson.text);
    }

    public void displaySubtitle(string character, string num, Color textColor)
    {
        string line = subtitles[character][num];

        subtitleMain.SetActive(true);
        text.text = character + ": " + line;
        text.color = textColor;
    }
}
