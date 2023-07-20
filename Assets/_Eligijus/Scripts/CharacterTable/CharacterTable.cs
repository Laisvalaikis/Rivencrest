using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterTable : MonoBehaviour
{
    [SerializeField] private Image tableBoarder;
    [SerializeField] private Image characterArt;
    [SerializeField] private TextMeshProUGUI className;
    [SerializeField] private TextMeshProUGUI role;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI maxHP;
    [SerializeField] private TextMeshProUGUI xpProgress;
    [SerializeField] private TextMeshProUGUI abilityPointCount;
    [SerializeField] private TextMeshProUGUI blessingList;
    [SerializeField] private PortraitBar portraitBar;
    [SerializeField] private GameObject confirmationTable;
    [SerializeField] private TextMeshProUGUI confirmationTableText;
    [SerializeField] private Image confirmationTableSprite;
    [SerializeField] private GameObject confirm;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [HideInInspector] public string originalName;
    [SerializeField] private Button sellButton;
    [SerializeField] private List<Button> abilityButtons;
    [SerializeField] private List<Image> abilityButtonImages;
    [SerializeField] private List<Image> abilityButtonIconImages;
    [SerializeField] private Image roleIcon;
    public View view;
    public TMP_InputField nameInput;
    [SerializeField] private GameUi gameUI;
    public HelpTable helpTable;
    public GameObject recruitmentCenterTable;
    public TownHall townHall;
    private List<bool> abilityButtonState;
    private int characterIndex;
    private Data _data;
    private bool pauseEnabled = false;
    
    private void Awake()
    {
    }

    private void OnEnable()
    {
        // ResetTempUnlockedAbilities();
        if (_data == null && Data.Instance != null)
        {
            _data = Data.Instance;
            if (abilityButtonState == null || abilityButtonState.Count == 0)
            {
                abilityButtonState = new List<bool>();
                for (int i = 0; i < abilityButtons.Count; i++)
                {
                    abilityButtonState.Add(false);
                }
            }
        }
    }

    public int GetCurrentCharacterIndex()
    {
        return characterIndex;
    }

    private void OnDisable()
    {
    }

    public void DisableAllButtons()
    {
        pauseEnabled = true;
        if (abilityButtonState == null)
        {
            abilityButtonState = new List<bool>();
        }
        else if (abilityButtonState.Count > 0)
        {
            for (int i = 0; i < abilityButtons.Count; i++)
            {
                abilityButtonState[i] = abilityButtons[i].interactable;
                abilityButtons[i].interactable = false;
            }
        }
    }

    public void EnableAllButtons()
    {
        pauseEnabled = false;
        if (abilityButtonState.Count > 0)
        {
            for (int i = 0; i < abilityButtons.Count; i++)
            {
                abilityButtons[i].interactable = abilityButtonState[i];
            }
        }
    }

    private void UpdateCharacterPointCorner()
    {
        List<SavedCharacter> charactersToUpdate = _data.Characters;
        if (charactersToUpdate[characterIndex].abilityPointCount <= 0)
        {
            portraitBar.DisableAbilityCorner(characterIndex);
        }
        else
        {
            portraitBar.EnableAbilityCorner(characterIndex);
        }

        if (gameUI != null)
        {
            gameUI.UpdateUnspentPointWarnings();
        }
    }

    public void ConfirmSelectedAbilities()
    {
        _data.Characters[characterIndex].toConfirmAbilities = 0;
        List<UnlockedAbilities> unlockedAbilityList = _data.Characters[characterIndex].unlockedAbilities;
        for (int i = 0; i < unlockedAbilityList.Count; i++)
        {
            if (!unlockedAbilityList[i].abilityConfirmed && unlockedAbilityList[i].abilityUnlocked)
            {
                _data.Characters[characterIndex].unlockedAbilities[i].abilityConfirmed = true;
            }
        }
        confirm.SetActive(false);
        UpdateCharacterPointCorner();
    }

    public void ChangeCharacterName()
    {
        nameInput.text = nameInput.text.ToUpper();
        _data.Characters[characterIndex].characterName = nameInput.text;
    }

    public void PreventEmptyName()
    {
        if (nameInput.text.Contains(' '))
        {
            _data.Characters[characterIndex].characterName = originalName;
            nameInput.text = originalName;
        }
        if (nameInput.text == "")
        {
            _data.Characters[characterIndex].characterName = originalName;
            nameInput.text = originalName;
        }
    }
    
    public void UpdateConfirmationTable()
    {
        var character = _data.Characters[characterIndex];
        Color color = character.playerInformation.classColor; // fix this shit
        confirmationTableText.text = $"Are you sure you want to sell <color=#{ColorUtility.ToHtmlStringRGBA(color)}>{character.characterName}</color> for <color=yellow>{character.cost / 2}</color> gold?";
        confirmationTableSprite.sprite = character.playerInformation.CharacterPortraitSprite;
    }
    
    public void UpdateAllAbilities()
    {
        var character = _data.Characters[characterIndex];
        for (int i = 0; i < abilityButtons.Count; i++)
        {
            if (character.abilityPointCount > 0 || !_data.Characters[characterIndex].unlockedAbilities[i].abilityConfirmed && _data.Characters[characterIndex].unlockedAbilities[i].abilityUnlocked)
            {
                abilityButtons[i].interactable = true;
            }
            else
            {
                abilityButtons[i].interactable = false;
            }

            if (!character.unlockedAbilities[i].abilityUnlocked)
            {
                Color color = abilityButtonImages[i].color - new Color(0.2f, 0.2f, 0.2f, 0f);
                abilityButtonImages[i].color = color;
            }
            else
            {
                abilityButtonImages[i].color = character.playerInformation.backgroundColor;
            }
        }
    }


    public void DisplayCharacterTable(int index)
    {
        // gameObject.SetActive(true);
        view.OpenView();
        int tempIndex = characterIndex;
        characterIndex = index;
        if (index != tempIndex)
        {
            helpTable.gameObject.SetActive(false);
            UndoAbilitySelection(tempIndex);
            UpdateTable();
            UpdateAllAbilities();
        }

        if (recruitmentCenterTable != null && townHall != null)
        {
            recruitmentCenterTable.SetActive(false);
            townHall.CloseTownHall();
        }
        else
        {
            Debug.Log("TownHall is null");
        }
        
        
    }

    public void EnableDisableHelpTable(int index)
    {
        if (!pauseEnabled)
        {
            Transform helpTableTransform = helpTable.transform;
            Vector3 currentPosition = helpTableTransform.position;
            Vector3 position = new Vector3(currentPosition.x, abilityButtons[index].transform.position.y,
                currentPosition.z);
            helpTableTransform.SetPositionAndRotation(position, helpTableTransform.rotation);
            helpTable.EnableTableForBoughtCharacters(index, characterIndex);
        }
    }
    
    public void UndoAbilitySelection(int selectedCharacterIndex)
    {
        List<UnlockedAbilities> unlockedAbilityList = _data.Characters[selectedCharacterIndex].unlockedAbilities;
        for (int i = 0; i < unlockedAbilityList.Count; i++)
        {
            if (!unlockedAbilityList[i].abilityConfirmed && unlockedAbilityList[i].abilityUnlocked)
            {
                RemoveAbility(i, selectedCharacterIndex);
            }
        }
        UpdateTable();
        UpdateAllAbilities();
    }
    
    public void ExitTable()
    {
        UndoAbilitySelection(characterIndex);
        view.ExitView();
    }
    
    public void OnLeftArrowClick()
    {
        UndoAbilitySelection(characterIndex);
        int newCharacterIndex = Mathf.Clamp(characterIndex - 1, 0, _data.Characters.Count - 1);
        DisplayCharacterTable(newCharacterIndex);
        UpdateTable();
        UpdateAllAbilities();
        portraitBar.ScrollDownByCharacterIndex(newCharacterIndex);
    }

    public void OnRightArrowClick()
    {
        UndoAbilitySelection(characterIndex);
        int newCharacterIndex = Mathf.Clamp(characterIndex + 1, 0, _data.Characters.Count - 1);
        DisplayCharacterTable(newCharacterIndex);
        UpdateTable();
        UpdateAllAbilities();
        portraitBar.ScrollUpByCharacterIndex(newCharacterIndex);
    }

    // until this everything is fixed
    
    public void SellCharacter()
    {
        int cost = _data.Characters[characterIndex].cost;
        _data.townData.townGold += cost / 2;
        gameUI.EnableGoldChange("+" + cost / 2 + "g");
        gameUI.UpdateTownCost();
        _data.Characters.RemoveAt(characterIndex);
        portraitBar.RemoveCharacter(characterIndex);
        view.ExitView();
        UpdateTable();
        if (characterIndex > _data.Characters.Count - 1)
        {
            characterIndex = _data.Characters.Count - 1;
        }
    }


    public void UpdateTable()
    {
        if (_data.Characters.Count > 0 && characterIndex < _data.Characters.Count && characterIndex >= 0)
        {
            UpdateTableInformation();
            UpdateConfirmationTable();
        }
        if (gameObject.activeInHierarchy)
        {
            HighlightCurrentCharacterInPortraitBar();
        }
        else
        {
            RemoveHighlightsFromPortraitBar();
        }
        confirmationTable.SetActive(false);
        confirm.SetActive(_data.Characters[characterIndex].toConfirmAbilities > 0);
        leftArrow.gameObject.SetActive(characterIndex > 0);
        rightArrow.gameObject.SetActive(characterIndex < _data.Characters.Count - 1);
        if (_data.Characters.Count > 3)
        {
            sellButton.interactable = true;
        }
        else
        {
            sellButton.interactable = false;
        }
    }
    

    private void RemoveHighlightsFromPortraitBar()
    {
        // for (int i = 0; i < portraitBar.buttonOnHover.Count; i++)
        // {
        //     if (portraitBar.townPortraits[i].gameObject.activeInHierarchy)
        //     {
        //         portraitBar.buttonOnHover[i].SetBool("select", false);
        //     }
        // }
    }

    private void HighlightCurrentCharacterInPortraitBar()
    {

        // for (int i = 0; i < portraitBar.buttonOnHover.Count; i++)
        // {
        //     if (portraitBar.townPortraits[i].gameObject.activeInHierarchy)
        //     {
        //         if (portraitBar.townPortraits[i].characterIndex == characterIndex)
        //         {
        //             portraitBar.buttonOnHover[i].SetBool("select", true);
        //         }
        //         else
        //         {
        //             portraitBar.buttonOnHover[i].SetBool("select", false);
        //         }
        //     }
        // }
    }
    

    public void UpdateTableInformation()
    {
        SavedCharacter character = _data.Characters[characterIndex];
        originalName = character.characterName;
        PlayerInformationData data = character.playerInformation;
        Color color = data.classColor;
        tableBoarder.color = data.secondClassColor;
        nameInput.text = character.characterName;
        className.text = data.ClassName;
        className.color = color;
        role.text = data.role;
        role.color = color;
        roleIcon.sprite = data.roleSprite;
        level.text = "LEVEL: " + character.level;
        level.color = color;
        maxHP.text = "MAX HP: " + CalculateMaxHP(character);
        maxHP.color = color;
        xpProgress.text = (character.level >= GameProgress.currentMaxLevel()) ? "MAX LEVEL" : character.xP + "/" + _data.XPToLevelUp[character.level - 1] + " XP";
        xpProgress.color = color;
        abilityPointCount.text = character.abilityPointCount.ToString();
        abilityPointCount.color = color;
        characterArt.sprite = data.CharacterSplashArt;
        blessingList.text = character.CharacterTableBlessingString();
        //
        for (int i = 0; i < abilityButtonImages.Count; i++)
        {
            abilityButtons[i].gameObject.SetActive(true);
            abilityButtonIconImages[i].color = character.playerInformation.classColor;
            abilityButtonIconImages[i].sprite = character.playerInformation.abilities[i].sprite;
            abilityButtonImages[i].color = character.playerInformation.backgroundColor;
            
        }
    }


    private int CalculateMaxHP(SavedCharacter character)
    {
        int maxHP = character.playerInformation.MaxHealth; // fix this
        maxHP += (character.level - 1) * 2;
        if (character.blessings.Find(x => x.blessingName == "Healthy") != null)
        {
            maxHP += 3;
        }
        return maxHP;
    }
    private void UpgradeAbility(int abilityIndex)
    {
        _data.Characters[characterIndex].unlockedAbilities[abilityIndex].abilityUnlocked = true;
        _data.Characters[characterIndex].abilityPointCount--;
        _data.Characters[characterIndex].toConfirmAbilities++;
        UpdateTable();
        UpdateAllAbilities();
    }
    private void RemoveAbility(int abilityIndex, int selectedCharacterIndex)
    {
        _data.Characters[selectedCharacterIndex].unlockedAbilities[abilityIndex].abilityUnlocked = false;
        _data.Characters[selectedCharacterIndex].toConfirmAbilities--;
        _data.Characters[selectedCharacterIndex].abilityPointCount++;
    }

    public void AddRemoveAbility(int index)
    {
        if (!_data.Characters[characterIndex].unlockedAbilities[index].abilityUnlocked)
        {
            UpgradeAbility(index);
        }
        else
        {
            RemoveAbility(index, characterIndex);
            UpdateTable();
            UpdateAllAbilities();
        }
    }



}
