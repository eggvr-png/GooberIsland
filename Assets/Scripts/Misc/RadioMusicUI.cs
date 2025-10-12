using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RadioMusicUI : MonoBehaviour
{
    public TextMeshProUGUI title;

    string oldTitle;

    public AudioSource menuMusic;

    void Start()
    {
        if (InterSceneDataKeeper.Instance.currentSongTitle != null)
        {
            title.text = InterSceneDataKeeper.Instance.currentSongTitle;

            oldTitle = InterSceneDataKeeper.Instance.currentSongTitle;
        }
    }

    void Update()
    {
        if (InterSceneDataKeeper.Instance.currentSongTitle != oldTitle)
        {
            title.text = InterSceneDataKeeper.Instance.currentSongTitle;
        }

        oldTitle = InterSceneDataKeeper.Instance.currentSongTitle;
    }

    public void startTest()
    {
        if (InterSceneDataKeeper.Instance.currentSong != null)
        {
            menuMusic.pitch = 0;
            InterSceneDataKeeper.Instance.gameObject.GetComponent<AudioSource>().clip = InterSceneDataKeeper.Instance.currentSong;
            InterSceneDataKeeper.Instance.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void stopTest()
    {
        if (InterSceneDataKeeper.Instance.currentSong != null)
        {
            menuMusic.pitch = 1;
            InterSceneDataKeeper.Instance.gameObject.GetComponent<AudioSource>().Stop();
        }
    }
}
