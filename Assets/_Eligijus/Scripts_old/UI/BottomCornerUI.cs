using System;
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
    private PlayerInformationData _currentPlayerInformationData;
    private GridMovement characterOnBoardGridMovement;
    private ActionButton actionButtonList;
    [SerializeField] private List<Image> actionButtonFrame;
    private PlayerInformation playerInformationCurrentCharacter;
    private PlayerInformation playerInformationCharacterOnBoard;
    private ActionManager actionManagerCharacterOnBoard;
    private GridMovement gridMovementCharacterOnBoard;
    
    private Image image;
    private Animator animator;
    public DebuffManager debuffManager;
    public List<GameObject> ButtonList;
    [HideInInspector] public List<GameObject> ButtonFrameList;
    private List<GameObject> ButtonIconList;
    private GameObject _characterOnBorad;
    [HideInInspector] public GameObject CharacterOnBoard {
        get { return _characterOnBorad; }
        set { _characterOnBorad = value; }
    }
    private KeyCode[] AbilityChangingButtonSequence = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y };
    private List<ActionButton> _actionButtons;
    [SerializeField] private ActionButton movementButton;
    [SerializeField] private Animator movementButtonFrame;
    private Animator _selectedButton;
    void Awake()
    {
        // GameInformation = GameObject.Find("GameInformation").gameObject;
        // movementText = transform.GetChild(0).Find("MovementTextBackground").GetChild(0).gameObject;
        // healthText = transform.GetChild(0).Find("HealthBar").Find("HealthText").gameObject;

        for (int i = 0; i < ButtonList.Count; i++)
        {
            ActionButton actionButtonList = ButtonList[i].GetComponent<ActionButton>();
        }
        _actionButtons = new List<ActionButton>();
        Debug.Log("Need To update action buttons");
        // ButtonFrameList = new List<GameObject>();
        // ButtonIconList = new List<GameObject>();
        for (int i = 0; i < ButtonList.Count; i++)
        { 
            //ButtonFrameList.Add(ButtonList[i].transform.Find("ActionButtonFrame").gameObject);
            // ButtonIconList.Add(ButtonList[i].transform.Find("ActionButtonImage").gameObject);
            // ButtonFrameList.Add(actionButtonFrame[i].transform.gameObject);
            // ButtonIconList.Add(actionButtonImage[i].transform.gameObject);
            // ButtonIconList[i].GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
            // ButtonIconList[i].GetComponent<Image>().color = ButtonIconColor;
           // _actionButtons.Add(ButtonList[i].GetComponent<ActionButton>());
            _actionButtons.Add(actionButtonList);
            var CantAttackIcon = ButtonList[i].transform.Find("CantAttackImage");
            if (CantAttackIcon != null)
            {
                // CantAttackIcon.gameObject.GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
                CantAttackIcon.gameObject.GetComponent<Image>().color = ButtonIconColor;
            }
        }
        // transform.GetChild(0).Find("MovementTextBackground").GetChild(0).gameObject.GetComponent<Text>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
    }
    //Start nebuvo cia ButtonManager start
    void Start()
    {
        GridMovement characterOnBoardGridMovement = CharacterOnBoard.GetComponent<GridMovement>();
        PlayerInformation playerInformationCurrentCharacter = currentCharacter.GetComponent<PlayerInformation>();
        PlayerInformation playerInformationCharacterOnBoard = CharacterOnBoard.GetComponent<PlayerInformation>();
        ActionManager actionManagerCharacterOnBoard = CharacterOnBoard.GetComponent<ActionManager>();
        //if (transform.Find("CornerUI").Find("DebuffIcons") != null)
        if(debuffManager!=null)
        {
            //transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().CharacterOnBoard = CharacterOnBoard;
            debuffManager.CharacterOnBoard = CharacterOnBoard;
        }
    }

    void Update()
    {
       //ChangesInCornerUI();
       
       
       
       if(transform.GetChild(0).gameObject.activeSelf)
       {
           for(int i = 0; i < ButtonList.Count; i++)
           {
               if (Input.GetKeyDown(AbilityChangingButtonSequence[i]))
               {
                   //ButtonList[i].GetComponent<ActionButton>().ChangePlayersState();
                   actionButtonList.ChangePlayersState();
                    
               }
           }
       }
    }

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
        if (CharacterOnBoard != null)
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
        if (CharacterOnBoard != null)
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
        currentCharacter = CharacterOnBoard;
        staminaPoints.text = currentCharacter.GetComponent<GridMovement>().AvailableMovementPoints.ToString(); //Sets movement text playerInformationCurrentCharacter
        //healthPoints.text = currentCharacter.GetComponent<PlayerInformation>().health.ToString(); //Sets health text
       // healthAnimator.SetFloat("healthPercent", currentCharacter.GetComponent<PlayerInformation>().GetHealthPercentage());
        healthPoints.text = playerInformationCurrentCharacter.health.ToString(); //Sets health text
        healthAnimator.SetFloat("healthPercent", playerInformationCurrentCharacter.GetHealthPercentage());
        SelectMovementButton();
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
        GameObject character = CharacterOnBoard;
        PlayerInformation playerInformation = character.GetComponent<PlayerInformation>();
        if (character != null)
        {
            for (int i = 0; i < playerInformation.enabledAbilitiesEnemy.Count; i++)
            {
                if (ButtonList.Count > currentButtonIndex && character.GetComponent<ActionManager>().FindActionListByName(playerInformation.enabledAbilitiesEnemy[i]) != null)
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
            for (int i = currentButtonIndex; i < ButtonList.Count; i++)
            {
                ButtonList[i].transform.parent.gameObject.SetActive(false);
                // string extensionName = "Extension" + (i + 1).ToString();
                // transform.Find("CornerUI").Find(extensionName).gameObject.SetActive(false);
            }
            
        }
    }
    
    public void ChangeCooldownVisuals()
    {
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < ButtonList.Count; i++)
            {
               // if (ButtonList[i].GetComponent<ActionButton>().buttonState != "Movement" &&
                 //   CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState) != null) 
                 if (actionButtonList.buttonState != "Movement" && actionManagerCharacterOnBoard.FindActionByName(actionButtonList.buttonState) != null)
                {
                   // var buttonActionScript = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
                   var buttonActionScript = actionManagerCharacterOnBoard.FindActionByName(actionButtonList.buttonState);
                    if (buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown || buttonActionScript.AvailableAttacks == 0)
                    {
                        // _buttonManager.ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = Color.gray;
                        ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor - new Color(0.2f, 0.2f, 0.2f, 0f);
                        for(int j = 0; j < ButtonList[i].transform.childCount; j++)
                        {
                            if(ButtonList[i].transform.GetChild(j).name == "Text" && buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown)
                            {
                                var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
                            }
                        }
                    }
                    else
                    {
                       ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor;
                        if (ButtonList[i].transform.Find("Text") != null)
                        {
                            var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                        }
                    }
                    Debug.Log("Need to redo this some time");
                }

               // else if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement")
                 else if (actionButtonList.buttonState == "Movement")
                {
                    //var characterMovementScript = CharacterOnBoard.GetComponent<GridMovement>();
                    var characterMovementScript = characterOnBoardGridMovement;
                    if (characterMovementScript.AvailableMovementPoints == 0)
                    {
                        ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor - new Color(0.2f, 0.2f, 0.2f, 0f);
                        // if (ButtonList[i].transform.Find("Text") != null
                        //     && (ButtonList[i].transform.Find("Text").GetComponent<CooldownText>().action.AbilityCooldown - ButtonList[i].transform.Find("Text").GetComponent<CooldownText>().action.AbilityPoints) > 0)
                        // {
                       // var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                       
                          // var image = actionButtonImage[i].GetComponent<Image>();
                        // image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
                        
                        // }
                    }
                    else
                    {
                        ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>().color = _currentPlayerInformationData.backgroundColor;
                        // if (ButtonList[i].transform.Find("Text") != null)
                        // {
                        var image = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                        // }
                    }
                }
            }
        }
    }
    
    
    
    
    
    
     public void AddDataToActionButtons(HelpTable helpTable)
    {
        for (int i = 0; i < _actionButtons.Count; i++)
        {
            _actionButtons[i]._helpTable = helpTable;
        }
    }

    public void DisableSelection(GameObject selected)
    {
        if (_selectedButton != null)
        {
            _selectedButton.SetBool("select", false);
            //_selectedButton = selected.GetComponent<Animator>();
            _selectedButton = animator;
            _selectedButton.SetBool("select", true);
        }
        else
        {
            //_selectedButton = selected.GetComponent<Animator>();
            _selectedButton = animator;
            _selectedButton.SetBool("select", true);
        }

        // for (int i = 0; i < ButtonFrameList.Count; i++)
        // {
        //     if (ButtonFrameList[i] != selected)
        //     {
        //         ButtonFrameList[i].GetComponent<Animator>().SetBool("select", false);
        //     }
        // }
        // selected.GetComponent<Animator>().SetBool("select", true);
    }

    public void SelectMovementButton()
    {
        if (_selectedButton != null)
        {
            _selectedButton.SetBool("select", false);
            _selectedButton = movementButtonFrame;
            _selectedButton.SetBool("select", true);
        }
        else
        {
            _selectedButton = movementButtonFrame;
            _selectedButton.SetBool("select", true);
        }
    }

    /*void Update()
    {
        //if (GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectedCharacter == CharacterOnBoard)
    }*/
    
    public void ChangesInCornerUIButtons()// This changes debuff icons and cantattack icons
    {
        if (transform.Find("CornerUI").Find("DebuffIcons") != null)
        {
           // transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().CharacterOnBoard = CharacterOnBoard;
           // transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().UpdateDebuffs();
           debuffManager.CharacterOnBoard = CharacterOnBoard;
           debuffManager.UpdateDebuffs();
        }

        //
        for (int i = 0; i < ButtonList.Count; i++)
        {
            /*var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            if (buttonAction != null && buttonAction.AttackAbility)
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(CharacterOnBoard.GetComponent<PlayerInformation>().CantAttackCondition);
                var actionButtonImage = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
                var actionButtonImageBackground = ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>();
                var buttonActionScript = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
                if (CharacterOnBoard.GetComponent<PlayerInformation>().CantAttackCondition)
                {
                    actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 0.1f);
                    actionButtonImageBackground.color = Color.gray;
                }
                else if (!(buttonActionScript.AbilityPoints < buttonActionScript.AbilityCooldown || buttonActionScript.AvailableAttacks == 0))
                {
                    actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 1f);
                    actionButtonImageBackground.color = Color.white;
                }
                //ButtonList[i].transform.Find("CantAttackImage").GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
            }*/
            ChangeAbilityDisabledConditions();
            //
            //var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            var buttonAction = actionManagerCharacterOnBoard.FindActionByName(actionButtonList.buttonState);
            var actionButtonImage = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
            var actionButtonImageBackground = ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>();
            //if ((buttonAction != null && buttonAction.isDisabled)
               // || (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement" && CharacterOnBoard.GetComponent<GridMovement>().isDisabled))
               if ((buttonAction != null && buttonAction.isDisabled)
                || (actionButtonList.buttonState == "Movement" && characterOnBoardGridMovement.isDisabled))
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(true);
                //
                actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 0.1f);
                actionButtonImageBackground.color = Color.gray;
            }
            //else if ((buttonAction != null && buttonAction.canGridBeEnabled())
                // || (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement" && CharacterOnBoard.GetComponent<GridMovement>().canGridBeEnabled()))
               else if ((buttonAction != null && buttonAction.canGridBeEnabled())
               || (actionButtonList.buttonState == "Movement" && characterOnBoardGridMovement.canGridBeEnabled()))
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(false);
                actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 1f);
                actionButtonImageBackground.color = Color.white;
            }
            else
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(false);
            }
        }
    }

    public void UpdateDebuffIcons()
    {
        if (transform.Find("CornerUI").Find("DebuffIcons") != null)
        {
           // transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().CharacterOnBoard = CharacterOnBoard;
           // transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().UpdateDebuffs();
           debuffManager.CharacterOnBoard = CharacterOnBoard;
           debuffManager.UpdateDebuffs();
        }
    }

    public void ChangeAbilityDisabledConditions()
    {
        for (int i = 0; i < ButtonList.Count; i++)
        {
            //var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            var buttonAction = actionManagerCharacterOnBoard.FindActionByName(actionButtonList.buttonState);
            // veiksmai, ne movement
            //if (buttonAction != null && 
               // (CharacterOnBoard.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
              //  || (buttonAction.AttackAbility && CharacterOnBoard.GetComponent<PlayerInformation>().CantAttackCondition)
               // || (CharacterOnBoard.GetComponent<PlayerInformation>().Silenced && !(buttonAction is PlayerAttack))
              //  || (CharacterOnBoard.GetComponent<PlayerInformation>().Stasis)))
              if (buttonAction != null && 
                  (playerInformationCharacterOnBoard.Debuffs.Contains("Stun")
                   || (buttonAction.AttackAbility && playerInformationCharacterOnBoard.CantAttackCondition)
                   || (playerInformationCharacterOnBoard.Silenced && !(buttonAction is PlayerAttack))
                   || (playerInformationCharacterOnBoard.Stasis)))
            {
                buttonAction.isDisabled = true;
            }
            else if (buttonAction != null)
            {
                buttonAction.isDisabled = false;

            }
            // movement
           // if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement"
                //&& (CharacterOnBoard.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
                //|| CharacterOnBoard.GetComponent<PlayerInformation>().CantMove)
               // || CharacterOnBoard.GetComponent<PlayerInformation>().Stasis)
               
               //if (actionButtonList.buttonState == "Movement"
                 //  && (CharacterOnBoard.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
                 //      || CharacterOnBoard.GetComponent<PlayerInformation>().CantMove)
                //   || CharacterOnBoard.GetComponent<PlayerInformation>().Stasis)
                if (actionButtonList.buttonState == "Movement"
                    && (playerInformationCharacterOnBoard.Debuffs.Contains("Stun")
                        || playerInformationCharacterOnBoard.CantMove)
                    || playerInformationCharacterOnBoard.Stasis)
            {
                //CharacterOnBoard.GetComponent<GridMovement>().isDisabled = true;
                characterOnBoardGridMovement.isDisabled = true;
            }
           // else if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement")
               else if (actionButtonList.buttonState == "Movement")
            {
                //CharacterOnBoard.GetComponent<GridMovement>().isDisabled = false;
                characterOnBoardGridMovement.isDisabled = false;
            }
        }
    }
    public void GenerateAbilities()
    {
        // int currentButtonIndex = 2;
        // SavedCharacter character = CharacterOnBoard.GetComponent<PlayerInformation>().savedCharacter;
        // if (character != null)
        // {
        //     for (int i = 0; i < character.unlockedAbilities.Length; i++)
        //     {
        //         if (ButtonList.Count > currentButtonIndex)
        //         {
        //             if (character.prefab.GetComponent<ActionManager>().FindActionByIndex(i) != null && character.unlockedAbilities[i] == '1')
        //             {
        //                 ButtonList[currentButtonIndex].transform.parent.gameObject.SetActive(true);
        //                 ButtonList[currentButtonIndex].transform.Find("ActionButtonImage").GetComponent<Image>().sprite = character.prefab.GetComponent<ActionManager>().FindActionByIndex(i).AbilityIcon;
        //                 ButtonList[currentButtonIndex].GetComponent<ActionButton>().buttonState = character.prefab.GetComponent<ActionManager>().FindActionByIndex(i).actionName;
        //                 currentButtonIndex++;
        //             }
        //         }
        //     }
        // }
    }

    public void GenerateAbilitiesForEnemy(List<string> abilitiesToEnable)
    {
        int currentButtonIndex = 2;
        GameObject character = CharacterOnBoard;
        if (character != null)
        {
            foreach(string ability in abilitiesToEnable)
            {
                if (ButtonList.Count > currentButtonIndex && character.GetComponent<ActionManager>().FindActionListByName(ability) != null)
                {
                    ButtonList[currentButtonIndex].transform.parent.gameObject.SetActive(false);
                    ButtonList[currentButtonIndex].transform.Find("ActionButtonImage").GetComponent<Image>().sprite = character.GetComponent<ActionManager>().FindActionListByName(ability).AbilityIcon;
                    ButtonList[currentButtonIndex].GetComponent<ActionButton>().buttonState = character.GetComponent<ActionManager>().FindActionListByName(ability).actionName;
                    currentButtonIndex++;
                }
            }
            for (int i = currentButtonIndex; i < ButtonList.Count; i++)
            {
                ButtonList[i].transform.parent.gameObject.SetActive(false);
                string extensionName = "Extension" + (i + 1).ToString();
                transform.Find("CornerUI").Find(extensionName).gameObject.SetActive(false);
            }
        }
    }
    
    
}
