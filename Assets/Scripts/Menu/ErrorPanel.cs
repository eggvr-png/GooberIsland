
using TMPro;
using UnityEngine;

public class ErrorPanel : MonoBehaviour
{
    public TextMeshProUGUI errorTextGUI;
    public GameObject errorPanel;

    // Update is called once per frame
    void Update()
    {
        if (InterSceneDataKeeper.errorText != null)
        {
            errorPanel.SetActive(true);
            errorTextGUI.text = InterSceneDataKeeper.errorText;
            InterSceneDataKeeper.errorText = null;
        }
    }

    
}
