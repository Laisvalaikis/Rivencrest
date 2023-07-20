using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PortraitBar : MonoBehaviour
{
    public List<CharacterPortrait> townPortraits;
    [SerializeField] private List<Button> portraitButtons;

    public Button up;

    public Button down;

    private Data _data;

    private int _lastElement = -1; // array starts from zero 

    private int _scrollCharacterSelectIndex;
    private int _page = 1;

    // Start is called before the first frame update
    void Start()
    {
        _data = Data.Instance;
        _scrollCharacterSelectIndex = 0;
        SetupCharacters();
    }

    public void DisableAllButtons()
    {
        for (int i = 0; i < portraitButtons.Count; i++)
        {
            portraitButtons[i].interactable = false;
        }
    }
    
    public void EnableAllButtons()
    {
        for (int i = 0; i < portraitButtons.Count; i++)
        {
            portraitButtons[i].interactable = true;
        }
    }

    public void SetupCharacters()
    {
        if (_scrollCharacterSelectIndex + townPortraits.Count < _data.Characters.Count)
        {
            down.gameObject.SetActive(true);
        }
        Debug.Log(_data.Characters.Count);
        for (int i = 0; i < _data.Characters.Count; i++)
        {
            if (i < townPortraits.Count)
            {
                UpdatePortrait(i, i);
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
        if (index < _data.Characters.Count && index < townPortraits.Count)
        {
            UpdatePortrait(index, index);
            _lastElement = index;
            
        }
        else if (_scrollCharacterSelectIndex + townPortraits.Count < _data.Characters.Count)
        {
            down.gameObject.SetActive(true);
        }
    }
    
    public void RemoveCharacter(int characterIndex)
    {
        int index = characterIndex-_scrollCharacterSelectIndex;
        CharacterPortrait characterPortrait = townPortraits[index];

        if(_data.Characters.Count < _page * townPortraits.Count && _data.Characters.Count >= (_page - 1) * townPortraits.Count )
        {
            townPortraits.RemoveAt(index);
            int siblingIndex = townPortraits[townPortraits.Count - 1].transform.GetSiblingIndex();
            characterPortrait.transform.SetSiblingIndex(siblingIndex);
            characterPortrait.gameObject.SetActive(false);
            townPortraits.Add(characterPortrait);
        }
        
        float countOfCharacters = _data.Characters.Count - (_page - 1) * townPortraits.Count ;
        float countToUpdate = countOfCharacters > townPortraits.Count ? townPortraits.Count : countOfCharacters;
        for (int i = index, count = characterIndex; i < countToUpdate; i++, count++)
        {
            UpdatePortrait(i, count);
        }

        UpdateArrows(_scrollCharacterSelectIndex * -1);
        
        if ( (_page - 1) * townPortraits.Count >= _data.Characters.Count)
        {
            Scroll(-1);
        }
        
        

        _lastElement--;
        
    }

    public void Scroll(int direction) // up - (-1), down - (1)
    {
        int scrollCalculation = _scrollCharacterSelectIndex + townPortraits.Count * direction;
        if (townPortraits.Count <= _data.Characters.Count && scrollCalculation >= 0 && scrollCalculation < _data.Characters.Count)
        {
            _scrollCharacterSelectIndex += townPortraits.Count * direction;
            int count = _data.Characters.Count - _scrollCharacterSelectIndex;
            for (int i = 0; i < townPortraits.Count; i++)
            {
                if (i < count)
                {
                    int index = i + _scrollCharacterSelectIndex;
                    UpdatePortrait(i, index);
                }
                else
                {
                    townPortraits[i].gameObject.SetActive(false);
                }
            }

            _lastElement = _scrollCharacterSelectIndex + count;
            _page += direction;
        }

        UpdateArrows(scrollCalculation);
    }

    public void ScrollUpByCharacterIndex(int characterIndex)
    {
        
        if (characterIndex >= _page * townPortraits.Count)
        {
            Scroll(1);
        }
        
    }

    public void ScrollDownByCharacterIndex(int characterIndex)
    {
        if (characterIndex < (_page-1) * townPortraits.Count)
        {
            Scroll(-1);
        }
    }

    public void UpdatePortrait(int portraitIndex, int index)
    {
        townPortraits[portraitIndex].gameObject.SetActive(true);
        townPortraits[portraitIndex].characterIndex = index;
        townPortraits[portraitIndex].characterImage.sprite = _data.Characters[index].playerInformation.CharacterPortraitSprite;
        townPortraits[portraitIndex].levelText.text = _data.Characters[index].level.ToString();
        if (_data.Characters[index].toConfirmAbilities > 0 || _data.Characters[index].abilityPointCount > 0)
        {
            townPortraits[portraitIndex].abilityPointCorner.SetActive(true);
        }
        else
        {
            townPortraits[portraitIndex].abilityPointCorner.SetActive(false);
        }
    }

    public void UpdateArrows(int scrollCalculation)
    {
        if (_data.Characters.Count > townPortraits.Count * (_page - 1) && _page - 1 > 0)
        {
            up.gameObject.SetActive(true);
        }
        else
        {
            up.gameObject.SetActive(false);
        }
        

        Debug.Log(Mathf.Abs(scrollCalculation) + townPortraits.Count);
        if (_data.Characters.Count > townPortraits.Count * _page && townPortraits.Count * _page <= _data.Characters.Count)
        {
            down.gameObject.SetActive(true);
        }
        else if (scrollCalculation <= _data.Characters.Count)
        {
            down.gameObject.SetActive(false); 
        }
        else if (scrollCalculation >= _data.Characters.Count)
        {
            down.gameObject.SetActive(false); 
        }
    }

    public void DisableAbilityCorner(int index)
    {
        int calculatedIndex = index-_scrollCharacterSelectIndex;
        townPortraits[calculatedIndex].abilityPointCorner.SetActive(false);
    }

    public void EnableAbilityCorner(int index)
    {
        int calculatedIndex = index-_scrollCharacterSelectIndex;
        townPortraits[calculatedIndex].abilityPointCorner.SetActive(true);
    }

}
