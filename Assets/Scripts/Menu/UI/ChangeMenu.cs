using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    public GameObject orignalMenu;
    public GameObject newMenu;

    public bool isEscape = false;

    private AddRemoveLowPass lowPassComponent;

    private void Awake()
    {
        lowPassComponent = GetComponent<AddRemoveLowPass>();
    }

    private void Update()
    {
        if (isEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            Switch();
            lowPassComponent?.addAudioLowPass();//is a botch but works
        }
    }

    public void Switch()
    {
        newMenu.SetActive(true);
        orignalMenu.SetActive(false);
    }
}
