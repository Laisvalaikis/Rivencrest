using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterAbilityRecruit:MonoBehaviour
{
    [SerializeField] private Image color;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Button abilityButton;

    public void UpdateCharacterAbility(int index, PlayerInformation playerInformation, SavedCharacter savedCharacter)
    {
        gameObject.SetActive(true);
        color.sprite = playerInformation.GetComponent<ActionManager>().AbilityBackground;
        abilityIcon.color = playerInformation.ClassColor;
        Debug.LogError("Redo This Part");
        if (playerInformation.GetComponent<ActionManager>().FindActionByIndex(index) != null)
        {
            abilityIcon.sprite = playerInformation.GetComponent<ActionManager>().FindActionByIndex(index).AbilityIcon;
        }
        else
        {
            gameObject.SetActive(false);
        }
        var abilityName = playerInformation.GetComponent<ActionManager>().FindActionByIndex(index).actionName;
        abilityButton.onClick.RemoveAllListeners();
        abilityButton.onClick.AddListener(() =>
        {
            GameObject.Find("CharacterTable").GetComponent<HelpTable>().EnableTableByName(abilityName, savedCharacter);
        });
    }
}
