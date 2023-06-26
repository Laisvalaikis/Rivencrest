using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Data : MonoBehaviour
{
    public static Data Instance { get; private set; }
    public List<SavedCharacter> Characters;
    public List<SavedCharacter> AllAvailableCharacters;
    public List<GameObject> AllEnemyCharacterPrefabs;
    public List<SavedCharacter> AllEnemySavedCharacters;
    [HideInInspector] public List<int> CharactersOnLastMission;
    //[HideInInspector] public bool wasLastMissionSuccessful;
    public TownData newGameData;
    [HideInInspector] public bool canButtonsBeClicked = true;
    public bool canButtonsBeClickedState = true;
    public List<int> XPToLevelUp;
    public bool isCurrentScenePlayableMap = false;
    [HideInInspector] public bool switchPortraits;
    [HideInInspector] public List<int> SwitchedCharacters;
    [HideInInspector] public int currentCharacterIndex = -1;
    public int maxCharacterCount;
    [HideInInspector] public bool createNewRCcharacters = false;
    public List<int> selectedEnemies;
    [HideInInspector] public Statistics statistics;
    [HideInInspector] public Statistics globalStatistics;
    /*[HideInInspector]*/
    public TownData townData;

    public UnityEvent characterRecruitmentEvent;
    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InsertCharacter(SavedCharacter character)
    {
        Characters.Add(character);
        characterRecruitmentEvent.Invoke();
    }
}
