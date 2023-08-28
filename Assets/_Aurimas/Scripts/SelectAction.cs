using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectAction : MonoBehaviour
{
    private ActionManagerNew _actionManagerNew;
    private List<Ability> _playerAbilities;
    private GameObject _currentPlayer;
    private PlayerInformationData _playerInformationData;
    [SerializeField] private HelpTable helpTable;
    [SerializeField] private AbilityManager _abilityManager;
    [SerializeField] private List<SelectActionButton> abilityButtons;
    private void GetAbilities()
    {
        _playerAbilities = _currentPlayer.GetComponent<ActionManagerNew>().ReturnAbilities();
        _playerInformationData = _currentPlayer.GetComponent<PlayerInformation>().playerInformationData;
    }

    private void GenerateActions()
    {
        int buttonIndex = 0;
        for (int i = 0; i < _playerAbilities.Count; i++)
        {
            if (_playerAbilities[i].enabled)
            {
                abilityButtons[buttonIndex].gameObject.SetActive(true);
                abilityButtons[buttonIndex].AbilityInformation(i, helpTable, _playerAbilities[i], this);
                abilityButtons[buttonIndex].AbilityButtonImage.sprite = _playerAbilities[i].AbilityImage;
                abilityButtons[buttonIndex].abilityButtonBackground.color = _playerInformationData.backgroundColor;
                abilityButtons[buttonIndex].characterPortrait.sprite = _playerInformationData.CharacterPortraitSprite;
                abilityButtons[buttonIndex].healthBar.text = _playerInformationData.MaxHealth.ToString();
                abilityButtons[buttonIndex].staminaButtonBackground.color = _playerInformationData.backgroundColor;
                buttonIndex++;
            }
        }

        for (int i = buttonIndex; i < abilityButtons.Count; i++)
        {
            abilityButtons[i].gameObject.SetActive(false);
        }
        
    }

    public void ActionSelection(CharacterAction characterAction)
    {
        _abilityManager.SetCurrentAbility(characterAction);
        characterAction.CreateGrid();
    }

    public void SetCurrentCharacter(GameObject currentPlayer)
    {
        gameObject.SetActive(true);
        _currentPlayer = currentPlayer;
        GetAbilities();
        GenerateActions();
        _abilityManager.SetCurrentAbility(_playerAbilities[0].Action);
    }
    public void DeSetCurrentCharacter()
    {
        _currentPlayer = null;
    }

}
