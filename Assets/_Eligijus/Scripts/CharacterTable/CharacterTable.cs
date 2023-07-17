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
    [SerializeField] private GameObject undo;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [HideInInspector] public string originalName;
    [SerializeField] private Button sellButton;
    [SerializeField] private List<Button> abilityButtons;
    [SerializeField] private List<Image> abilityButtonImages;
    public TMP_InputField nameInput;
    [SerializeField] private GameUi gameUI;
    public HelpTable helpTable;
    public GameObject recruitmentCenterTable;
    public TownHall townHall;
    
    private int characterIndex;
    private List<int> tempUnlockedAbilities = new List<int>();
    private Data _data;
    
    private void Awake()
    {
        ResetTempUnlockedAbilities();
    }

    private void OnEnable()
    {
        ResetTempUnlockedAbilities();
        if (_data == null && Data.Instance != null)
        {
            _data = Data.Instance;
        }
    }

    public int GetCurrentCharacterIndex()
    {
        return characterIndex;
    }

    private void OnDisable()
    {
        ResetTempUnlockedAbilities();
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
        gameUI.UpdateUnspentPointWarnings();
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
            abilityButtons[i].interactable = character.abilityPointCount > 0;
            if (character.unlockedAbilities[i] == '0')
            {
                abilityButtonImages[i].color = Color.gray;
            }
            else
            {
                abilityButtonImages[i].color = Color.white;
            }
        }
    }


    public void DisplayCharacterTable(int index)
    {
        gameObject.SetActive(true);
        if (index != characterIndex)
        {
            helpTable.gameObject.SetActive(false);
            UpdateAllAbilities();
            UpdateTable();
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
        
        
    }

    public void EnableDisableHelpTable(int index)
    {
        helpTable.EnableTableForTown(index, characterIndex);
    }
    
    public void UndoAbilitySelection()
    {
        foreach (int abilityIndex in tempUnlockedAbilities)
        {
            RemoveAbility(abilityIndex);
        }
        UpdateAllAbilities();
        tempUnlockedAbilities.Clear();
        UpdateTable();
    }
    
    public void ExitTable()
    {
        gameObject.SetActive(false);
    }
    
    public void OnLeftArrowClick()
    {
        int newCharacterIndex = Mathf.Clamp(characterIndex - 1, 0, _data.Characters.Count - 1);
        DisplayCharacterTable(newCharacterIndex);
        UpdateAllAbilities();
        UpdateTable();
        portraitBar.ScrollDownByCharacterIndex(newCharacterIndex);
    }

    public void OnRightArrowClick()
    {
        int newCharacterIndex = Mathf.Clamp(characterIndex + 1, 0, _data.Characters.Count - 1);
        DisplayCharacterTable(newCharacterIndex);
        UpdateAllAbilities();
        UpdateTable();
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
        gameObject.SetActive(false);
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
        UpdateCharacterPointCorner();
        tempUnlockedAbilities.Add(abilityIndex);
        UpdateAllAbilities();
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
        UpdateCharacterPointCorner();
    }
    
}
