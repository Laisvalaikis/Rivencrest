using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Image characterPortrait;
    [SerializeField] private TextMeshProUGUI healthBar;
    [SerializeField] private Image staminaButtonBackground;
    [SerializeField] private List<SelectActionButton> abilityButtons;
    private void GetAbilities()
    {
        _playerAbilities = _currentPlayer.GetComponent<ActionManagerNew>().GetAbilities();
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
                buttonIndex++;
            }
        }

        for (int i = buttonIndex; i < abilityButtons.Count; i++)
        {
            abilityButtons[i].gameObject.SetActive(false);
        }
        
    }

    private void UpdatePlayerInfo()
    {
        characterPortrait.sprite = _playerInformationData.CharacterPortraitSprite;
        healthBar.text = _playerInformationData.MaxHealth.ToString();
        staminaButtonBackground.color = _playerInformationData.backgroundColor;
    }

    public void ActionSelection(BaseAction characterAction)
    {
        _abilityManager.SetCurrentAbility(characterAction);
    }

    public void SetCurrentCharacter(GameObject currentPlayer)
    {
        gameObject.SetActive(true);
        _currentPlayer = currentPlayer;
        GetAbilities();
        UpdatePlayerInfo();
        _abilityManager.SetCurrentAbility(_playerAbilities[0].Action);
        GenerateActions();
    }
    public void DeSetCurrentCharacter()
    {
        _currentPlayer = null;
    }

}
