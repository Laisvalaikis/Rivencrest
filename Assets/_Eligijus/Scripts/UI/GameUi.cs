using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI townGold;
    [SerializeField] private List<TextMeshProUGUI> townGoldChanges;
    [SerializeField] private TextMeshProUGUI dayNumber;
    [SerializeField] private TextMeshProUGUI difficulty;
    [SerializeField] private GameObject abilityPointWarning;
    [SerializeField] private GameObject buyRecruitsWarning;
    [SerializeField] private Button embark;
    private Data _data;

    private void Start()
    {
        _data = Data.Instance;
        UpdateDifficultyText();
        UpdateDayNumber();
        UpdateTownCost();
        UpdateEmbarkButton();
    }

    public void UpdateEmbarkButton()
    {
        if (_data.Characters.Count >= _data.minCharacterCount)
        {
            embark.interactable = true;
        }
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
        if (_data.townData.difficultyLevel == 0)
        {
            _data.townData.difficultyLevel = 1;
            difficulty.text = "HARD";
        }
        else
        {
            _data.townData.difficultyLevel = 0;
            difficulty.text = "EASY";
        }
    }
    
    private void UpdateDifficultyText()
    {
        if (_data.townData.difficultyLevel == 0)
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
