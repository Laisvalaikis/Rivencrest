using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HelpTableController : MonoBehaviour
{
   // public Sprite attackIcon;
   public List<AbilityText> abilityText;
    private Dictionary<string, AbilityText> _abilities;
    [HideInInspector] public bool hasActionButtonBeenEntered = false;
    public HelpTable helpTable;
    public Data _data;
    private Vector3 isAbilitySlowOriginalPosition;
    
    private void Start()
    {
        _abilities = new Dictionary<string, AbilityText>();
        for (int i = 0; i < abilityText.Count; i++)
        {
            _abilities.Add(abilityText[i].name, abilityText[i]);
        }

        isAbilitySlowOriginalPosition = helpTable.isAbilitySlow.transform.localPosition;
    }

    public void EnableTableForTown(int abilityIndex)
    {
        var ability = _data.Characters[GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex].prefab.
            GetComponent<ActionManager>().FindActionByIndex(abilityIndex).action.GetBuffedAbility(_data.Characters[GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex].blessings);
        AbilityText abilityText = _abilities[ability.actionStateName];
        if (abilityText != null)
        {
            if (GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(abilityIndex).transform.Find("ActionButtonFrame").GetComponent<Animator>().GetBool("select"))
            {
                helpTable.gameObject.SetActive(false);
                CloseHelpTable();
            }
            else
            {

                helpTable.gameObject.SetActive(false);
                CloseHelpTable();
                var character = _data.Characters[GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().characterIndex];
                helpTable.gameObject.SetActive(true);
                FillTableWithInfo(ability, abilityText, character, character.prefab.GetComponent<ActionManager>());
                GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(abilityIndex).transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("select", true);
            }
        }
    }

    private void FillTableWithInfo(BaseAction ability, AbilityText abilityText, SavedCharacter character, ActionManager actionManager)
    {
        helpTable.icon.sprite = actionManager.FindActionListByName(ability.actionStateName).AbilityIcon;
        helpTable.abilityTitle.text = abilityText.abilityTitle;
        helpTable.abilityDescription.text = abilityText.abilityDescription;
        helpTable.cooldownText.text = ability.AbilityCooldown.ToString();
        if (ability.maxAttackDamage == 0)
        {
            helpTable.damageIcon.SetActive(false);
            helpTable.damageText.gameObject.SetActive(false);
            // helpTable.isAbilitySlow.transform.localPosition = isAbilitySlowOriginalPosition + new Vector3(0f, 80f);
            Debug.Log("Offset for slow/fast ability off");
        }
        else
        {
            helpTable.damageIcon.SetActive(true);
            helpTable.damageText.gameObject.SetActive(true);
            helpTable.damageText.text = ability.GetDamageString();
            Debug.Log("Offset for slow/fast ability off");
            // helpTable.isAbilitySlow.transform.localPosition = isAbilitySlowOriginalPosition;
        }
        helpTable.slowAbility.SetActive(ability.isAbilitySlow);
        helpTable.fastAbility.SetActive(!ability.isAbilitySlow);
        helpTable.rangeText.text = ability.AttackRange.ToString();
        var blessingsList = character.blessings.FindAll(x => x.spellName == ability.actionStateName);
        string blessingsText = "";
        foreach (var blessing in blessingsList)
        {
            blessingsText += $"{blessing.blessingName}\n";
        }
        helpTable.blessingsText.text = blessingsText;
    }

    public void EnableTableForInGameButton()
    {
        var gameInformation = GameObject.Find("GameInformation").GetComponent<GameInformation>();
        if (gameInformation.SelectedCharacter != null || gameInformation.InspectedCharacter != null)
        {
            var character = gameInformation.SelectedCharacter == null ? gameInformation.InspectedCharacter : gameInformation.SelectedCharacter;
            var ability = character.GetComponent<ActionManager>().FindActionByName(character.GetComponent<PlayerInformation>().currentState).GetBuffedAbility(character.GetComponent<PlayerInformation>().savedCharacter.blessings);
            AbilityText abilityText = _abilities[ability.actionStateName];
            if (abilityText != null)
            {
                
                gameInformation.helpTableOpen = true;
                gameInformation.isBoardDisabled = true;
                helpTable.gameObject.SetActive(true);
                FillTableWithInfo(ability, abilityText, character.GetComponent<PlayerInformation>().savedCharacter, character.GetComponent<ActionManager>());
               
            }
        }
    }

    public void EnableTableForInGameRightClick(string abilityName)
    {
        var gameInformation = GameObject.Find("GameInformation").GetComponent<GameInformation>();
        if (gameInformation.SelectedCharacter != null || gameInformation.InspectedCharacter != null)
        {
            var character = gameInformation.SelectedCharacter == null ? gameInformation.InspectedCharacter : gameInformation.SelectedCharacter;
            BaseAction action = character.GetComponent<ActionManager>().FindActionByName(abilityName);
            if (action != null) 
            {
                var ability = action.GetBuffedAbility(character.GetComponent<PlayerInformation>().savedCharacter.blessings);
                AbilityText abilityText = _abilities[ability.actionStateName];
                if (abilityText != null)
                {
                    gameInformation.helpTableOpen = true;
                    gameInformation.isBoardDisabled = true;
                    helpTable.gameObject.SetActive(true);
                    FillTableWithInfo(ability, abilityText, character.GetComponent<PlayerInformation>().savedCharacter,
                        character.GetComponent<ActionManager>());
                }
            }
        }
    }

    public void EnableTableForPVP(string abilityName, SavedCharacter character)
    {
        CloseAllHelpTables();
        var ability = character.prefab.GetComponent<ActionManager>().FindActionByName(abilityName).GetBuffedAbility(character.blessings);
        AbilityText abilityText = _abilities[ability.actionStateName];
        if (abilityText == null)
        {
            print("Ner tokios help table lol");
        }
        else
        {
            
            helpTable.gameObject.SetActive(true);
            FillTableWithInfo(ability, abilityText, character, character.prefab.GetComponent<ActionManager>());
        }
    }

    public void EnableTableByName(string abilityName, SavedCharacter character)
    {
        CloseAllHelpTables();
        var ability = character.prefab.GetComponent<ActionManager>().FindActionByName(abilityName).GetBuffedAbility(character.blessings);
        AbilityText abilityText = _abilities[ability.actionStateName];
        if (abilityText == null)
        {
            print("Ner tokios help table lol");
        }
        else
        {
            helpTable.gameObject.SetActive(true);
            FillTableWithInfo(ability, abilityText, character, character.prefab.GetComponent<ActionManager>());
        }
    }

    public void CloseHelpTable()
    {
        for (int i = 0; i < GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.childCount; i++)
        {
            GameObject.Find("Canvas").transform.Find("CharacterTable").transform.Find("Abilities").transform.GetChild(i).transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("select", false);
        }
    }

    public void CloseAllHelpTables()
    {
        if (GameObject.Find("Canvas") != null && GameObject.Find("Canvas").transform.Find("HelpTables") != null)
        {
            foreach (Transform x in GameObject.Find("Canvas").transform.Find("HelpTables"))
            {
                helpTable.gameObject.SetActive(false);
            }
        }
        if (GameObject.Find("CanvasCamera") != null && GameObject.Find("CanvasCamera").transform.Find("HelpTables") != null)
        {
            foreach (Transform x in GameObject.Find("CanvasCamera").transform.Find("HelpTables"))
            {
                helpTable.gameObject.SetActive(false);
            }
        }
    }
}
