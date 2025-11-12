using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCosmeticSys : MonoBehaviour
{
    public GameObject[] cosmetics;

    public Renderer[] limbs;

    public Material green;
    public Material blue;
    public Material pink;

    public int cosId;

    void Start()
    {
        int savedClr = PlayerPrefs.GetInt("clr");
        int savedHat = PlayerPrefs.GetInt("hat");

        if (savedHat == 0)
        {
            return;
        }
        else
        {
            changeCosmeticId(savedHat - 1);
        }

        if (savedClr == 1)
        {
            foreach (Renderer renderer in limbs)
            {
                renderer.material = blue;
            }
        }
        else if (savedClr == 2)
        {
            foreach (Renderer renderer in limbs)
            {
                renderer.material = pink;
            }
        }
    }

    public void changeCosmeticId(int cosmeticId)
    {
        cosId = cosmeticId;
        foreach (GameObject cosmetic in cosmetics)
        {
            cosmetic.SetActive(false);
        }
        if (cosId == -1)
        {
            PlayerPrefs.SetInt("hat", 0);
            PlayerPrefs.Save();
            return;
        }
        else
        {
            cosmetics[cosId].SetActive(true);
            PlayerPrefs.SetInt("hat", cosId + 1);
            PlayerPrefs.Save();
        }
    }

    public void changeColor(int color)
    {
        if (color == 0)
        {
            foreach (Renderer renderer in limbs)
            {
                renderer.material = green;
            }

            PlayerPrefs.SetInt("clr", 0);
            PlayerPrefs.Save();
        }

        if (color == 1)
        {
            foreach (Renderer renderer in limbs)
            {
                renderer.material = blue;
            }

            PlayerPrefs.SetInt("clr", 1);
            PlayerPrefs.Save();
        }

        if (color == 2)
        {
            foreach (Renderer renderer in limbs)
            {
                renderer.material = pink;
            }

            PlayerPrefs.SetInt("clr", 2);
            PlayerPrefs.Save();
        }
    }
}
