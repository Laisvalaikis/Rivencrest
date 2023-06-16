using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    [SerializeField] private Data _data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //SAVESYSTEM
    public void SaveTownData()
    {

        TownData townData = _data.townData;
        _data.selectedEnemies = new List<int>();
        bool allowEnemySelection = false;
        bool allowDuplicates = false;
        if (SceneManager.GetActiveScene().name == "CharacterSelect3")
        {
            GetComponent<CharacterSelect>().SaveData();
            allowEnemySelection = true;
            allowDuplicates = GetComponent<CharacterSelect>().allowDuplicates;
        }
        List<SavedCharacter> RCcharacters = new List<SavedCharacter>();
        if (SceneManager.GetActiveScene().name == "Town")
        {
            RCcharacters = GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop;
        }
        else if (SaveSystem.DoesSaveFileExist() && !_data.createNewRCcharacters)
        {
            townData.rcCharacters.ForEach(savableCharacter => RCcharacters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
            //TownData loadedData = SaveSystem.LoadTownData();
            //for (int i = 0; i < townData.rcCharacters.Count; i++)
            //{
            //    SavedCharacter newCharacter = AllAvailableCharacters[loadedData.RCcharacters[i]];
            //    newCharacter.characterName = loadedData.RCcharacterNames[i];
            //    newCharacter.level = loadedData.RCcharacterLevels[i];
            //    newCharacter.XP = 0;
            //    newCharacter.abilityPointCount = newCharacter.level;
            //    newCharacter.unlockedAbilities = "0000";
            //    RCcharacters.Add(newCharacter);
            //}
        }
        if (_data.createNewRCcharacters)
        {
            RCcharacters = null;
        }
        TownData data = new TownData(townData.difficultyLevel, townData.townGold, townData.day, _data.Characters, _data.CharactersOnLastMission,
            townData.wasLastMissionSuccessful, false, townData.singlePlayer, townData.selectedMission, townData.townHall, RCcharacters,
            _data.selectedEnemies, allowEnemySelection, allowDuplicates, SaveSystem.LoadTownData().teamColor,
            townData.slotName, townData.selectedEncounter, townData.pastEncounters, townData.generateNewEncounters, townData.generatedEncounters, townData.gameSettings);
        SaveSystem.SaveTownData(data);
        SaveSystem.SaveStatistics(_data.statistics);
        SaveSystem.SaveStatistics(_data.globalStatistics, true);
    }
    
    public void LoadTownData()
    {
        _data.townData = SaveSystem.LoadTownData();
    
        //difficultyLevel = townData.difficultyLevel;
        //townGold = townData.townGold;
        //day = townData.day;
        _data.Characters.Clear();
        _data.CharactersOnLastMission.Clear();
  
        _data.townData.characters.ForEach(savableCharacter => _data.Characters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
        //if (townData.wereCharactersOnAMission)
        //{
        //    for (int i = 0; i < townData.charactersOnLastMission.Length; i++)
        //    {
        //        CharactersOnLastMission.Add(townData.charactersOnLastMission[i]);
        //    }
        //    wasLastMissionSuccessful = townData.wasLastMissionSuccessful;
        //}
        _data.CharactersOnLastMission = new List<int>(_data.townData.charactersOnLastMission);
        //singlePlayer = townData.singlePlayer;
        //townHall = townData.townHall;
        //RC
        if (SceneManager.GetActiveScene().name == "Town" && !_data.townData.createNewRCcharacters && !_data.townData.newGame)
        {
            //Debug.Log("Loading RC data");
            List<SavedCharacter> RCcharacters = new List<SavedCharacter>();
            //for (int i = 0; i < townData.RCcharacters.Length; i++)
            //{
            //    SavedCharacter newCharacter = AllAvailableCharacters[townData.RCcharacters[i]];
            //    newCharacter.characterName = townData.RCcharacterNames[i];
            //    newCharacter.level = townData.RCcharacterLevels[i];
            //    newCharacter.XP = 0;
            //    newCharacter.abilityPointCount = newCharacter.level;
            //    newCharacter.unlockedAbilities = "0000";
            //    RCcharacters.Add(newCharacter);
            //}
            _data.townData.rcCharacters.ForEach(savableCharacter => RCcharacters.Add(new SavedCharacter(savableCharacter, _data.AllAvailableCharacters[savableCharacter.prefabIndex].prefab)));
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = RCcharacters;
        }
        else if (SceneManager.GetActiveScene().name == "Town" && (_data.townData.createNewRCcharacters || _data.townData.newGame))
        {
            GameObject.Find("CanvasCamera").transform.Find("RecruitmentCenterTable").GetComponent<Recruitment>().CharactersInShop = null;
        }
        _data.globalStatistics = SaveSystem.LoadStatistics(true);
        _data.statistics = SaveSystem.LoadStatistics();
        //selectedMission = townData.selectedMission;
    }
    
}
