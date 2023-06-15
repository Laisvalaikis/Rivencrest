using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
    private static LoadingScreenController Instance;
    [SerializeField] private CanvasGroup darkScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float fadeLength;
    [SerializeField] private float waitTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void LoadScene(int sceneToLoad)
    {
        Instance.StartCoroutine(Instance.SceneTransition(sceneToLoad));
    }

    IEnumerator SceneTransition(int sceneName)
    {
        darkScreen.alpha = 0;
        darkScreen.gameObject.SetActive(true);
        float timer = 0f;
        while(timer < fadeLength)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        loadingScreen.SetActive(true);
        timer = fadeLength;
        while (timer > 0)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        darkScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        var operation = SceneManager.LoadSceneAsync(sceneName);
        while(!operation.isDone)
        {
            yield return null;
        }
        darkScreen.alpha = 0;
        darkScreen.gameObject.SetActive(true);
        timer = 0f;
        while (timer < fadeLength)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        loadingScreen.SetActive(false);
        timer = fadeLength;
        while (timer > 0)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, fadeLength);
            darkScreen.alpha = timer / fadeLength;
            yield return null;
        }
        darkScreen.gameObject.SetActive(false);
    }
}
