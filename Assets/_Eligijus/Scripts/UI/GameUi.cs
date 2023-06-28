using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI townGold;
    [SerializeField] private List<TextMeshProUGUI> townGoldChanges;
    [SerializeField] private TextMeshProUGUI dayNumber;
    [SerializeField] private TextMeshProUGUI difficulty;
    [SerializeField] private GameObject abilityPointWarning;
    [SerializeField] private GameObject buyRecruitsWarning;
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
    
    private void UpdateDifficultyText()
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
        if (Data.Instance.Characters.Count < 3)
        {
            buyRecruitsWarning.SetActive(true);
        }
        else
        {
            buyRecruitsWarning.SetActive(false);
        }

        
    }
    
    public void UpdateUnspentPointWarnings()
    {
        bool playerHasUnspentPoints = false;
        if (Data.Instance.Characters != null)
        {
            List<SavedCharacter> charactersToUpdate = Data.Instance.Characters;
            for (int i = 0; i < charactersToUpdate.Count; i++)
            {
                if (charactersToUpdate[i].abilityPointCount > 0)
                {
                    playerHasUnspentPoints = true;
                    abilityPointWarning.SetActive(true);
                    break;
                }
            }

            if (!playerHasUnspentPoints)
            {
                abilityPointWarning.SetActive(false);
            }
        }
    }

}
