using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterPortrait : PortraitButton
{
    public int characterIndex = 0;
    public Image characterImage;
    public TextMeshProUGUI levelText;
    public GameObject abilityPointCorner;
    public CharacterTable characterTable;
    private Data _data;

    private void Start()
    {
        _data = Data.Instance;
    }

    public override void OnPortraitClick()
    {
            if (_data.switchPortraits)
            {
                AddCharacterForSwitching(characterIndex);
            }
            else
            {
                if (characterTable.gameObject.activeInHierarchy && characterTable.characterIndex == characterIndex)
                {
                    characterTable.ExitTable();
                }
                else
                {
                    characterTable.DisplayCharacterTable(characterIndex);
                    characterTable.UpdateTable();
                }
            }
        
    }
    
    private void AddCharacterForSwitching(int index)
    {
        _data.SwitchedCharacters.Add(index);
        if (_data.SwitchedCharacters.Count == 2)
        {
            _data.switchPortraits = false;
            (_data.Characters[_data.SwitchedCharacters[0]], _data.Characters[_data.SwitchedCharacters[1]]) = (_data.Characters[_data.SwitchedCharacters[1]], _data.Characters[_data.SwitchedCharacters[0]]);
        }
    }

}
