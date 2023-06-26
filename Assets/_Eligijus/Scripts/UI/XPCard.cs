using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPCard : MonoBehaviour
{
    public GameObject characterTable;
    public Image portrait;
    public TextMeshProUGUI className;
    public TextMeshProUGUI xP;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xPToGain;
    public SavedCharacter character;
    private int XPToLevelUp;
    public Data _data; // Keisti prieiga prie data
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateXPButton()
    {
        if (character == null)
        {
            characterTable.SetActive(false);
        }
        else
        {
            characterTable.SetActive(true);
            var charInformation = character.prefab.GetComponent<PlayerInformation>(); // Sita sutvarkyti reikia
            className.text = character.characterName; //charInformation.ClassName;
            className.color = charInformation.ClassColor;
            portrait.sprite = charInformation.CharacterPortraitSprite;
            XPToLevelUp = _data.XPToLevelUp[character.level - 1];
            xP.text = character.xP + "/" + XPToLevelUp + " XP";
            if (character.level  >= GameProgress.currentMaxLevel())
                xP.text = "MAX LEVEL";
            levelText.text = character.level.ToString();
            xPToGain.text = "+" + character.xPToGain.ToString() + " XP";

            if (character.dead)
            {
                className.color = Color.gray;
                xPToGain.text = "DEAD";
                xPToGain.color = Color.gray;
                xP.color = Color.gray;
            }

        }
    }

    public void GrowXP(int XPToGrow)
    {
        if(character != null && character.xPToGain > 0)
        {
            if (character.level >= GameProgress.currentMaxLevel()) // Sutvarkyti Prieiga prie max lygio
            {
                character.xPToGain = 0;
            }
            if (character.xPToGain < XPToGrow)
            {
                XPToGrow = character.xPToGain;
            }
            character.xP += XPToGrow;
            character.xPToGain -= XPToGrow;
            if(character.xP >= XPToLevelUp)
            {
                character.level++;
                if(character.level != 4/*XPProgressManager.currentMaxLevel()*/)
                {
                    character.abilityPointCount++;
                }
                if (character.level >= GameProgress.currentMaxLevel())
                {
                    character.xPToGain = 0;
                    character.xP = 0;
                }
                else
                {
                    character.xP = character.xP - XPToLevelUp;
                }
                XPToLevelUp = _data.XPToLevelUp[character.level - 1];
                //kazkokia animacija
            }
            xP.text = character.xP + "/" + XPToLevelUp + " XP";
            if (character.level >= GameProgress.currentMaxLevel())
                xP.text = "MAX LEVEL";
            levelText.text = character.level.ToString();
        }
    }
    
}
