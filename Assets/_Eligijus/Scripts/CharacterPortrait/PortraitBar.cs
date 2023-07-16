using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
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

    private int _scrollCharacterSelectIndex;
    private int _page = 1;

    // Start is called before the first frame update
    void Start()
    {
        _data = Data.Instance;
        _scrollCharacterSelectIndex = 0;
        SetupCharacters();
    }

    public void SetupCharacters()
    {
        if (_scrollCharacterSelectIndex + townPortraits.Count < _data.Characters.Count)
        {
            down.gameObject.SetActive(true);
        }
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
        
        // fixed part
        
        for (int i = index, count = characterIndex; i < _data.Characters.Count - characterIndex; i++, count++)
        {
            UpdatePortrait(i, count);
        }
        
        if ( townPortraits.Count >= _data.Characters.Count)
        {
            UpdateArrows(_scrollCharacterSelectIndex + townPortraits.Count);
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

    public void UpdatePortrait(int portraitIndex, int index)
    {
        townPortraits[portraitIndex].gameObject.SetActive(true);
        townPortraits[portraitIndex].characterIndex = index;
        townPortraits[portraitIndex].characterImage.sprite = _data.Characters[index].prefab
            .GetComponent<PlayerInformation>().CharacterPortraitSprite;
        townPortraits[portraitIndex].levelText.text = _data.Characters[index].level.ToString();
        if (_data.Characters[index].abilityPointCount > 0)
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
        if(scrollCalculation <= 0)
        {
            up.gameObject.SetActive(false);
            
        }
        else if (_data.Characters.Count > townPortraits.Count)
        {
            up.gameObject.SetActive(true);
        }

        if (_data.Characters.Count > townPortraits.Count && Mathf.Abs(scrollCalculation) + townPortraits.Count < _data.Characters.Count)
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
