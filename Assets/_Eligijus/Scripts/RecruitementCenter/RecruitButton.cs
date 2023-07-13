using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitButton : MonoBehaviour
{
    public Image portrait;
    public Button buyButton;
    public TextMeshProUGUI className;
    public TextMeshProUGUI cost;
    public SavedCharacter character;
    private int XPToLevelUp;
    public Data _data;

    private void OnEnable()
    {
        if (_data == null && Data.Instance != null)
        {
            _data = Data.Instance;
        }
    }

    public void UpdateRecruitButton()
    {
        
            var charInformation = character.playerInformation;
            className.text = charInformation.ClassName;
            className.color = charInformation.classColor;
            portrait.sprite = charInformation.CharacterPortraitSprite;
            cost.text = character.cost.ToString() + "g";
            if (_data.townData.townGold >= character.cost && _data.Characters.Count < _data.maxCharacterCount)
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
    }

    public void BuyCharacter()
    {
        if (_data.Characters.Count < _data.maxCharacterCount)
        {
            GameObject.Find("GameProgress").GetComponent<GameProgress>().BuyCharacter(character);
            transform.parent.parent.gameObject.GetComponent<Recruitment>().CharactersInShop.Remove(character);
            transform.parent.parent.gameObject.GetComponent<Recruitment>().UpdateButtons();
        }
        else Debug.Log("Ziurek ka darai, kvaily!");
    }

}
