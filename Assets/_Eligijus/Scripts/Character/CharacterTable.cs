﻿using System.Collections;
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
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject confirmationTable;
    [SerializeField] private GameObject undo;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [HideInInspector] public int characterIndex;
    [HideInInspector] public string originalName;
    public TMP_InputField nameInput;
    [SerializeField] private Button sellButton;
    private List<int> tempUnlockedAbilities = new List<int>();
    public Data _data;
    public GameUi gameUI;
    public HelpTable helpTable;
    public HelpTableController helpTableController;
    public GameObject recruitmentCenterTable;
    public TownHall townHall;
    private void Awake()
    {
        ResetTempUnlockedAbilities();
    }

    private void OnEnable()
    {
        ResetTempUnlockedAbilities();
    }

    private void OnDisable()
    {

        gameUI.UpdateUnspentPointWarnings();
        UpdateCharacterPointCorner();
        ResetTempUnlockedAbilities();
    }

    private void UpdateCharacterPointCorner()
    {
        List<SavedCharacter> charactersToUpdate = _data.AllAvailableCharacters;
        if (charactersToUpdate[characterIndex].abilityPointCount < 0)
        {
            portraitBar.townPortraits[characterIndex].abilityPointCorner.SetActive(false);
        }
        else
        {
            portraitBar.townPortraits[characterIndex].abilityPointCorner.SetActive(true);
        }
    }

    public void ResetTempUnlockedAbilities()
    {
        tempUnlockedAbilities = new List<int>();
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

    public void SellCharacter()
    {
        int cost = _data.AllAvailableCharacters.Find(x => x.prefab == _data.Characters[characterIndex].prefab).cost;
        _data.townData.townGold += cost / 2;
        gameUI.EnableGoldChange("+" + cost / 2 + "g");
        characterController.SellCharacter(characterIndex);
        UpdateTable();
    }

    public void UpdateTable()
    {
        if (_data.Characters.Count > 0 && characterIndex < _data.Characters.Count && characterIndex >= 0)
        {
            UpdateAbilities();
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
        undo.SetActive(tempUnlockedAbilities.Count > 0);
        leftArrow.interactable = characterIndex > 0;
        rightArrow.interactable = characterIndex < _data.Characters.Count - 1;
        if (_data.Characters.Count > 3)
        {
            sellButton.interactable = true;
        }
        else
        {
            sellButton.interactable = false;
        }
    }

    public void UpdateConfirmationTable()
    {
        var character = _data.Characters[characterIndex];
        Color color = character.characterData.classColor; // fix this shit
        transform.Find("ConfirmationTable").Find("Text").GetComponent<Text>().text = $"Are you sure you want to sell <color=#{ColorUtility.ToHtmlStringRGBA(color)}>{character.characterName}</color> for <color=yellow>{character.cost / 2}</color> gold?";
        transform.Find("ConfirmationTable").Find("Portrait").GetComponent<Image>().sprite = character.prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
    }

    private void RemoveHighlightsFromPortraitBar()
    {
        for (int i = 0; i < portraitButtons.buttonOnHover.Count; i++)
        {
            if (portraitButtons.townPortraits[i].gameObject.activeInHierarchy)
            {
                portraitButtons.buttonOnHover[i].SetBool("select", false);
            }
        }
    }

    private void HighlightCurrentCharacterInPortraitBar()
    {

        for (int i = 0; i < portraitButtons.buttonOnHover.Count; i++)
        {
            if (portraitButtons.townPortraits[i].gameObject.activeInHierarchy)
            {
                if (portraitButtons.townPortraits[i].characterIndex == characterIndex)
                {
                    portraitButtons.buttonOnHover[i].SetBool("select", true);
                }
                else
                {
                    portraitButtons.buttonOnHover[i].SetBool("select", false);
                }
            }
        }
    }

    public void UpdateAbilities()
    {
        var character = _data.Characters[characterIndex];
        for (int i = 0; i < transform.Find("Abilities").childCount; i++)
        {
            transform.Find("Abilities").GetChild(i).Find("Add").GetComponent<Button>().interactable = character.abilityPointCount > 0;
            transform.Find("Abilities").GetChild(i).Find("Add").gameObject.SetActive
            (character.unlockedAbilities[i] == '0'
                && character.abilityPointCount > 0);
            if (character.unlockedAbilities[i] == '0')
            {
                transform.Find("Abilities").GetChild(i).Find("Color").GetComponent<Image>().color = Color.gray;
            }
            else
            {
                transform.Find("Abilities").GetChild(i).Find("Color").GetComponent<Image>().color = Color.white;
            }
        }

    }
    public void UpdateTableInformation()
    {
        SavedCharacter character = _data.Characters[characterIndex];
        originalName = character.characterName;
        PlayerInformation characterInfo = character.prefab.GetComponent<PlayerInformation>();
        Color color = characterInfo.ClassColor;
        tableBoarder.color = characterInfo.ClassColor2;
        nameInput.text = character.characterName;
        className.text = character.prefab.GetComponent<PlayerInformation>().ClassName;
        className.color = color;
        role.text = characterInfo.role;
        role.color = color;
        UpdateRoleIcon(character);
        level.text = "LEVEL: " + character.level.ToString();
        level.color = color;
        maxHP.text = "MAX HP: " + CalculateMaxHP(character);//character.prefab.GetComponent<PlayerInformation>().MaxHealth.ToString();
        maxHP.color = color;
        xpProgress.text = (character.level >= GameProgress.currentMaxLevel()) ? "MAX LEVEL" : character.xP + "/" + _data.XPToLevelUp[character.level - 1] + " XP";
        xpProgress.color = color;
        abilityPointCount.text = character.abilityPointCount.ToString();
        abilityPointCount.color = color;
        characterArt.sprite = characterInfo.CharacterSplashArt;
        blessingList.text = character.CharacterTableBlessingString();
        //
        for (int i = 0; i < transform.Find("Abilities").transform.childCount; i++)
        {
            transform.Find("Abilities").GetChild(i).gameObject.SetActive(true);
            transform.Find("Abilities").GetChild(i).Find("Color").GetComponent<Image>().sprite = character.prefab.GetComponent<ActionManager>().AbilityBackground;
            transform.Find("Abilities").GetChild(i).Find("AbilityIcon").GetComponent<Image>().color = characterInfo.ClassColor;
            if (character.prefab.GetComponent<ActionManager>().FindActionByIndex(i) != null)
            {
                transform.Find("Abilities").GetChild(i).Find("AbilityIcon").GetComponent<Image>().sprite = character.prefab.GetComponent<ActionManager>().FindActionByIndex(i).AbilityIcon;
            }
            else
            {
                transform.Find("Abilities").GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    
    public void DisplayCharacterTable(int index)
    {
        Debug.Log("Character Select scene can be broken");
        GameObject table = GameObject.Find("Canvas").transform.Find("CharacterTable").gameObject;
        if (index != characterIndex)
        {
            helpTableController.CloseHelpTable();
            helpTable.gameObject.SetActive(false);
            
        }
        characterIndex = index;
        ResetTempUnlockedAbilities();

        if (recruitmentCenterTable != null && townHall != null)
        {
            recruitmentCenterTable.SetActive(false);
            townHall.CloseTownHall();
        }
        else
        {
            Debug.Log("TownHall is null");
        }
        
        table.SetActive(true);
    }
    
    private int CalculateMaxHP(SavedCharacter character)
    {
        int maxHP = character.prefab.GetComponent<PlayerInformation>().MaxHealth; // fix this
        maxHP += (character.level - 1) * 2;
        if (character.blessings.Find(x => x.blessingName == "Healthy") != null)
        {
            maxHP += 3;
        }
        return maxHP;
    }
    private void UpdateRoleIcon(SavedCharacter character)
    {
        for (int i = 0; i < transform.Find("Info").transform.Find("Role").childCount; i++)
        {
            if (character.prefab.GetComponent<PlayerInformation>().role == transform.Find("Info").transform.Find("Role").GetChild(i).name)
            {
                transform.Find("Info").transform.Find("Role").GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.Find("Info").transform.Find("Role").GetChild(i).gameObject.SetActive(false);
            }

        }

    }
    public void UpgradeAbility(int abilityIndex)
    {
        var unlockedAbilities = _data.Characters[characterIndex].unlockedAbilities;
        string newUnlockedAbilities = "";
        for (int i = 0; i < unlockedAbilities.Length; i++)
        {
            if (i != abilityIndex)
            {
                newUnlockedAbilities += unlockedAbilities[i];
            }
            else
            {
                newUnlockedAbilities += 1.ToString();
            }
        }
        _data.Characters[characterIndex].unlockedAbilities = newUnlockedAbilities;
        _data.Characters[characterIndex].abilityPointCount--;
        tempUnlockedAbilities.Add(abilityIndex);
        UpdateTable();

    }
    private void RemoveAbility(int abilityIndex)
    {
        var unlockedAbilities = _data.Characters[characterIndex].unlockedAbilities;
        string newUnlockedAbilities = "";
        for (int i = 0; i < unlockedAbilities.Length; i++)
        {
            if (i != abilityIndex)
            {
                newUnlockedAbilities += unlockedAbilities[i];
            }
            else
            {
                newUnlockedAbilities += 0.ToString();
            }
        }
        _data.Characters[characterIndex].unlockedAbilities = newUnlockedAbilities;
        _data.Characters[characterIndex].abilityPointCount++;
    }

    public void OnLeftArrowClick()
    {
        int newCharacterIndex = Mathf.Clamp(characterIndex - 1, 0, _data.Characters.Count - 1);
        if (newCharacterIndex == 5)//newCharacterIndex / 6 < characterIndex / 6)
        {
            Debug.Log("Need To change Function");
            // gameProgress.UpdateCharacterBar(1);
        }
        DisplayCharacterTable(newCharacterIndex);
        UpdateTable();
    }

    public void OnRightArrowClick()
    {
        int newCharacterIndex = Mathf.Clamp(characterIndex + 1, 0, _data.Characters.Count - 1);
        if (newCharacterIndex == 6)//newCharacterIndex / 6 > characterIndex / 6)
        {
            Debug.Log("Need To change Function");
            // gameProgress.UpdateCharacterBar(-1);
        }
        DisplayCharacterTable(newCharacterIndex);
        UpdateTable();
    }

    public void UndoAbilitySelection()
    {
        foreach (int abilityIndex in tempUnlockedAbilities)
        {
            RemoveAbility(abilityIndex);
        }
        tempUnlockedAbilities.Clear();
        UpdateTable();
    }

    public void ExitTable()
    {
        gameObject.SetActive(false);
        helpTable.gameObject.SetActive(false);
        UpdateTable();
    }
    
    
}
