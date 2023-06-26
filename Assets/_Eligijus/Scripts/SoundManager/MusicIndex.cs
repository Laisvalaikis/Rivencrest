using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIndex : MonoBehaviour
{
    [SerializeField] private int onStartMusicIndex = 0;
    [SerializeField] private bool changeOnAwake = false;

    [SerializeField] private int changeMusicIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (changeOnAwake)
        {
            MusicManager.Instance.ChangeLevelMusic(onStartMusicIndex);
        }
    }

    public void ChangeLevelMusic()
    {
        MusicManager.Instance.ChangeLevelMusic(changeMusicIndex);
    }
}
