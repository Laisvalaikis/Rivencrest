using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    private MusicIndex _musicIndex;

    private void Start()
    {
        _musicIndex = GetComponent<MusicIndex>();
    }

    public void SceneTransition()
    {
        _musicIndex.ChangeLevelMusic();
        LoadingScreenController.LoadScene(sceneToLoad);
    }
    
    public void SceneTransition(int sceneIndex)
    {
        _musicIndex.ChangeLevelMusic();
        LoadingScreenController.LoadScene(sceneIndex);
    }
    
    public void QuitGame() 
    {
        Application.Quit();
    }
}
