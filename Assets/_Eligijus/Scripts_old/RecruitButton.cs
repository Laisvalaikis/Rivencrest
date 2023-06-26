using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitButton : MonoBehaviour
{
    public GameObject characterTable;
    public Image portrait;
    public Button buyButton;
    public TextMeshProUGUI className;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI xP;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xPToGain;
    public SavedCharacter character;
    private int XPToLevelUp;
    public Data _data;
    
    public void UpdateRecruitButton()
    {
        if (character == null)
        {
            transform.Find("CharacterTable").gameObject.SetActive(false);
        }
        else
        {
            // transform.Find("CharacterTable").gameObject.SetActive(true);
            characterTable.SetActive(true);
            var charInformation = character.prefab.GetComponent<PlayerInformation>();
            // transform.Find("CharacterTable").Find("ClassName").GetComponent<Text>().text = charInformation.ClassName;
            // transform.Find("CharacterTable").Find("ClassName").GetComponent<Text>().color = charInformation.ClassColor;
            className.text = charInformation.ClassName;
            className.color = charInformation.ClassColor;
            // transform.Find("CharacterTable").Find("Portrait").GetComponent<Image>().sprite = charInformation.CharacterPortraitSprite;
            portrait.sprite = charInformation.CharacterPortraitSprite;
            // transform.Find("CharacterTable").Find("Cost").GetComponent<Text>().text = character.cost.ToString() + "g";
            cost.text = character.cost.ToString() + "g";
            if (_data.townData.townGold >= character.cost && _data.Characters.Count < _data.maxCharacterCount)
            {
                // transform.Find("CharacterTable").Find("BuyButton").GetComponent<Button>().interactable = true;
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
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
