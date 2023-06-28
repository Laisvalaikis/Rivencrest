using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PortraitBar : MonoBehaviour
{
    public List<CharacterPortrait> townPortraits;

    public Button up;

    public Button down;

    private Data _data;

    private int _lastElement = -1; // array starts from zero 

    private List<SavedCharacter> _currentCharacters;

    private int _scrollCharacterSelectIndex;

    // Start is called before the first frame update
    void Start()
    {
        _data = Data.Instance;
        _currentCharacters = _data.Characters;
        _scrollCharacterSelectIndex = 0;
        SetupCharacters();
    }

    public void SetupCharacters()
    {
        if (_scrollCharacterSelectIndex + townPortraits.Count < _currentCharacters.Count)
        {
            down.gameObject.SetActive(true);
        }
        for (int i = 0; i < _currentCharacters.Count; i++)
        {
            if (i < townPortraits.Count)
            {
                townPortraits[i].gameObject.SetActive(true);
                townPortraits[i].characterIndex = i;
                townPortraits[i].characterImage.sprite = _currentCharacters[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
                townPortraits[i].levelText.text = _currentCharacters[i].level.ToString();
                if (_currentCharacters[i].abilityPointCount > 0)
                {
                    townPortraits[i].abilityPointCorner.SetActive(true);
                }
                else
                {
                    townPortraits[i].abilityPointCorner.SetActive(false); 
                }

                _lastElement = i;
                // townPortraits[i] = _currentCharacters[i].
            }
            else
            {
                break;
            }
        }
    }

    public void InsertCharacter()
    {
        int index = _lastElement + 1;
        if (index < _currentCharacters.Count && index < townPortraits.Count)
        {
            townPortraits[index].gameObject.SetActive(true);
            townPortraits[index].characterIndex = index;
            townPortraits[index].characterImage.sprite = _currentCharacters[index].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            townPortraits[index].levelText.text = _currentCharacters[index].level.ToString();
            if (_currentCharacters[index].abilityPointCount > 0)
            {
                townPortraits[index].abilityPointCorner.SetActive(true);
            }
            else
            {
                townPortraits[index].abilityPointCorner.SetActive(false); 
            }

            _lastElement = index;
            
        }
        else if (_scrollCharacterSelectIndex + townPortraits.Count < _currentCharacters.Count)
        {
            down.gameObject.SetActive(true);
        }
    }

    public void RemoveCharacter(int characterIndex)
    {
        int index = characterIndex-_scrollCharacterSelectIndex;
        CharacterPortrait characterPortrait = townPortraits[index];
        townPortraits.RemoveAt(index);
        for (int i = index, count = characterIndex; i < townPortraits.Count; i++, count++)
        {
            townPortraits[i].characterIndex = count;
        }
        int siblingIndex = townPortraits[townPortraits.Count - 1].transform.GetSiblingIndex();
        characterPortrait.transform.SetSiblingIndex(siblingIndex);
        characterPortrait.gameObject.SetActive(false);
        townPortraits.Add(characterPortrait);
        if ( townPortraits.Count >= _currentCharacters.Count)
        {
            Scroll(-1);
        }

        _lastElement--;
        
    }

    public void Scroll(int direction) // up - (-1), down - (1)
    {
        int scrollCalculation = _scrollCharacterSelectIndex + townPortraits.Count * direction;
        if (townPortraits.Count <= _currentCharacters.Count && scrollCalculation >= 0 && scrollCalculation < _currentCharacters.Count)
        {
            _scrollCharacterSelectIndex += townPortraits.Count * direction;
            int count = _currentCharacters.Count - _scrollCharacterSelectIndex;
            for (int i = 0; i < townPortraits.Count; i++)
            {
                if (i < count)
                {
                    int index = i + _scrollCharacterSelectIndex;
                    townPortraits[i].gameObject.SetActive(true);
                    townPortraits[i].characterIndex = index;
                    townPortraits[i].characterImage.sprite = _currentCharacters[index].prefab
                        .GetComponent<PlayerInformation>().CharacterPortraitSprite;
                    townPortraits[i].levelText.text = _currentCharacters[index].level.ToString();
                    if (_currentCharacters[index].abilityPointCount > 0)
                    {
                        townPortraits[i].abilityPointCorner.SetActive(true);
                    }
                    else
                    {
                        townPortraits[i].abilityPointCorner.SetActive(false);
                    }
                }
                else
                {
                    townPortraits[i].gameObject.SetActive(false);
                }
            }

            _lastElement = _scrollCharacterSelectIndex + count;
        }

        
        
        if(scrollCalculation <= 0)
        {
            up.gameObject.SetActive(false);
            
        }
        else if (_currentCharacters.Count > townPortraits.Count)
        {
            up.gameObject.SetActive(true);
        }

        if (_currentCharacters.Count > townPortraits.Count && scrollCalculation + townPortraits.Count * direction < _currentCharacters.Count)
        {
            down.gameObject.SetActive(true);
        }
        else if (scrollCalculation <= _currentCharacters.Count)
        {
            down.gameObject.SetActive(false); 
        }
        else if (scrollCalculation >= _currentCharacters.Count)
        {
            down.gameObject.SetActive(false); 
        }

    }

    public void DisableAbilityCorner(int index)
    {
        townPortraits[index].abilityPointCorner.SetActive(false);
    }

    public void EnableAbilityCorner(int index)
    {
        townPortraits[index].abilityPointCorner.SetActive(true);
    }

}
