using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Recruitment : MonoBehaviour
{
    public List<RecruitButton> recruitTableCharacters;
    public List<SavedCharacter> CharactersInShop = null;
    public int AttractedCharactersCount;
    public Button reRollButton;
    public TextAsset NamesMFile;
    public TextAsset NamesWFile;
    [SerializeField] private PortraitBar portraitBar;
    [SerializeField] private GameUi gameUI;
    private Data _data;
    private int CharacterLevelChar = 0;
    private List<string> NamesM = new List<string>();
    private List<string> NamesW = new List<string>();

    private void OnEnable()
    {
        if (_data == null)
        {
            _data = Data.Instance;
        }
    }

    public void RecruitmentStart()
    {
        ReadString(NamesM, NamesMFile);
        ReadString(NamesW, NamesWFile);
        if (_data.townData.day > 1)
        {
            AttractedCharactersCount = 2;
            int townHallChar = _data.townData.townHall.attractedCharactersCount;
            CharacterLevelChar = _data.townData.townHall.attractedCharacterLevel;
            if (townHallChar == 1)
            {
                AttractedCharactersCount = 3;
            }
            else if(townHallChar == 2)
            {
                AttractedCharactersCount = 4;
            }
            else if (townHallChar == 3)
            {
                AttractedCharactersCount = 5;
            }
        }
        if(CharactersInShop == null || CharactersInShop.Count == 0)
        {
            CharactersInShop = new List<SavedCharacter>();
            CreateCharactersInShop();
        }
        UpdateButtons();
    }

    public void ReadString(List<string> stringList, TextAsset namesFile)
    {
        stringList.Clear();
        using (var file = new StringReader(namesFile.text))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                stringList.Add(line);
            }
        }
    }
    private void CreateCharactersInShop()
    {
        List<SavedCharacter> AllCharactersCopy = new List<SavedCharacter>(_data.AllAvailableCharacters);
        CharactersInShop.Clear();
        Debug.LogError("LOL");
        if(NamesW.Count <= 8)
        {
            ReadString(NamesW, NamesWFile);
        }
        if (NamesM.Count <= 8)
        {
            ReadString(NamesM, NamesMFile);
        }
        for (int i = 0; i < AttractedCharactersCount; i++)
        {
            int randomIndex = Random.Range(0, AllCharactersCopy.Count);
            SavedCharacter characterToAdd = new SavedCharacter(AllCharactersCopy[randomIndex]);
            characterToAdd.level = 1;
            characterToAdd.xP = 0;
            AllCharactersCopy.RemoveAll(x => x.prefab == characterToAdd.prefab);
            List<string> NameList;
            if (characterToAdd.playerInformation.ClassName == "ASSASSIN" ||
                characterToAdd.playerInformation.ClassName == "ENCHANTRESS" ||
                characterToAdd.playerInformation.ClassName == "SORCERESS" ||
                characterToAdd.playerInformation.ClassName == "HUNTRESS")
            {
                NameList = NamesW;
            }
            else {
                NameList = NamesM;
            }
            int randomIndex2 = Random.Range(0, NameList.Count);
            characterToAdd.characterName = NameList[randomIndex2].ToUpper();
            NameList.RemoveAt(randomIndex2);
            //
            characterToAdd.abilityPointCount = 1;
            characterToAdd.unlockedAbilities = "0000";
            Debug.LogError("Need to redo Unlocked Abilities");
            //
            if (CharacterLevelChar == 1)
            {
                characterToAdd.level = 2;
                characterToAdd.abilityPointCount = 2;
            }
            //
            CharactersInShop.Add(characterToAdd);
        }
    }
    public void UpdateButtons()
    {
        UpdateRerollButton();
        for (int i = 0; i < recruitTableCharacters.Count; i++)
        {
            if (i < CharactersInShop.Count)
            {
                recruitTableCharacters[i].character = CharactersInShop[i];
                recruitTableCharacters[i].gameObject.SetActive(true);
                recruitTableCharacters[i].UpdateRecruitButton();
            }
            else
            {
                recruitTableCharacters[i].character = null;
                recruitTableCharacters[i].gameObject.SetActive(false);
            }
            
        }
    }
    private void UpdateRerollButton()
    {
        if (_data.townData.townHall != null)
        {
            int townHallChar = _data.townData.townHall.characterReRoll;
            reRollButton.interactable = (townHallChar == 1);
        }
    }
    
    public void BuyCharacter(int index)
    {
        if (_data.Characters.Count < _data.maxCharacterCount)
        {
            SavedCharacter savedCharacter = recruitTableCharacters[index].character;
            if (_data.canButtonsBeClicked)
            {
                
                _data.InsertCharacter(savedCharacter);
                _data.townData.townGold -= savedCharacter.cost;
                gameUI.EnableGoldChange("-" + savedCharacter.cost + "g");
                gameUI.UpdateTownCost();
                portraitBar.InsertCharacter();
                gameUI.UpdateUnspentPointWarnings();
                gameUI.UpdateBuyRecruitsWarning();
                _data.statistics.charactersBoughtCountByClass[Statistics.GetClassIndex(savedCharacter.playerInformation.ClassName)]++;
                _data.globalStatistics.charactersBoughtCountByClass[Statistics.GetClassIndex(savedCharacter.playerInformation.ClassName)]++;
            
            }
            CharactersInShop.Remove(savedCharacter);
            UpdateButtons();
        }
        else Debug.Log("Ziurek ka darai, kvaily!");
    }
    
    public void Reroll()
    {
        AttractedCharactersCount = CharactersInShop.Count;
        CreateCharactersInShop();
        UpdateButtons();
    }

    public SavedCharacter GetInShopCharacterByIndex(int index)
    {
        return CharactersInShop[index];
    }
    
}
