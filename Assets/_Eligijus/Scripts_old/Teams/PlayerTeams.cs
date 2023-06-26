using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeams : MonoBehaviour
{
    public GameObject portraitBoxPrefab;
    public Transform portraitBoxContainer;
    public Transform CornerUIManagerContainer;
    public TeamsList allCharacterList;
    public string ChampionTeam;
    public string ChampionAllegiance;
    public int undoCount = 2;
    public List<GameObject> otherCharacters = new List<GameObject>();
    public bool isGameOver;
    public Data _data;
    public HelpTableController helpTableController;
    public ButtonManager characterUiButtonManager;
    void Start()
    {
        if(GameObject.Find("GameProgress") != null)
        {
            GameObject.Find("GameProgress").GetComponent<MapSetup>().SetupAMap();
        }
        for (int i = 0; i < allCharacterList.teams.Count; i++)
        {
            SpawnCharacters(i, TransformToFloats(allCharacterList.teams[i].coordinates));
        }

        //
        if (GameObject.Find("PVPManager") != null)
        {
            GameObject.Find("PVPManager").GetComponent<PVPManager>().ApplyAbilitySelections();
        }
        //

        if (GameObject.Find("GameProgress") != null)
        {
            GameObject.Find("GameProgress").GetComponent<GameProgress>().SetSavedCharactersOnPrefabs();
        }
        GetComponent<GameInformation>().ChangeActiveTeam(allCharacterList.teams[0].teamName);
        GetComponent<GameInformation>().ChangeVisionTiles();
        isGameOver = false;
    }
    private void SpawnCharacters(int teamIndex, List<(float, float)> coordinates)
    {
        var spawnCoordinates = coordinates;
        int i = 0;
        GameObject portraitBox = Instantiate(portraitBoxPrefab, portraitBoxContainer);
        portraitBox.GetComponent<TeamInformation>().teamIndex = teamIndex;
        GetComponent<ColorManager>().SetPortraitBoxSprites(portraitBox, allCharacterList.teams[teamIndex].teamName);// priskiria spalvas mygtukams ir paciam portraitboxui
        //Portrait box color
        //portraitBox.GetComponent<Image>().sprite = allCharacterList.teams[teamIndex].teamPortraitBoxSprite;
        if (allCharacterList.teams[teamIndex].teamFlag != null)
        {
            allCharacterList.teams[teamIndex].teamFlag.GetComponent<SpriteRenderer>().sprite = allCharacterList.teams[teamIndex].teamFlagSprite;
            allCharacterList.teams[teamIndex].teamFlag.transform.Find("FlagCollider").GetComponent<Flag>().FlagsTeam = allCharacterList.teams[teamIndex].teamName;
            allCharacterList.teams[teamIndex].teamFlagPoint.transform.Find("FlagPoint").GetComponent<FlagPoint>().team = allCharacterList.teams[teamIndex].teamName;
        }
        allCharacterList.teams[teamIndex].teamPortraitBoxGameObject = portraitBox;
        foreach (var x in spawnCoordinates)
        {
            if (i < allCharacterList.teams[teamIndex].characters.Count)
            {
                GameObject spawnedCharacter = Instantiate(allCharacterList.teams[teamIndex].characters[i], new Vector3(x.Item1, x.Item2, 0f), Quaternion.identity); //Spawning the prefab into the scene.
                if(allCharacterList.teams[teamIndex].isTeamAI)
                {
                    int points = 2 * (_data.townData.selectedEncounter.encounterLevel - 1);
                    spawnedCharacter.GetComponent<PlayerInformation>().MaxHealth += points;
                    Debug.Log("Player " + spawnedCharacter.name + " received additional " + points + " points.");
                }
                
                spawnedCharacter.GetComponent<PlayerInformation>()._data = _data;
                spawnedCharacter.GetComponent<AIBehaviour>()._data = _data;
                spawnedCharacter.GetComponent<PlayerInformation>().CharactersTeam = allCharacterList.teams[teamIndex].teamName; //Assigning the characters their team.
                allCharacterList.teams[teamIndex].characters[i] = spawnedCharacter; //Turning prefabs into gameObjects on board.
                if (!characterUiButtonManager.gameObject.activeInHierarchy)
                {
                    characterUiButtonManager.gameObject.SetActive(true);
                }
                characterUiButtonManager.AddDataToActionButtons(helpTableController);
                characterUiButtonManager.CharacterOnBoard = allCharacterList.teams[teamIndex].characters[i];//Assigning character to its UI button manager
                characterUiButtonManager.GetComponent<BottomCornerUI>().characterUiData = spawnedCharacter.GetComponent<PlayerInformation>().characterUiData;
                
                characterUiButtonManager.GetComponent<BottomCornerUI>().EnableAbilities(spawnedCharacter.GetComponent<PlayerInformation>().savedCharacter);
                spawnedCharacter.GetComponent<PlayerInformation>().cornerPortraitBoxInGame = characterUiButtonManager.gameObject;//Assigning UI button manager character to its character
                if (allCharacterList.teams[teamIndex].isTeamAI)
                {
                    spawnedCharacter.GetComponent<PlayerInformation>().Respawn = true;
                }
                
                Debug.Log("Cia kazkas daroma su ui corner");
            }
            i++;
        }
        allCharacterList.teams[teamIndex].undoCount = undoCount;
        portraitBox.GetComponent<TeamInformation>().ModifyList();

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
        character.GetComponent<PlayerInformation>().cornerPortraitBoxInGame = characterUiButtonManager.gameObject;//Assigning UI button manager character to its character
        Debug.Log("Cia kazkas daroma su Ui corner manager");
        AddCharacterToCurrentTeam(character);
    }
    public void AddCharacterToCurrentTeam(GameObject character)
    {
        int teamIndex = GetComponent<GameInformation>().activeTeamIndex;
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
    public List<GameObject> AliveCharacterList(int teamIndex)
    {
        List<GameObject> aliveCharacterList = new List<GameObject>();
        for (int j = 0; j < allCharacterList.teams[teamIndex].characters.Count; j++)
        {
            if (allCharacterList.teams[teamIndex].characters[j].GetComponent<PlayerInformation>().health > 0)
            {
                aliveCharacterList.Add(allCharacterList.teams[teamIndex].characters[j]);
            }
        }
        return aliveCharacterList;
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
    public List<(float,float)> TransformToFloats(List<Transform> tList)
    {
        List<(float, float)> fList = new List<(float, float)>();
        foreach(Transform x in tList)
        {
            fList.Add((x.position.x, x.position.y));
        }
        return fList;
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
public class CharacterList
{
    public List<GameObject> characters;
    public List<Transform> coordinates;
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
    public List<CharacterList> teams;
}
