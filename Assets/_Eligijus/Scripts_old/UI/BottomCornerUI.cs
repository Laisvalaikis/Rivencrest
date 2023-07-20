using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomCornerUI : MonoBehaviour
{
    public Color ButtonIconColor;
    //public GameObject characterPortrait;
    private GameObject GameInformation;
    private bool ActivateOnce = true;
    private GameObject selectedCharacter;
    private GameObject currentCharacter;
    private GameObject movementText;
    private GameObject healthText;
    public GameObject buttonPrefab;
    public List<TextMeshProUGUI> buttonText;
    public List<ActionButton> buttonAction;
    public List<Image> buttonImages;
    public List<Image> buttonBackgroundImages;
    public List<Image> backgroundImages;
    public Image staminaBackground;
    public Image portrait;
    public TextMeshProUGUI staminaPoints;
    public TextMeshProUGUI healthPoints;
    public Animator healthAnimator;
    public GameObject cornerUi;
    private ButtonManager _buttonManager;
    private PlayerInformationData _currentPlayerInformationData;
    void Awake()
    {
        // GameInformation = GameObject.Find("GameInformation").gameObject;
        // movementText = transform.GetChild(0).Find("MovementTextBackground").GetChild(0).gameObject;
        // healthText = transform.GetChild(0).Find("HealthBar").Find("HealthText").gameObject;
        _buttonManager = GetComponent<ButtonManager>();
    }

    /*void Update()
    {
        ChangesInCornerUI();
    }*/

    // public void ChangesInCornerUI()
    // {
    //     if (GameInformation.GetComponent<GameInformation>().SelectedCharacter != null)
    //     {
    //         currentCharacter = GameInformation.GetComponent<GameInformation>().SelectedCharacter;
    //     }
    //     else if (GameInformation.GetComponent<GameInformation>().InspectedCharacter != null)
    //     {
    //         currentCharacter = GameInformation.GetComponent<GameInformation>().InspectedCharacter;
    //     }
    //     else
    //     {
    //         currentCharacter = null;
    //     }
    //     if (currentCharacter != null &&
    //         currentCharacter.GetComponent<PlayerInformation>().characterUiData == characterUiData)
    //     {
    //         this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    //         /*characterPortrait.GetComponent<Image>().sprite = GameInformation.GetComponent<GameInformation>()
    //             .SelectedCharacter.GetComponent<PlayerInformation>().CharacterPortraitSprite;*/
    //         movementText.gameObject.GetComponent<Text>().text =
    //             currentCharacter.GetComponent<GridMovement>().AvailableMovementPoints.ToString(); //Sets movement text
    //         healthText.gameObject.GetComponent<Text>().text =
    //             currentCharacter.GetComponent<PlayerInformation>().health.ToString(); //Sets health text
    //         transform.GetChild(0).Find("HealthBar").Find("Health").GetComponent<Animator>().SetFloat("healthPercent",
    //             currentCharacter.GetComponent<PlayerInformation>().GetHealthPercentage()); //Sets float of animator
    //         //galima tiesiog zinot sito UI owneri (characteri)
    //         if (ActivateOnce || selectedCharacter != currentCharacter)
    //         {
    //             this.GetComponent<ButtonManager>().DisableSelection(this.GetComponent<ButtonManager>().ButtonFrameList[0]);
    //             ActivateOnce = false;
    //             selectedCharacter = currentCharacter;
    //         }
    //     }
    //     else
    //     {
    //         this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    //         ActivateOnce = true;
    //     }
    // }

    public void UpdateData(int buttonIndex, int index)
    {
        if (_buttonManager.CharacterOnBoard != null)
        {
            cornerUi.SetActive(true);
        }
        else
        {
            cornerUi.SetActive(false);
        }
        buttonText[buttonIndex].color = _currentPlayerInformationData.textColor;
        buttonImages[buttonIndex].sprite = _currentPlayerInformationData.abilities[index].sprite;
        buttonAction[buttonIndex].buttonState = _currentPlayerInformationData.abilities[index].abilityAction.ToString();
        buttonBackgroundImages[buttonIndex].color = _currentPlayerInformationData.backgroundColor;
        UpdateCommonData();
    }

    public void UpdateCommonData()
    {
        if (_buttonManager.CharacterOnBoard != null)
        {
            cornerUi.SetActive(true);
        }
        else
        {
            cornerUi.SetActive(false);
        }
        portrait.sprite = _currentPlayerInformationData.characterSprite;
        Color half = _currentPlayerInformationData.backgroundColor;
        half.a = 0.5f;
        staminaBackground.color = half;
        for (int i = 0; i < backgroundImages.Count; i++)
        {
            backgroundImages[i].color = _currentPlayerInformationData.backgroundColor;
        }
        currentCharacter = _buttonManager.CharacterOnBoard;
        staminaPoints.text = currentCharacter.GetComponent<GridMovement>().AvailableMovementPoints.ToString(); //Sets movement text
        healthPoints.text = currentCharacter.GetComponent<PlayerInformation>().health.ToString(); //Sets health text
        healthAnimator.SetFloat("healthPercent", currentCharacter.GetComponent<PlayerInformation>().GetHealthPercentage());
        _buttonManager.SelectMovementButton();
    }

    public void EnableAbilities(SavedCharacter savedCharacter)
    {
        // int currentButtonIndex = 2;
        // int buttonIndex = 0;
        // if (savedCharacter != null)
        // {
        //     for (int i = 0; i < savedCharacter.unlockedAbilities.Length; i++)
        //     {
        //         if (_buttonManager.ButtonList.Count > currentButtonIndex)
        //         {
        //             if (savedCharacter.prefab.GetComponent<ActionManager>().FindActionByIndex(i) != null && savedCharacter.unlockedAbilities[i] == '1')
        //             {
        //                 _buttonManager.ButtonList[currentButtonIndex].transform.parent.gameObject.SetActive(true);
        //                 UpdateData(buttonIndex, i);
        //                 // ButtonList[currentButtonIndex].transform.Find("ActionButtonImage").GetComponent<Image>().sprite = savedCharacter.prefab.GetComponent<ActionManager>().FindActionByIndex(i).AbilityIcon;
        //                 // ButtonList[currentButtonIndex].GetComponent<ActionButton>().buttonState = savedCharacter.prefab.GetComponent<ActionManager>().FindActionByIndex(i).actionName;
        //                 currentButtonIndex++;
        //                 buttonIndex++;
        //             }
        //         }
        //     }
        //     for (int i = currentButtonIndex; i < _buttonManager.ButtonList.Count; i++)
        //     {
        //         _buttonManager.ButtonList[i].transform.parent.gameObject.SetActive(false);
        //         // string extensionName = "Extension" + (i + 1).ToString();
        //         // transform.Find("CornerUI").Find(extensionName).gameObject.SetActive(false);
        //     }
        //
        //     if (savedCharacter.unlockedAbilities == "0000")
        //     {
        //         UpdateCommonData();
        //     }
        // }
    }
    
    public void EnableAbilities()
    {
        int currentButtonIndex = 2;
        int buttonIndex = 0;
        GameObject character = _buttonManager.CharacterOnBoard;
        PlayerInformation playerInformation = character.GetComponent<PlayerInformation>();
        if (character != null)
        {
            for (int i = 0; i < playerInformation.enabledAbilitiesEnemy.Count; i++)
            {
                if (_buttonManager.ButtonList.Count > currentButtonIndex && character.GetComponent<ActionManager>().FindActionListByName(playerInformation.enabledAbilitiesEnemy[i]) != null)
                {
                    UpdateData(buttonIndex, i);
                    buttonAction[buttonIndex].buttonState = character.GetComponent<ActionManager>().FindActionListByName(playerInformation.enabledAbilitiesEnemy[i]).actionName;
                    // _buttonManager.ButtonList[currentButtonIndex].transform.Find("ActionButtonImage").GetComponent<Image>().sprite = character.GetComponent<ActionManager>().FindActionListByName(playerInformation.enabledAbilitiesEnemy[i]).AbilityIcon;
                    // _buttonManager.ButtonList[currentButtonIndex].GetComponent<ActionButton>().buttonState = 
                    currentButtonIndex++;
                    buttonIndex++;
                }
            }
            Debug.LogError("Need to change this");
            if (playerInformation.enabledAbilitiesEnemy.Count == 0)
            {
                UpdateCommonData();
            }
            for (int i = currentButtonIndex; i < _buttonManager.ButtonList.Count; i++)
            {
                _buttonManager.ButtonList[i].transform.parent.gameObject.SetActive(false);
                // string extensionName = "Extension" + (i + 1).ToString();
                // transform.Find("CornerUI").Find(extensionName).gameObject.SetActive(false);
            }
            
        }
    }
    
    public void ChangeCooldownVisuals()
    {
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < _buttonManager.ButtonList.Count; i++)
            {
                if (_buttonManager.ButtonList[i].GetComponent<ActionButton>().buttonState != "Movement" &&
                    _buttonManager.CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(_buttonManager.ButtonList[i].GetComponent<ActionButton>().buttonState) != null)
                {
                    var buttonActionScript = _buttonManager.CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(_buttonManager.ButtonList[i].GetComponent<ActionButton>().buttonState);
                    if (buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown || buttonActionScript.AvailableAttacks == 0)
                    {
                        // _buttonManager.ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = Color.gray;
                        _buttonManager.ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor - new Color(0.2f, 0.2f, 0.2f, 0f);
                        for(int j = 0; j < _buttonManager.ButtonList[i].transform.childCount; j++)
                        {
                            if(_buttonManager.ButtonList[i].transform.GetChild(j).name == "Text" && buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown)
                            {
                                var image = _buttonManager.ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
                            }
                        }
                    }
                    else
                    {
                        _buttonManager.ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor;
                        if (_buttonManager.ButtonList[i].transform.Find("Text") != null)
                        {
                            var image = _buttonManager.ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                        }
                    }
                    Debug.Log("Need to redo this some time");
                }

                else if (_buttonManager.ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement")
                {
                    var characterMovementScript = _buttonManager.CharacterOnBoard.GetComponent<GridMovement>();
                    if (characterMovementScript.AvailableMovementPoints == 0)
                    {
                        _buttonManager.ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor - new Color(0.2f, 0.2f, 0.2f, 0f);
                        // if (ButtonList[i].transform.Find("Text") != null
                        //     && (ButtonList[i].transform.Find("Text").GetComponent<CooldownText>().action.AbilityCooldown - ButtonList[i].transform.Find("Text").GetComponent<CooldownText>().action.AbilityPoints) > 0)
                        // {
                        var image = _buttonManager.ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
                        // }
                    }
                    else
                    {
                        _buttonManager.ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor;
                        // if (ButtonList[i].transform.Find("Text") != null)
                        // {
                        var image = _buttonManager.ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                        // }
                    }
                }
            }
        }
    }
    
    
}
