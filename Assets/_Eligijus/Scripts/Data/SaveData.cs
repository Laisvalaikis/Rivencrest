using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{

    private Data _data;
    private List<SavedCharacter> _recruitCharacters;
    private bool _allowEnemySelection = false;
    private bool _allowDuplicates = false;

    private void Start()
    {
        _data = Data.Instance;
        LoadData();
    }

    //SAVESYSTEM
    private void SaveGameData()
    {
        TownData townData = _data.townData;
        _data.selectedEnemies = new List<int>();
        TownData data = new TownData(townData.difficultyLevel, townData.townGold, townData.day, _data.Characters, _data.CharactersOnLastMission,
            townData.wasLastMissionSuccessful, false, townData.singlePlayer, townData.selectedMission, townData.townHall, _recruitCharacters,
            _data.selectedEnemies, _allowEnemySelection, _allowDuplicates, SaveSystem.LoadTownData().teamColor,
            townData.slotName, townData.selectedEncounter, townData.pastEncounters, townData.generateNewEncounters, townData.generatedEncounters, townData.gameSettings);
        SaveSystem.SaveTownData(data);
        SaveSystem.SaveStatistics(_data.statistics);
        SaveSystem.SaveStatistics(_data.globalStatistics, true);
    }

    public void SaveTownData()
    {
        _allowEnemySelection = false;
        _allowDuplicates = false;
        _recruitCharacters = GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop;
        SaveGameData();
    }

    public void SaveCharacterSelectData()
    {
        GetComponent<CharacterSelect>().SaveData();
        _allowEnemySelection = true;
        _allowDuplicates = GetComponent<CharacterSelect>().allowDuplicates;
        SetupRecruitmentCharacters();
        SaveGameData();
    }
    

    public void SetupRecruitmentCharacters()
    {
        if (SaveSystem.DoesSaveFileExist() && !_data.createNewRCcharacters)
        {
            TownData townData = _data.townData;
            townData.rcCharacters.ForEach(savableCharacter => _recruitCharacters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
        }
        if (_data.createNewRCcharacters)
        {
            _recruitCharacters = null;
        }
    }

    public void OtherSaveData()
    {
        _allowEnemySelection = false;
        _allowDuplicates = false;
        SetupRecruitmentCharacters();
        SaveGameData();
    }

    public void LoadTownData()
    {
        _data.townData = SaveSystem.LoadTownData();
        _data.Characters.Clear();
        _data.CharactersOnLastMission.Clear();
  
        _data.townData.characters.ForEach(savableCharacter => _data.Characters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
        _data.CharactersOnLastMission = new List<int>(_data.townData.charactersOnLastMission);
        //RC
        if (SceneManager.GetActiveScene().name == "Town" && !_data.townData.createNewRCcharacters && !_data.townData.newGame)
        {
            List<SavedCharacter> RCcharacters = new List<SavedCharacter>();
            _data.townData.rcCharacters.ForEach(savableCharacter => RCcharacters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = RCcharacters;
        }
        else if (SceneManager.GetActiveScene().name == "Town" && (_data.townData.createNewRCcharacters || _data.townData.newGame))
        {
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = null;
        }
        _data.globalStatistics = SaveSystem.LoadStatistics(true);
        _data.statistics = SaveSystem.LoadStatistics();
    }

    public void LoadTownDataNew()
    {
        _data.townData = SaveSystem.LoadTownData();
        _data.Characters.Clear();
        _data.CharactersOnLastMission.Clear();
        _data.townData.characters.ForEach(savableCharacter => _data.Characters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
        _data.CharactersOnLastMission = new List<int>(_data.townData.charactersOnLastMission);
        LoadRecruitmentCenter();
        _data.globalStatistics = SaveSystem.LoadStatistics(true);
        _data.statistics = SaveSystem.LoadStatistics();
    }
    
    public void LoadData()
    {
        _data.townData = SaveSystem.LoadTownData();
        _data.Characters.Clear();
        _data.CharactersOnLastMission.Clear();
        _data.townData.characters.ForEach(savableCharacter => _data.Characters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
        _data.CharactersOnLastMission = new List<int>(_data.townData.charactersOnLastMission);
        _data.globalStatistics = SaveSystem.LoadStatistics(true);
        _data.statistics = SaveSystem.LoadStatistics();
    }

    public void LoadRecruitmentCenter()
    {
        if (!_data.townData.createNewRCcharacters && !_data.townData.newGame)
        {
            List<SavedCharacter> RCcharacters = new List<SavedCharacter>();
            _data.townData.rcCharacters.ForEach(savableCharacter => RCcharacters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = RCcharacters;
        }
        else if (_data.townData.createNewRCcharacters || _data.townData.newGame)
        {
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = null;
        }
    }

}
