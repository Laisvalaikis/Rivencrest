using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    public TextMeshProUGUI townGold;
    public List<TextMeshProUGUI> townGoldChanges;
    public TextMeshProUGUI dayNumber;
    public TextMeshProUGUI difficulty;
    // public GameObject CharacterButtons;
    public GameObject BuyRecruitsWarning;
    private Data _gameData;

    private void Start()
    {
        _gameData = Data.Instance;
        UpdateDifficultyText();
        UpdateDayNumber();
        UpdateTownCost();
    }

    public void UpdateTownCost()
    {
        townGold.text = Data.Instance.townData.townGold.ToString() + "g";
    }
    
    public void UpdateDayNumber()
    {
        dayNumber.text = "Day " + Data.Instance.townData.day.ToString();
    }
    
    public void UpdateDifficultyButton()
    {
        if (_gameData.townData.difficultyLevel == 0)
        {
            _gameData.townData.difficultyLevel = 1;
            difficulty.text = "HARD";
        }
        else
        {
            _gameData.townData.difficultyLevel = 0;
            difficulty.text = "EASY";
        }
    }
    
    public void UpdateDifficultyText()
    {
        if (_gameData.townData.difficultyLevel == 0)
        {
            difficulty.text = "EASY";
        }
        else
        {
            difficulty.text = "HARD";   
        }
    }

    public void EnableGoldChange(string text)
    {
      
            for (int i = 0; i < townGoldChanges.Count; i++)
            {
                if (!townGoldChanges[i].gameObject.activeSelf)
                {
                    townGoldChanges[i].text = text;
                    townGoldChanges[i].gameObject.SetActive(true);
                    break;
                }
            }
    }
    public void UpdateBuyRecruitsWarning()
    {
        BuyRecruitsWarning.SetActive(Data.Instance.Characters.Count < 3);
    }

    // PERDARYTI VISA SITA KITAIP
        public void UpdateUnspentPointWarnings()
    {
        // bool playerHasUnspentPoints = false;
        // List<SavedCharacter> charactersToUpdate;
        // if (Data.Instance.Characters != null)
        //     charactersToUpdate = Data.Instance.Characters;
        // else
        //     charactersToUpdate = Data.Instance.AllAvailableCharacters;
        // foreach(SavedCharacter character in charactersToUpdate)
        // {
        //     if(character.abilityPointCount>0)
        //     {
        //         playerHasUnspentPoints = true;
        //         abilityPointWarning.SetActive(true);
        //         break;
        //     }
        // }
        // if(!playerHasUnspentPoints)
        // {
        //     abilityPointWarning.SetActive(false);
        // }
    }

}
