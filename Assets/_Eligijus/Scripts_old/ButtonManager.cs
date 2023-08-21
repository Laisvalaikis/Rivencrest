using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private BottomCornerUI bottomCornerUI;
    private Image image;
    private Animator animator;
    public ActionButton actionButton;
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
        _actionButtons = new List<ActionButton>();
        Debug.Log("Need To update action buttons");
        ButtonFrameList = new List<GameObject>();
        ButtonIconList = new List<GameObject>();
        for (int i = 0; i < ButtonList.Count; i++)
        { 
            //ButtonFrameList.Add(ButtonList[i].transform.Find("ActionButtonFrame").gameObject);
           // ButtonIconList.Add(ButtonList[i].transform.Find("ActionButtonImage").gameObject);
           ButtonFrameList.Add(ButtonList[i].transform.Find("ActionButtonFrame").gameObject);
           ButtonIconList.Add(ButtonList[i].transform.Find("ActionButtonImage").gameObject);
           // ButtonIconList[i].GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
           ButtonIconList[i].GetComponent<Image>().color = bottomCornerUI.ButtonIconColor;
            _actionButtons.Add(ButtonList[i].GetComponent<ActionButton>());
            var CantAttackIcon = ButtonList[i].transform.Find("CantAttackImage");
            if (CantAttackIcon != null)
            {
               // CantAttackIcon.gameObject.GetComponent<Image>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
               CantAttackIcon.gameObject.GetComponent<Image>().color = bottomCornerUI.ButtonIconColor;
            }
        }
        // transform.GetChild(0).Find("MovementTextBackground").GetChild(0).gameObject.GetComponent<Text>().color = GetComponent<BottomCornerUI>().ButtonIconColor;
    }
    void Start()
    {
        
        //if (transform.Find("CornerUI").Find("DebuffIcons") != null)
        if(debuffManager!=null)
        {
            //transform.Find("CornerUI").Find("DebuffIcons").gameObject.GetComponent<DebuffManager>().CharacterOnBoard = CharacterOnBoard;
            debuffManager.CharacterOnBoard = CharacterOnBoard;
        }
    }
    void Update()
    {
        if(transform.GetChild(0).gameObject.activeSelf)
        {
            for(int i = 0; i < ButtonList.Count; i++)
            {
                if (Input.GetKeyDown(AbilityChangingButtonSequence[i]))
                {
                    ButtonList[i].GetComponent<ActionButton>().ChangePlayersState();
                    
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
            var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            var actionButtonImage = ButtonList[i].transform.Find("ActionButtonImage").GetComponent<Image>();
            var actionButtonImageBackground = ButtonList[i].transform.Find("ActionButtonBackground").GetComponent<Image>();
            if ((buttonAction != null && buttonAction.isDisabled)
                || (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement" && CharacterOnBoard.GetComponent<GridMovement>().isDisabled))
            {
                ButtonList[i].transform.Find("CantAttackImage").gameObject.SetActive(true);
                //
                actionButtonImage.color = new Color(actionButtonImage.color.r, actionButtonImage.color.g, actionButtonImage.color.b, 0.1f);
                actionButtonImageBackground.color = Color.gray;
            }
            else if ((buttonAction != null && buttonAction.canGridBeEnabled())
                || (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement" && CharacterOnBoard.GetComponent<GridMovement>().canGridBeEnabled()))
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
            var buttonAction = CharacterOnBoard.GetComponent<ActionManager>().FindActionByName(ButtonList[i].GetComponent<ActionButton>().buttonState);
            // veiksmai, ne movement
            if (buttonAction != null && 
                (CharacterOnBoard.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
                || (buttonAction.AttackAbility && CharacterOnBoard.GetComponent<PlayerInformation>().CantAttackCondition)
                || (CharacterOnBoard.GetComponent<PlayerInformation>().Silenced && !(buttonAction is PlayerAttack))
                || (CharacterOnBoard.GetComponent<PlayerInformation>().Stasis)))
            {

                buttonAction.isDisabled = true;
            }
            else if (buttonAction != null)
            {
                buttonAction.isDisabled = false;

            }
            // movement
            if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement"
                && (CharacterOnBoard.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
                || CharacterOnBoard.GetComponent<PlayerInformation>().CantMove)
                || CharacterOnBoard.GetComponent<PlayerInformation>().Stasis)
            {
                CharacterOnBoard.GetComponent<GridMovement>().isDisabled = true;
            }
            else if (ButtonList[i].GetComponent<ActionButton>().buttonState == "Movement")
            {
                CharacterOnBoard.GetComponent<GridMovement>().isDisabled = false;
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
