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
    [SerializeField] private CharacterTable characterTable;
    [SerializeField] private TownHall townHall;
    private bool embarStatek;
    private Data _data;
    private bool pauseMenuEnabled = false;
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
        _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
        _data.canButtonsBeClicked = false;
        pauseMenuInGame.gameObject.SetActive(true);
        confirmMenu.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void UnPauseInGame()
    {
        _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
        _data.canButtonsBeClicked = true;
        pauseMenu.gameObject.SetActive(false);
        confirmMenu.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void PauseTown()
    {
        if (!pauseMenuEnabled)
        {
            _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
            _data.canButtonsBeClicked = false;
            embarStatek = embark.interactable;
            embark.interactable = false;
            DisableTownButtons();
            pauseMenu.gameObject.SetActive(true);
            confirmMenu.SetActive(false);
            Time.timeScale = 0;
            pauseMenuEnabled = true;
        }

    }
    
    public void UnPauseTown()
    {
        if (pauseMenuEnabled)
        {
            _data.canButtonsBeClickedState = _data.canButtonsBeClicked;
            _data.canButtonsBeClicked = true;
            embark.interactable = embarStatek;
            EnableTownButton();
            pauseMenu.gameObject.SetActive(false);
            confirmMenu.SetActive(false);
            Time.timeScale = 1;
            pauseMenuEnabled = false;
        }
    }

    public void EnableDisablePauseGame()
    {
        if (pauseMenuEnabled)
        {
            UnPauseTown();
        }
        else
        {
            PauseTown();
        }
    }

    public void DisableTownButtons()
    {
        if (portraitBar != null)
        {
            portraitBar.DisableAllButtons();
        }

        if (characterTable != null)
        {
            characterTable.DisableAllButtons();
        }

        if (townHall != null)
        {
            townHall.DisableAllButtons();
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
        
        if (characterTable != null)
        {
            characterTable.EnableAllButtons();
        }
        
        if (townHall != null)
        {
            townHall.EnableAllButtons();
        }
        
        if (recruitmentButton != null)
        {
            recruitmentButton.interactable = true;
            townHallButton.interactable = true;
        }
    }
}
