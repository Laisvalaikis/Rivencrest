﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UsedAbility
{
    public BaseAction Ability { get; set; }
    public ChunkData Chunk { get; set; }

    public UsedAbility(BaseAction ability, ChunkData chunk)
    {
        Ability = ability;
        Chunk = chunk;
    }
}
public class PlayerTeams : MonoBehaviour
{
    private GameInformation gameInformation;
    private GameProgress gameProgress;
    public TeamInformation portraitTeamBox;
    public TeamsList allCharacterList;
    public string ChampionTeam;
    public string ChampionAllegiance;
    public int undoCount = 2;
    public List<GameObject> otherCharacters = new List<GameObject>();
    public bool isGameOver;
    public HelpTable helpTable;
    public ButtonManager characterUiButtonManager;
    public MapSetup mapSetup;
    private TeamsList currentCharacters;
    [SerializeField] private ColorManager colorManager;
    private Data _data;
    void Start()
    {
        _data = Data.Instance;
        allCharacterList.teams[0].characters.Clear();
        for (int i = 0; i < _data.Characters.Count; i++)
        {
            allCharacterList.teams[0].characters.Add(_data.Characters[i].prefab);
        }
        // mapSetup.SetupAMap();
        currentCharacters = new TeamsList();
        currentCharacters.teams = new List<Team>();
        for (int i = 0; i < allCharacterList.teams.Count; i++)
        {
            currentCharacters.teams.Add(new Team());
            currentCharacters.teams[i].characters = new List<GameObject>();
            currentCharacters.teams[i].aliveCharacters = new List<GameObject>();
            currentCharacters.teams[i].aliveCharactersPlayerInformation = new List<PlayerInformation>();
            SpawnCharacters(i, allCharacterList.teams[i].coordinates);
        }
        
        //if (GameObject.Find("GameProgress") != null)
        if(gameInformation!=null)
        {
            //GameObject.Find("GameProgress").GetComponent<GameProgress>().SetSavedCharactersOnPrefabs();
            gameProgress.SetSavedCharactersOnPrefabs();
        }
        
        isGameOver = false;
    }
    private void SpawnCharacters(int teamIndex, List<Vector3> coordinates)
    {
        var spawnCoordinates = coordinates;
        // portraitTeamBox.teamIndex = teamIndex;
        colorManager.SetPortraitBoxSprites(portraitTeamBox.gameObject, allCharacterList.teams[teamIndex].teamName);// priskiria spalvas mygtukams ir paciam portraitboxui
        //Portrait box color
        //portraitBox.GetComponent<Image>().sprite = allCharacterList.teams[teamIndex].teamPortraitBoxSprite;
        int i = 0;
        foreach (var x in spawnCoordinates)
        {
            if (i < allCharacterList.teams[teamIndex].characters.Count)
            {
                GameObject spawnedCharacter = Instantiate(allCharacterList.teams[teamIndex].characters[i], new Vector3(x.x, x.y, 0f), Quaternion.identity); //Spawning the prefab into the scene.
                PlayerInformation playerInformation = spawnedCharacter.GetComponent<PlayerInformation>();
                playerInformation.SetPlayerTeam(teamIndex);
                GameTileMap.Tilemap.SetCharacter(spawnedCharacter.transform.position + new Vector3(0, 0.5f, 0), spawnedCharacter, playerInformation);
                currentCharacters.teams[teamIndex].characters.Add(spawnedCharacter);
                currentCharacters.teams[teamIndex].aliveCharacters.Add(spawnedCharacter);
                currentCharacters.teams[teamIndex].aliveCharactersPlayerInformation.Add(playerInformation); 
                // if(allCharacterList.teams[teamIndex].isTeamAI)
        //         {
        //             int points = 2 * (_data.townData.selectedEncounter.encounterLevel - 1);
        //             spawnedCharacter.GetComponent<PlayerInformation>().MaxHealth += points;
        //             Debug.Log("Player " + spawnedCharacter.name + " received additional " + points + " points.");
        //         }
        //         
        //         spawnedCharacter.GetComponent<PlayerInformation>()._data = _data;
        //         spawnedCharacter.GetComponent<AIBehaviour>()._data = _data;
        //         spawnedCharacter.GetComponent<PlayerInformation>().CharactersTeam = allCharacterList.teams[teamIndex].teamName; //Assigning the characters their team.
        //         allCharacterList.teams[teamIndex].characters[i] = spawnedCharacter; //Turning prefabs into gameObjects on board.
        //         if (!characterUiButtonManager.gameObject.activeInHierarchy)
        //         {
        //             characterUiButtonManager.gameObject.SetActive(true);
        //         }
        //         characterUiButtonManager.AddDataToActionButtons(helpTable);
        //         characterUiButtonManager.CharacterOnBoard = allCharacterList.teams[teamIndex].characters[i];//Assigning character to its UI button manager
        //         // characterUiButtonManager.GetComponent<BottomCornerUI>().characterUiData = spawnedCharacter.GetComponent<PlayerInformation>().characterUiData;
        //         
        //         characterUiButtonManager.GetComponent<BottomCornerUI>().EnableAbilities(spawnedCharacter.GetComponent<PlayerInformation>().savedCharacter);
        //         spawnedCharacter.GetComponent<PlayerInformation>().cornerPortraitBoxInGame = characterUiButtonManager.gameObject;//Assigning UI button manager character to its character
        //         if (allCharacterList.teams[teamIndex].isTeamAI)
        //         {
        //             spawnedCharacter.GetComponent<PlayerInformation>().Respawn = true;
        //         }
                
            }
            i++;
        }
        allCharacterList.teams[teamIndex].undoCount = undoCount;
        portraitTeamBox.ModifyList();

        allCharacterList.teams[teamIndex].lastSelectedPlayer = allCharacterList.teams[teamIndex].characters[0];//LastSelectedPlayer
        //portraitBox.SetActive(false);
        //dar veliavas reiketu paspawnint, bet pirma jas sutvarkyt.
    }
    public void  SpawnDefaultCharacter(GameObject character) //spawns cornerUI for character not in teams list
    {
        if (!otherCharacters.Contains(character))
        {
            otherCharacters.Add(character);
        }
        // GameObject cornerUIManager = Instantiate(character.GetComponent<PlayerInformation>().CornerUIManager, CornerUIManagerContainer); //Spawning cornerUI of each player.
        // character.GetComponent<PlayerInformation>().CornerUIManager = cornerUIManager; //Turning prefabs into gameObjects in canvas.
        characterUiButtonManager.CharacterOnBoard = character;//Assigning character to its UI button manager
        Debug.Log("Cia kazkas daroma su Ui corner manager");
        AddCharacterToCurrentTeam(character);
    }
    public void AddCharacterToCurrentTeam(GameObject character)
    {
        //int teamIndex = GetComponent<GameInformation>().activeTeamIndex;
        int teamIndex = gameInformation.activeTeamIndex;
        /*//Object
        if (character.GetComponent<PlayerInformation>().isThisObject)
        {
            character.GetComponent<PlayerInformation>().CharactersTeam = allCharacterList.teams[teamIndex].teamName;
            character.GetComponent<PlayerInformation>().PlayerSetup();
            GetComponent<GameInformation>().ChangeVisionTiles();
        }
        //Character
        else */
        {
            if (allCharacterList.teams[teamIndex].characters.Count < 8)
            {
                if (otherCharacters.Contains(character))
                {
                    otherCharacters.Remove(character);
                }
                int i = allCharacterList.teams[teamIndex].characters.Count;
                allCharacterList.teams[teamIndex].characters.Add(character);
                character.GetComponent<PlayerInformation>().CharactersTeam = allCharacterList.teams[teamIndex].teamName;
                GameObject portraitBox = allCharacterList.teams[teamIndex].teamPortraitBoxGameObject;
                portraitBox.GetComponent<TeamInformation>().ModifyList();
                character.GetComponent<PlayerInformation>().PlayerSetup();
                GetComponent<GameInformation>().ChangeVisionTiles();
            }
        }
    }
    public bool IsGameOver()
    {
        if(allCharacterList.teams.Count == 1) {
            return false;
        }
        int activeTeams = 0;
        List<string> activeAllegiances = new List<string>();
        for (int i = 0; i < allCharacterList.teams.Count; i++)
        {
            int activeCharacters = 0;
            for (int j = 0; j < allCharacterList.teams[i].characters.Count; j++)
            {
                if (allCharacterList.teams[i].characters[j].GetComponent<PlayerInformation>().health > 0)
                    activeCharacters++;
            }
            if (activeCharacters > 0)
            {
                bool alreadyAdded = false;
                for(int j = 0; j < activeAllegiances.Count; j++)
                {
                    if (activeAllegiances[j] == allCharacterList.teams[i].teamAllegiance)
                        alreadyAdded = true;
                }
                if (alreadyAdded == false)
                {
                    activeAllegiances.Add(allCharacterList.teams[i].teamAllegiance);
                    ChampionAllegiance = allCharacterList.teams[i].teamAllegiance;
                }
                activeTeams++;
                ChampionTeam = allCharacterList.teams[i].teamName;
            }
        }
        if (activeTeams == 0 || activeTeams > 1)
            ChampionTeam = null;
        if (activeAllegiances.Count == 0 || activeAllegiances.Count > 1)
            ChampionAllegiance = null;
        if (activeTeams <= 1 || activeAllegiances.Count <= 1)
        {
            GameObject.Find("Canvas").transform.Find("EnemyTurnText").gameObject.SetActive(false);
            isGameOver = true;
            return true;
        }
        else return false;
    }
    // public List<GameObject> AliveCharacterList(int teamIndex)
    // {
    //     List<GameObject> aliveCharacterList = new List<GameObject>();
    //     for (int j = 0; j < allCharacterList.teams[teamIndex].characters.Count; j++)
    //     {
    //         if (allCharacterList.teams[teamIndex].characters[j].GetComponent<PlayerInformation>().health > 0)
    //         {
    //             aliveCharacterList.Add(allCharacterList.teams[teamIndex].characters[j]);
    //         }
    //     }
    //     return currentCharacters.teams[teamIndex].characters;
    // }
    
    
    public List<GameObject> AliveCharacterList(int teamIndex)
    {
        return currentCharacters.teams[teamIndex].aliveCharacters;
    }
    
