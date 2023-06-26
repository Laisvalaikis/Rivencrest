using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneChangingButton : MonoBehaviour
{
    public TextMeshProUGUI text; 
    public int SceneToLoad;
    public bool TextHighlight = false;
    public bool FrameHighlight = true;
    private Color OriginalTextColor;
    
    [SerializeField] private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        OriginalTextColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene()
    { 
        // MusicManager.instance.NextLevelMusic();
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
        Time.timeScale = 1;
    }
    public void SceneTransition()
    {
        LoadingScreenController.LoadScene(SceneToLoad);
    }
    public void OnHover()
    {
        if (TextHighlight)
        {
            text.color = Color.white;
        }
        if(FrameHighlight)
        {
            transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", true);
        }
    }
    public void OffHover()
    {
        if (TextHighlight)
        {
            text.color = OriginalTextColor;
        }
        if(FrameHighlight)
        {
            transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", false);
        }
    }
    public void QuitGame() 
    {
        Application.Quit();
    }
}
