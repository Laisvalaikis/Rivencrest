using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpTable : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI abilityTitle;
    public TextMeshProUGUI abilityDescription;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI blessingsText;
    public GameObject damageIcon;
    public GameObject isAbilitySlow;
    public GameObject slowAbility;
    public GameObject fastAbility;
    public List<AbilityText> abilityText;
    private Dictionary<string, AbilityText> _abilities;
    [HideInInspector] public bool hasActionButtonBeenEntered = false;
    private Vector3 isAbilitySlowOriginalPosition;
    private bool wasSetuped = false;
    private bool wasSelected = false;
    private Data _data;
    
    public void closeHelpTable()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        SetupHelpTable();
    }

    private void SetupHelpTable()
    {
        if (!wasSetuped)
        {

            if (_data == null && Data.Instance != null)
            {
                _data = Data.Instance;
            }

            _abilities = new Dictionary<string, AbilityText>();
            for (int i = 0; i < abilityText.Count; i++)
            {
                _abilities.Add(abilityText[i].name, abilityText[i]);
            }

            isAbilitySlowOriginalPosition = isAbilitySlow.transform.localPosition;
            wasSetuped = true;
        }
    }

    public void EnableTableForBoughtCharacters(int abilityIndex, int characterIndex)
    {
        SetupHelpTable();
        ActionManager actionManager = _data.Characters[characterIndex].prefab.GetComponent<ActionManager>();
        SavedCharacter character = _data.Characters[characterIndex];
        UpdateHelpTable(abilityIndex, character, actionManager);
    }

    private void UpdateHelpTable(int abilityIndex, SavedCharacter character, ActionManager actionManager)
    {
        var ability = actionManager.FindActionByIndex(abilityIndex).action;
        AbilityText abilityText = _abilities[ability.actionStateName];
        if (abilityText != null)
        {
            if (wasSelected)
            {
                gameObject.SetActive(false);
                wasSelected = false;
            }
            else
            {
                gameObject.SetActive(true);
                FillTableWithInfo(ability, abilityText, character, actionManager);
                wasSelected = true;
            }
        }
    }

    public void EnableTableForCharacters(int abilityIndex, int characterIndex)
    {
        SetupHelpTable();
        ActionManager actionManager = _data.AllAvailableCharacters[characterIndex].prefab.GetComponent<ActionManager>();
        SavedCharacter character = _data.AllAvailableCharacters[characterIndex];
        UpdateHelpTable(abilityIndex, character, actionManager);
    }
    

    private void FillTableWithInfo(BaseAction ability, AbilityText abilityText, SavedCharacter character, ActionManager actionManager)
    {
        icon.sprite = actionManager.FindActionListByName(ability.actionStateName).AbilityIcon;
        abilityTitle.text = abilityText.abilityTitle;
        abilityDescription.text = abilityText.abilityDescription;
        cooldownText.text = ability.AbilityCooldown.ToString();
        if (ability.maxAttackDamage == 0)
        {
            damageIcon.SetActive(false);
            damageText.gameObject.SetActive(false);
            // helpTable.isAbilitySlow.transform.localPosition = isAbilitySlowOriginalPosition + new Vector3(0f, 80f);
            Debug.Log("Offset for slow/fast ability off");
        }
        else
        {
            damageIcon.SetActive(true);
            damageText.gameObject.SetActive(true);
            damageText.text = ability.GetDamageString();
            Debug.Log("Offset for slow/fast ability off");
            // helpTable.isAbilitySlow.transform.localPosition = isAbilitySlowOriginalPosition;
        }
        slowAbility.SetActive(ability.isAbilitySlow);
        fastAbility.SetActive(!ability.isAbilitySlow);
        rangeText.text = ability.AttackRange.ToString();
        var blessingsList = character.blessings.FindAll(x => x.spellName == ability.actionStateName);
        StringBuilder blessingTextBuilder = new StringBuilder();
        foreach (var blessing in blessingsList)
        {
            blessingTextBuilder.Append($"{blessing.blessingName}\n");
        }
        blessingsText.text = blessingTextBuilder.ToString();
    }


}
