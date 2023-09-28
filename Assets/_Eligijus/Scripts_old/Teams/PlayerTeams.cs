using System.Collections;
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
    [SerializeField] private TurnManager TurnManager;
    private GameInformation gameInformation;
    private GameProgress gameProgress;
    public TeamInformation portraitTeamBox;
    public TeamsList allCharacterList;
    public string ChampionTeam;
    public string ChampionAllegiance;
    public int undoCount = 2;
    public List<GameObject> otherCharacters = new List<GameObject>();
    public bool isGameOver;
    public ButtonManager characterUiButtonManager;
    private TeamsList currentCharacters;
    [SerializeField] private ColorManager colorManager;
    private Data _data;
    
    void Start()
    {
        InitializeCharacterLists();
        SpawnAllCharacters();
        if(gameInformation!=null)
        {
            gameProgress.SetSavedCharactersOnPrefabs();
        }
        isGameOver = false;
    }
    
    
    private void InitializeCharacterLists()
    {
        _data = Data.Instance;
        allCharacterList.Teams[0].characters.Clear();
        foreach (var t in _data.Characters)
        {
            allCharacterList.Teams[0].characters.Add(t.prefab);
        }
        currentCharacters = new TeamsList { Teams = new List<Team>() };
    }
    
    private void SpawnAllCharacters()
    {
        for (int i = 0; i < allCharacterList.Teams.Count; i++)
        {
            currentCharacters.Teams.Add(new Team());
            currentCharacters.Teams[i].characters = new List<GameObject>();
            currentCharacters.Teams[i].aliveCharacters = new List<GameObject>();
            currentCharacters.Teams[i].aliveCharactersPlayerInformation = new List<PlayerInformation>();
            SpawnCharacters(i, allCharacterList.Teams[i].coordinates);
        }
    }
    private void SpawnCharacters(int teamIndex, List<Vector3> coordinates)
    {
        var spawnCoordinates = coordinates;
        colorManager.SetPortraitBoxSprites(portraitTeamBox.gameObject, allCharacterList.Teams[teamIndex].teamName);// priskiria spalvas mygtukams ir paciam portraitboxui
        int i = 0;
        foreach (var x in spawnCoordinates)
        {
            if (i < allCharacterList.Teams[teamIndex].characters.Count)
            {
                GameObject spawnedCharacter = Instantiate(allCharacterList.Teams[teamIndex].characters[i], new Vector3(x.x, x.y, 0f), Quaternion.identity); //Spawning the prefab into the scene.
                PlayerInformation playerInformation = spawnedCharacter.GetComponent<PlayerInformation>();
                playerInformation.SetPlayerTeam(teamIndex);
                GameTileMap.Tilemap.SetCharacter(spawnedCharacter.transform.position + new Vector3(0, 0.5f, 0), spawnedCharacter, playerInformation);
                currentCharacters.Teams[teamIndex].characters.Add(spawnedCharacter);
                currentCharacters.Teams[teamIndex].aliveCharacters.Add(spawnedCharacter);
                currentCharacters.Teams[teamIndex].aliveCharactersPlayerInformation.Add(playerInformation);
            }
            i++;
        }
        allCharacterList.Teams[teamIndex].undoCount = undoCount;
        portraitTeamBox.ModifyList();

        allCharacterList.Teams[teamIndex].lastSelectedPlayer = allCharacterList.Teams[teamIndex].characters[0];//LastSelectedPlayer
    }
    public void  SpawnDefaultCharacter(GameObject character) //spawns cornerUI for character not in teams list
    {
        if (!otherCharacters.Contains(character))
        {
            otherCharacters.Add(character);
        }
        AddCharacterToCurrentTeam(character);
    }
    public void AddCharacterToCurrentTeam(GameObject character)
    {
        Team currentTeam = TurnManager.GetCurrentTeam();
        int teamIndex = gameInformation.activeTeamIndex;
        if (currentTeam.characters.Count < 8) 
        {
            if (otherCharacters.Contains(character))
            {
                otherCharacters.Remove(character);
            }
            currentTeam.characters.Add(character);
            PlayerInformation playerInformation = character.GetComponent<PlayerInformation>();
            playerInformation.CharactersTeam = allCharacterList.Teams[teamIndex].teamName;
            GameObject portraitBox = allCharacterList.Teams[teamIndex].teamPortraitBoxGameObject;
            portraitBox.GetComponent<TeamInformation>().ModifyList();
            playerInformation.PlayerSetup();
            //todo: maybe update fog of war
        }
    }
    public List<GameObject> AliveCharacterList(int teamIndex)
    {
        return currentCharacters.Teams[teamIndex].aliveCharacters;
    }
    public List<PlayerInformation> AliveCharacterPlayerInformationList(int teamIndex)
    {
        return currentCharacters.Teams[teamIndex].aliveCharactersPlayerInformation;
    }
    public GameObject FirstAliveCharacter(int teamIndex)
    {
        if (allCharacterList.Teams[teamIndex].lastSelectedPlayer.GetComponent<PlayerInformation>().GetHealth() > 0)
        {
            return allCharacterList.Teams[teamIndex].lastSelectedPlayer;
        }
        for (int j = 0; j < allCharacterList.Teams[teamIndex].characters.Count; j++)
        {
            if (allCharacterList.Teams[teamIndex].characters[j].GetComponent<PlayerInformation>().GetHealth() > 0)
            {
                return allCharacterList.Teams[teamIndex].characters[j];
            }
        }
        return null;
    }
    public string FindTeamAllegiance(string teamName)
    {
        for(int i = 0;i< allCharacterList.Teams.Count; i++)
        {
            if(allCharacterList.Teams[i].teamName == teamName)
            {
                return allCharacterList.Teams[i].teamAllegiance;
            }
        }
        return "";
    }
    public void RemoveCharacterFromTeam(GameObject character, string teamName)
    {
        if(allCharacterList.Teams.Find(x => x.teamName == teamName) != null)
        {
            allCharacterList.Teams.Find(x => x.teamName == teamName).characters.Remove(character);
        }
    }

    public List<GameObject> GetAllCharacters() 
    {
        List<GameObject> characters = new List<GameObject>();

        for (int i = 0; i < allCharacterList.Teams.Count; i++)
        {
            for (int j = 0; j < allCharacterList.Teams[i].characters.Count; j++)
            {
                characters.Add(allCharacterList.Teams[i].characters[j]);
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
    public bool isTeamAI;
    [HideInInspector] public int undoCount;
    [HideInInspector] public GameObject teamPortraitBoxGameObject;
    [HideInInspector] public GameObject lastSelectedPlayer;

}

[System.Serializable]
public class TeamsList
{
    public List<Team> Teams;
}