    public List<PlayerInformation> AliveCharacterPlayerInformationList(int teamIndex)
    {
        return currentCharacters.teams[teamIndex].aliveCharactersPlayerInformation;
    }
    
    public GameObject FirstAliveCharacter(int teamIndex)
    {
        if (allCharacterList.teams[teamIndex].lastSelectedPlayer.GetComponent<PlayerInformation>().health > 0)
        {
            return allCharacterList.teams[teamIndex].lastSelectedPlayer;
        }
        for (int j = 0; j < allCharacterList.teams[teamIndex].characters.Count; j++)
        {
            if (allCharacterList.teams[teamIndex].characters[j].GetComponent<PlayerInformation>().health > 0)
            {
                return allCharacterList.teams[teamIndex].characters[j];
            }
        }
        return null;
    }
    public string FindTeamAllegiance(string teamName)
    {
        for(int i = 0;i< allCharacterList.teams.Count; i++)
        {
            if(allCharacterList.teams[i].teamName == teamName)
            {
                return allCharacterList.teams[i].teamAllegiance;
            }
        }
        return "";
    }
    public void RemoveCharacterFromTeam(GameObject character, string teamName)
    {
        if(allCharacterList.teams.Find(x => x.teamName == teamName) != null)
        {
            allCharacterList.teams.Find(x => x.teamName == teamName).characters.Remove(character);
        }
    }

    public List<GameObject> GetAllCharacters() 
    {
        List<GameObject> characters = new List<GameObject>();

        for (int i = 0; i < allCharacterList.teams.Count; i++)
        {
            for (int j = 0; j < allCharacterList.teams[i].characters.Count; j++)
            {
                characters.Add(allCharacterList.teams[i].characters[j]);
            }
        }
        foreach (GameObject x in otherCharacters)
        {
            characters.Add(x);
        }
        return characters;
    }
}
[System.Serializable]
public class Team
{
    public List<GameObject> characters;
    public List<GameObject> aliveCharacters;
    public List<PlayerInformation> aliveCharactersPlayerInformation;
    public List<Vector3> coordinates;
    public List<UsedAbility> usedAbilities = new List<UsedAbility>();
    public string teamName;
    public string teamAllegiance;
    public GameObject teamFlag;
    public Sprite teamFlagSprite;
    public GameObject teamFlagPoint;
    public bool isTeamAI;
    [HideInInspector] public int undoCount;
    [HideInInspector] public GameObject teamPortraitBoxGameObject;
    [HideInInspector] public GameObject lastSelectedPlayer;

}

[System.Serializable]
public class TeamsList
{
    public List<Team> teams;
}
