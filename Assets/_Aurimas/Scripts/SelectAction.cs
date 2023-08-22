using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAction : MonoBehaviour
{
    private ActionManagerNew _actionManagerNew;
    private List<Ability> _playerAbilities;
    private GameObject _currentPlayer;
    private PlayerInformationData _playerInformationData;
    [SerializeField] private AbilityManager _abilityManager;
    [SerializeField] private List<SelectActionButton> abilityButtons;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void GetAbilities()
    {
        _playerAbilities = _currentPlayer.GetComponent<ActionManagerNew>().ReturnAbilities();
        _playerInformationData = _currentPlayer.GetComponent<PlayerInformation>().playerInformationData;
    }

    public void GenerateActions()
    {
        int buttonIndex = 0;
        for (int i = 0; i < _playerAbilities.Count; i++)
        {
            if (_playerAbilities[i].enabled)
            {
                abilityButtons[buttonIndex].gameObject.SetActive(true);
                abilityButtons[buttonIndex].AbilityInformation(i, _playerAbilities[i].Action, this);
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

    public void ActionSelection(int index, CharacterAction characterAction)
    {
        _abilityManager.SetCurrentAbility(characterAction);
    }

    public void SetCurrentCharacter(GameObject currentPlayer)
    {
        _currentPlayer = currentPlayer;
        GetAbilities();
        GenerateActions();
    }
    
    
}
