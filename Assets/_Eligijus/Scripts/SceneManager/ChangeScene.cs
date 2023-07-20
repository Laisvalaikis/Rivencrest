using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    public void SceneTransition()
    {
        UIStack.ClearStack();
        LoadingScreenController.LoadScene(sceneToLoad);
    }
    
    public void SceneTransition(int sceneIndex)
    {
        UIStack.ClearStack();
        LoadingScreenController.LoadScene(sceneIndex);
    }
    
    public void QuitGame() 
    {
        Application.Quit();
    }
    
}
