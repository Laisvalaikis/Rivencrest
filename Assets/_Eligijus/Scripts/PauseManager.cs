using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject confirmMenu;
    [SerializeField] private GameObject pauseMenuInGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button embark;
    [Header("Town Buttons")]
    [SerializeField] private Button recruitmentButton;
    [SerializeField] private Button townHallButton;
    [SerializeField] private PortraitBar portraitBar;
    private bool embarStatek;
    private Data _data;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (_data == null)
        {
            _data = Data.Instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PauseInGame()
    {
        gameObject.SetActive(true);
        _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
        _data.canButtonsBeClicked = false;
        pauseMenuInGame.gameObject.SetActive(true);
        confirmMenu.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void UnPauseInGame()
    {
        gameObject.SetActive(false);
        _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
        _data.canButtonsBeClicked = true;
        pauseMenu.gameObject.SetActive(false);
        confirmMenu.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void PauseGame()
    {
        gameObject.SetActive(true);
        _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
        _data.canButtonsBeClicked = false;
        embarStatek = embark.interactable;
        embark.interactable = false;
        DisableTownButtons();
        pauseMenu.gameObject.SetActive(true);
        confirmMenu.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void UnPauseGame()
    {
        gameObject.SetActive(false);
        _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
        _data.canButtonsBeClicked = true;
        embark.interactable = embarStatek;
        EnableTownButton();
        pauseMenu.gameObject.SetActive(false);
        confirmMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void DisableTownButtons()
    {
        if (portraitBar != null)
        {
            portraitBar.DisableAllButtons();
        }
        if (recruitmentButton != null)
        {
            recruitmentButton.interactable = false;
            townHallButton.interactable = false;
        }
    }

    public void EnableTownButton()
    {
        if (portraitBar != null)
        {
            portraitBar.EnableAllButtons();
        }
        if (recruitmentButton != null)
        {
            recruitmentButton.interactable = true;
            townHallButton.interactable = true;
        }
    }
}
