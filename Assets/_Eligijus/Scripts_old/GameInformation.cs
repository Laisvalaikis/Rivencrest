using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.CompilerServices;
using TMPro;

public class GameInformation : MonoBehaviour
{
    //Bools
    [HideInInspector] public bool isVictoryScreenEnabled = false;
    [HideInInspector] public bool isEndTurnScreenEnabled = false;
    [HideInInspector] public bool isBoardDisabled;
    [HideInInspector] public bool helpTableOpen = false;
    [HideInInspector] public bool isDragAvailable = true;
    [HideInInspector] public bool respawnEnemiesDuringThisTurn;
    [HideInInspector] public bool canButtonsBeClicked = true;
    [HideInInspector] public bool isAITurn;
    private bool activateOnce = true;
    private bool portalButtonState;
    private bool addButtonState;
    private bool undoTurnButtonState;
    private bool isBoardDisabledState;
    public bool isFogOfWarEnabled = true;
    public SaveData _saveData;
    public Data _data;
    public HelpTable helpTable;
    public ButtonManager cornerButtonManager;
    public BottomCornerUI bottomCornerUI;
    public List<String> Events; // Merchant death
    //Other
    [HideInInspector] public GameObject SelectedCharacter  {
        get { return _selectedCharacter; }
        set { _selectedCharacter = value; }
    }
    public GameObject _selectedCharacter;
    [HideInInspector] public GameObject InspectedCharacter;
    [HideInInspector] public UndoAction undoAction;
    [HideInInspector] public List<EnvironmentalHazard> environmentalHazards;
    public GameObject Camera;
    private GameObject AddButton;
    public GameObject MapOverlay;

    //Floats
    [HideInInspector] public float endTurnCountDown = 0f;
    private float victoryCountDown = 2f;
    private float fadeCountDown = 1f;
    private float fadeCountDown1;

    //Strings
    [HideInInspector] public string ActiveTeam = "Red";
    public string PlayerAllegiance = "";

    //Ints
    [HideInInspector] public int activeTeamIndex = 0;
    private int activateAfter2Frames = 0;

    public static GameInformation instance;
    
    void Start()
    {
        //if (instance != null && instance != this)
       // {
          //  Destroy(this);
       // }
        //else
       // {
            instance = this;
        //}
        ActiveTeam = GetComponent<PlayerTeams>().allCharacterList.teams[0].teamName; //The team who goes first.
        AddButton = GameObject.Find("Canvas").transform.Find("AddButton")?.gameObject;
        //ChangeVisionTiles();
        activateAfter2Frames = 1;
        //kviesti is playerTeams
        //ChangeActiveTeam(ActiveTeam);
        fadeCountDown = 0.7f;
        fadeCountDown1 = fadeCountDown;
        victoryCountDown = 0.5f;
        respawnEnemiesDuringThisTurn = false;
        undoAction = new UndoAction();
        undoAction.available = false;
        GameObject.Find("Canvas").transform.Find("FadeScreen")?.gameObject.SetActive(true);
    }

    void Update()
    {
        if (activateOnce)
        {
            FakeUpdate();
            ChangeVisionTiles();
            activateOnce = false;
        }
        if (activateAfter2Frames == 1)
        {
            activateAfter2Frames++;
        }
        else if (activateAfter2Frames == 2)
        {
            FakeUpdate();
            ChangeVisionTiles();
            activateAfter2Frames = 0;
        }
        //if (Input.GetKey("q"))//cia kadangi q turetu pasirinkt spella tai turbut reik atjungt
        //{
        //    Application.Quit();
        //}
        if (Input.GetMouseButtonDown(1) && (SelectedCharacter != null || InspectedCharacter != null) && !helpTable.hasActionButtonBeenEntered)
        {
            EnableMovementAction();
        }
        // if (GameObject.Find("HelpButton") != null)
        // {
        //     if (((SelectedCharacter != null && SelectedCharacter.GetComponent<PlayerInformation>().currentState != "Movement")
        //         || (InspectedCharacter != null && InspectedCharacter.GetComponent<PlayerInformation>().currentState != "Movement")) && helpTableOpen == false)
        //     {
        //         GameObject.Find("HelpButton").GetComponent<Button>().interactable = true;
        //     }
        //     else GameObject.Find("HelpButton").GetComponent<Button>().interactable = false;
        // }
        if (isVictoryScreenEnabled)
        {
            if (victoryCountDown > 0)
            {
                victoryCountDown -= Time.deltaTime;
            }
            else
            {
                if (fadeCountDown > 0)
                {
                    fadeCountDown -= Time.deltaTime;
                    float alpha = (fadeCountDown1 - fadeCountDown) / fadeCountDown1;
                    Color original = GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("Continue").GetComponent<TextMeshProUGUI>().color;
                    GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("Continue").GetComponent<TextMeshProUGUI>().color = new Color(original.r, original.g, original.b, alpha);
                    Debug.Log("Need to fix this");
                }
                GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("Continue").gameObject.SetActive(true);
                if (Input.GetMouseButtonUp(0))
                {
                    if (SaveSystem.DoesSaveFileExist() && SaveSystem.LoadTownData().singlePlayer)
                    {
                        SceneManager.LoadScene("MissionEnd", LoadSceneMode.Single);
                    }
                    else
                    {
                        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
                    }
                }
            }
        }
        //
        if (Input.GetMouseButtonUp(0))
        {
            activateOnce = true;
            //FakeUpdate();
        }

        if (endTurnCountDown > 0)
        {
            endTurnCountDown -= Time.deltaTime;
        }

        if (isEndTurnScreenEnabled && Input.GetMouseButtonUp(0) && endTurnCountDown <= 0)
        {
            GameObject.Find("Canvas").transform.Find("PortraitBoxesContainer").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("EndTurn").gameObject.SetActive(true);
            // GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("HelpButton").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("CornerUIManagerContainer").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("PortalButton").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("EndTurnScreen").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.SetActive(true);
            StartCoroutine(ExecuteAfterTime(0.5f, () =>
            {
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = false;
            }));
            isEndTurnScreenEnabled = false;
            EndTurn();
        }
        if (Input.GetKeyDown("escape"))
        {
            if (GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.activeSelf)
                UnpauseGame();
            else PauseGame();

            FakeUpdate();
        }
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (Input.GetKeyDown("b"))
        {
            PreviewMode.EnablePreviewMode(GetComponent<PlayerTeams>().GetAllCharacters(), MapOverlay);
            //Debug.Log("Can buttons be clicked: " + canButtonsBeClicked);
        }
        if (Input.GetKeyUp("b"))
        {
            PreviewMode.DisablePreviewMode(GetComponent<PlayerTeams>().GetAllCharacters(), MapOverlay);
            //Debug.Log("Can buttons be clicked: " + canButtonsBeClicked);
        }
    }
    public void FakeUpdate()
    {
        var playerTeamsList = GetComponent<PlayerTeams>().allCharacterList;
        for (int i = 0; i < playerTeamsList.teams.Count; i++)
        {
            for (int j = 0; j < playerTeamsList.teams[i].characters.Count; j++)
            {
                // playerTeamsList.teams[i].characters[j].GetComponent<PlayerMovement>().FakeUpdate();
            }
        }
        foreach (GameObject x in GetComponent<PlayerTeams>().otherCharacters)
        {
            // x.GetComponent<PlayerMovement>().FakeUpdate();
        }
        //
        if (undoAction.available && GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].undoCount > 0)
        {
            GameObject.Find("Canvas").transform.Find("UndoTurn").GetComponent<Button>().interactable = true;
        }
        else GameObject.Find("Canvas").transform.Find("UndoTurn").GetComponent<Button>().interactable = false;
        GameObject.Find("Canvas").transform.Find("UndoTurn").Find("Frame").Find("UndoCount").GetComponent<TextMeshProUGUI>().text = "["
            + GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].undoCount + "]";
        CheckWhetherToHighlightEndTurn();
        if (GameObject.Find("Canvas").transform.Find("EndTurn").GetComponent<EndTurn>().confirmState
            && !GameObject.Find("Canvas").transform.Find("EndTurn").GetComponent<ButtonMechanics>().hovered)
        {
            GameObject.Find("Canvas").transform.Find("EndTurn").transform.Find("Text").GetComponent<Text>().text = "END TURN";
            GameObject.Find("Canvas").transform.Find("EndTurn").GetComponent<EndTurn>().confirmState = false;
        }
       // Debug.Log("Reikia sutvarkyti");
    }
    public void UndoMove()
    {
        if (undoAction.available)
        {
            undoAction.MoveBack();
            GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].undoCount--;
        }
    }

    public void EnableMovementAction()
    {
        DisableGrids();
        if (SelectedCharacter != null)
        {
            // SelectedCharacter.GetComponent<GridMovement>().EnableGrid();
            cornerButtonManager.DisableSelection(
                cornerButtonManager.ButtonList[0].transform.Find("ActionButtonFrame").gameObject); //ijungia movement frame
            
        }
        else if (InspectedCharacter != null)
        {
            // InspectedCharacter.GetComponent<GridMovement>().EnableGrid();
            cornerButtonManager.DisableSelection(
                cornerButtonManager.ButtonList[0].transform.Find("ActionButtonFrame").gameObject); //ijungia movement frame
        }
        Debug.Log("Cia kazkas daroma su corner ui manager");
    }
    public void SelectACharacter(GameObject character)
    {
        if (SelectedCharacter != null && SelectedCharacter == character)
        {
            DisableAllCharacters();
            character.GetComponent<PlayerInformation>().TeamManager.GetComponent<TeamInformation>().DisableSelectionAll();
            SelectedCharacter = null;
        }
        else
        {
            InspectedCharacter = null;
            SelectedCharacter = character;
            FocusSelectedCharacter(SelectedCharacter);
            //Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1;
            //Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1;
            DisableOtherCharacterMovement(character);
           // if (!character.GetComponent<PlayerInformation>().isThisObject)
          //  {
          cornerButtonManager.CharacterOnBoard = character;
          //      character.GetComponent<PlayerInformation>().TeamManager.GetComponent<TeamInformation>().DisableSelection(
           //     character.GetComponent<PlayerInformation>().characterPortrait);
           //     GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].lastSelectedPlayer = SelectedCharacter;
          //  }
          
           // character.GetComponent<GridMovement>().EnableGrid();
            // bottomCornerUI.characterUiData = _selectedCharacter.GetComponent<PlayerInformation>().characterUiData;
           // bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
            //character.GetComponent<PlayerInformation>().CornerUIManager.transform.GetChild(0).gameObject.SetActive(true);
            //Camera.GetComponent<CinemachineVirtualCamera>().Follow = character.transform;
        }
        AddButton?.SetActive(false);
        GetComponent<ColorManager>().MovementHighlightHover = new Color(253f/255f, 255f/255f, 59f/255f);

    }
    public void InspectACharacter(GameObject CharacterToInspect)
    {

        if (InspectedCharacter != null && InspectedCharacter == CharacterToInspect)
        {
            DisableAllCharacters();
            InspectedCharacter = null;
            AddButton?.SetActive(false);
        }
        else
        {
            DisableGrids();
            InspectedCharacter = CharacterToInspect;
            if (SelectedCharacter != null)
                DeselectTeam(SelectedCharacter);
            DisableOtherCharacterMovement(CharacterToInspect);
            // CharacterToInspect.GetComponent<GridMovement>().EnableGrid();
            cornerButtonManager.transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("cia kazkas daroma su corner ui manager");
            SelectedCharacter = null;
            cornerButtonManager.CharacterOnBoard = CharacterToInspect;
            // bottomCornerUI.characterUiData = InspectedCharacter.GetComponent<PlayerInformation>().characterUiData;
            bottomCornerUI.EnableAbilities();
            if (CharacterToInspect.GetComponent<PlayerInformation>().CharactersTeam == "Default")
            {
                AddButton?.SetActive(true);
            }
            else
            {
                AddButton?.SetActive(false);
            }
        }
        GetComponent<ColorManager>().MovementHighlightHover = new Color(203f/255f, 54f/255f, 214f/255f);
    }
    private void DisableOtherCharacterMovement(GameObject character)
    {
        DisableAllCharacters();
        //character.GetComponent<PlayerMovement>().enabled = true;
        character.GetComponent<PlayerInformation>().ToggleSelectionBorder(true);
    }
    public void DeselectTeam(GameObject character)
    {
        DisableAllCharacters();
        character.GetComponent<PlayerInformation>().TeamManager.GetComponent<TeamInformation>().DisableSelectionAll();
        SelectedCharacter = null;
    }

    private void DisableAllCharacters()
    {
        for (int i = 0; i < GetComponent<PlayerTeams>().allCharacterList.teams.Count; i++)
        {
            foreach(GameObject characterInList in GetComponent<PlayerTeams>().allCharacterList.teams[i].characters)
            {
                DisableCharacter(characterInList);
            }
            //for (int j = 0; j < GetComponent<PlayerTeams>().allCharacterList.teams[i].characters.Count; j++)
            //{
            //    GameObject characterInList = GetComponent<PlayerTeams>().allCharacterList.teams[i].characters[j];
            //    //characterInList.GetComponent<PlayerMovement>().enabled = false;
            //    DisableCharacter(characterInList);
            //}
        }
        foreach (GameObject x in GetComponent<PlayerTeams>().otherCharacters)
        { //cia tas pats kas virsuj :/
            DisableCharacter(x);
        }
    }

    private static void DisableCharacter(GameObject character)
    {
        // character.GetComponent<PlayerInformation>().ToggleSelectionBorder(false);
        // character.GetComponent<GridMovement>().DisableGrid();
        // instance.cornerButtonManager.transform.GetChild(0).gameObject.SetActive(false);
        // for (int k = 0; k < character.GetComponent<ActionManager>().ActionScripts.Count; k++)
        // {
        //     character.GetComponent<ActionManager>().ActionScripts[k].action.DisableGrid();
        // }
    }

    public void DisableGrids()
    {
        for (int i = 0; i < GetComponent<PlayerTeams>().allCharacterList.teams.Count; i++)
        {
            for (int j = 0; j < GetComponent<PlayerTeams>().allCharacterList.teams[i].characters.Count; j++)
            {
                // GameObject characterInList = GetComponent<PlayerTeams>().allCharacterList.teams[i].characters[j];
                // characterInList.GetComponent<GridMovement>().DisableGrid();
                // characterInList.GetComponent<GridMovement>().DisableWayTiles();
                // for (int k = 0; k < characterInList.GetComponent<ActionManager>().ActionScripts.Count; k++)
                // {
                //     characterInList.GetComponent<ActionManager>().ActionScripts[k].action.DisableGrid();
                // }
            }
        }
        foreach (GameObject x in GetComponent<PlayerTeams>().otherCharacters)
        {
            // x.GetComponent<GridMovement>().DisableGrid();
            // for (int k = 0; k < x.GetComponent<ActionManager>().ActionScripts.Count; k++)
            // {
            //     x.GetComponent<ActionManager>().ActionScripts[k].action.DisableGrid();
            // }
        }
    }
    public void MoveSelectedCharacter(GameObject newPosition)
    {
        // GameObject FinalDestination = SelectedCharacter.GetComponent<GridMovement>().PickUpWayTiles();
        // if (SelectedCharacter.GetComponent<GridMovement>().AreThereConsumablesOnTheWay())
        // {
            // undoAction.available = false;
        // }
        // if (FinalDestination != null)
        // {
            // newPosition = FinalDestination;
        // }
        SelectedCharacter.transform.position = newPosition.transform.position + new Vector3(0f, 0f, -1f);
        // SelectedCharacter.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
        bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
    }
    public void MoveCharacter(GameObject newPosition, GameObject character)
    {
        // GameObject FinalDestination = character.GetComponent<GridMovement>().PickUpWayTiles();
        // if (character.GetComponent<GridMovement>().AreThereConsumablesOnTheWay())
        // {
            // undoAction.available = false;
        // }
        // if (FinalDestination != null)
        // {
            // newPosition = FinalDestination;
        // }
        character.transform.position = newPosition.transform.position + new Vector3(0f, 0f, -1f);
        // character.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
        bottomCornerUI.EnableAbilities(character.GetComponent<PlayerInformation>().savedCharacter);
    }
    public void FocusSelectedCharacter(GameObject character)
    {
        Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1;
        Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1;
        Camera.GetComponent<CinemachineVirtualCamera>().Follow = character.transform.Find("CameraPosition").transform;
    }
    public void DisableVisionTiles()
    {
        OnAnyMove(); //VienkartinisUpdate
        var playerTeamsList = GetComponent<PlayerTeams>().allCharacterList;
        var playerTeams = GetComponent<PlayerTeams>();
        for (int i = 0; i < playerTeamsList.teams.Count; i++) //isjungia visu
        {
            for (int j = 0; j < playerTeamsList.teams[i].characters.Count; j++)
            {
                playerTeamsList.teams[i].characters[j].GetComponent<CharacterVision>().DisableGrid();
                if (playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject != null)
                {
                    playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject.GetComponent<CharacterVision>().DisableGrid();
                }
            }
        }
    }
    public void ChangeVisionTiles()
    {
        OnAnyMove(); //VienkartinisUpdate
        var playerTeamsList = GetComponent<PlayerTeams>().allCharacterList;
        var playerTeams = GetComponent<PlayerTeams>();
        for (int i = 0; i < playerTeamsList.teams.Count; i++) //isjungia visu
        {
            for (int j = 0; j < playerTeamsList.teams[i].characters.Count; j++)
            {
                playerTeamsList.teams[i].characters[j].GetComponent<CharacterVision>().DisableGrid();
                if (playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject != null)
                {
                    playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject.GetComponent<CharacterVision>().DisableGrid();
                }
            }
        }
        //
        bool isTeamAI = GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].isTeamAI;
        for (int i = 0; i < playerTeamsList.teams.Count; i++) //ijungia aktyvios komandos allegiance komandu
        {
            if (isTeamAI) //skiriasi if'ai tik su kuo lygina team allegiance
            {
                if (playerTeams.FindTeamAllegiance(playerTeamsList.teams[i].teamName) == PlayerAllegiance)
                {
                    for (int j = 0; j < playerTeamsList.teams[i].characters.Count; j++)//ir ijungia dabartines komandos
                    {
                        playerTeamsList.teams[i].characters[j].GetComponent<CharacterVision>().EnableGrid();
                        if (playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject != null)
                        {
                            playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject.GetComponent<CharacterVision>().EnableGrid();
                        }
                    }
                }
            }
            else if (playerTeams.FindTeamAllegiance(playerTeamsList.teams[i].teamName) == playerTeams.FindTeamAllegiance(playerTeamsList.teams[activeTeamIndex].teamName))
            {
                for (int j = 0; j < playerTeamsList.teams[i].characters.Count; j++)//ir ijungia dabartines komandos
                {
                    playerTeamsList.teams[i].characters[j].GetComponent<CharacterVision>().EnableGrid();
                    if (playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject != null)
                    {
                        playerTeamsList.teams[i].characters[j].GetComponent<PlayerInformation>().VisionGameObject.GetComponent<CharacterVision>().EnableGrid();
                    }
                }
            }
        }
        /* 
        for (int j = 0; j < playerTeamsList.teams[activeTeamIndex].characters.Count; j++)//ir ijungia dabartines komandos
    {
        playerTeamsList.teams[activeTeamIndex].characters[j].GetComponent<CharacterVision>().EnableGrid();
        if (playerTeamsList.teams[activeTeamIndex].characters[j].GetComponent<PlayerInformation>().VisionGameObject != null)
        {
            playerTeamsList.teams[activeTeamIndex].characters[j].GetComponent<PlayerInformation>().VisionGameObject.GetComponent<CharacterVision>().EnableGrid();
        }
    }
    */
        activateOnce = true;
    }
    public void OnAnyMove()
    {
        var playerTeamsList = GetComponent<PlayerTeams>().allCharacterList;
        for (int i = 0; i < playerTeamsList.teams.Count; i++)
        {
            for (int j = 0; j < playerTeamsList.teams[i].characters.Count; j++)
            {
                // playerTeamsList.teams[i].characters[j].GetComponent<PlayerMovement>().OnAnyMove();
            }
        }
        foreach (GameObject x in GetComponent<PlayerTeams>().otherCharacters)
        { 
            // x.GetComponent<PlayerMovement>().OnAnyMove();       
        }
    }
    public void ChangeActiveTeam(string newActiveTeamsName)
    {
        GameObject newActiveTeamsPortraitBox = null;
        for (int i = 0; i < GetComponent<PlayerTeams>().allCharacterList.teams.Count; i++)
        {
            if (GetComponent<PlayerTeams>().allCharacterList.teams[i].teamName == newActiveTeamsName)
            {
                newActiveTeamsPortraitBox = GetComponent<PlayerTeams>().allCharacterList.teams[i].teamPortraitBoxGameObject;
            }
            else
            {
                GetComponent<PlayerTeams>().allCharacterList.teams[i].teamPortraitBoxGameObject.SetActive(false);
            }
        }
        if (newActiveTeamsPortraitBox != null)
        {
            newActiveTeamsPortraitBox.SetActive(true);
        }
        ActiveTeam = newActiveTeamsName;
    }
    public void EndTurn()
    {
        OnAnyMove();
        DisableAllCharacters();
        InspectedCharacter = null;
        for (int j = 0; j < GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].characters.Count; j++) //dabartine komanda
        {
            // GameObject characterInList = GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].characters[j];
            // characterInList.GetComponent<GridMovement>().OnTurnEnd();
            // characterInList.GetComponent<PlayerInformation>().OnTurnEnd();
            // for (int k = 0; k < characterInList.GetComponent<ActionManager>().ActionScripts.Count; k++)
            // {
            //     characterInList.GetComponent<ActionManager>().ActionScripts[k].action.OnTurnEnd();
            // }
        }
        //----------Environmental hazards iskvietimas
        foreach (EnvironmentalHazard hazard in environmentalHazards)
        {
            hazard.ActivateAction(true);
        }
        //--------------------------------
        if (activeTeamIndex + 1 == GetComponent<PlayerTeams>().allCharacterList.teams.Count)
        {
            activeTeamIndex = 0;
        }
        else
        {
            activeTeamIndex++;
        }
        ChangeActiveTeam(GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].teamName);

        //va sita padelayinti jei yra komandos kortele
        for (int j = 0; j < GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].characters.Count; j++)//komanda kurios dabar eile bus
        {
            GameObject characterInList = GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].characters[j];
            // characterInList.GetComponent<GridMovement>().OnTurnStart();
            //  for (int k = 0; k < characterInList.GetComponent<ActionManager>().ActionScripts.Count; k++)
            // {
            //     characterInList.GetComponent<ActionManager>().ActionScripts[k].action.OnTurnStart();
            // }
            characterInList.GetComponent<PlayerInformation>().OnTurnStart();
        }

        //----------Environmental hazards iskvietimas
        foreach (EnvironmentalHazard hazard in environmentalHazards)
        {
            hazard.ActivateAction(false);
        }
        //--------------------------------

        bool isTeamAI = GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].isTeamAI;
        if (!isTeamAI) //Camera focus at the start of the turn
        {
            if (GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].lastSelectedPlayer.GetComponent<PlayerInformation>().health > 0)
            {
                FocusSelectedCharacter(GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].lastSelectedPlayer);
            }
            else if (GetComponent<PlayerTeams>().FirstAliveCharacter(activeTeamIndex) != null)
            {
                FocusSelectedCharacter(GetComponent<PlayerTeams>().FirstAliveCharacter(activeTeamIndex));
            }
        }
        if (SelectedCharacter != null)
        {
            DeselectTeam(SelectedCharacter);
        }
        ChangeVisionTiles();
        //AI
        if (isTeamAI)
        {
            isAITurn = true;
            GameObject EnemyTurnText = GameObject.Find("Canvas").transform.Find("EnemyTurnText").gameObject;
            EnemyTurnText.SetActive(true);
            EnemyTurnText.GetComponent<Text>().color = ColorStorage.TeamColor(ActiveTeam);
            GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("EndTurn").GetComponent<Button>().interactable = false;
            canButtonsBeClicked = false;
            isBoardDisabled = true;

            if (!respawnEnemiesDuringThisTurn)
            {
                List<GameObject> AliveAIList = GetComponent<PlayerTeams>().AliveCharacterList(activeTeamIndex);//kad nereiktu laukt kol kiekvienas negyvas paeis
                int PlayersInTeamCount = AliveAIList.Count;
                float TimeSpent = 0;
                for (int j = 0; j < PlayersInTeamCount; j++)//komanda kurios dabar eile bus
                {
                    GameObject characterInList = AliveAIList[j];
                    if (characterInList.GetComponent<PlayerInformation>().health > 0)
                    {
                        StartCoroutine(ExecuteAfterTime(0.5f + TimeSpent, () =>
                        {
                            //FocusSelectedCharacter(characterInList);
                            //characterInList.GetComponent<AIBehaviour>().DoTurnActions();
                        }));
                        //TimeSpent += characterInList.GetComponent<AIBehaviour>().TimeRequiredForTurn();
                    }
                }
                //Debug.Log(TimeSpent);
                StartCoroutine(ExecuteAfterTime(1f + TimeSpent, () =>
                {
                    EndAITurn(false);
                }));
            }
            else//respawn
            {
                GetComponent<AIManager>().SpawnEnemiesAtSpawnPoints(3, 1);
                respawnEnemiesDuringThisTurn = false;
            }
        }
        else if(GameObject.Find("GameProgress") != null && _data.townData.singlePlayer)
        {
            GameObject.Find("Canvas").transform.Find("YourTurn").gameObject.SetActive(true);
        }
        //
        activateOnce = true;
        undoAction.available = false;
    }

    public void EndAITurn(bool hasEnemySpawningJustOccured)
    {
        isAITurn = false;
        OnAnyMove();
        if (!isVictoryScreenEnabled || hasEnemySpawningJustOccured)
        {
            EndTurn();
            GameObject.Find("Canvas").transform.Find("EnemyTurnText").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("EndTurn").GetComponent<Button>().interactable = true;
            canButtonsBeClicked = true;
            isBoardDisabled = false;
        }
    }

    public void GameOver()
    {
        PlayerTeams teamsInformation = GetComponent<PlayerTeams>();
        if (teamsInformation.ChampionTeam != null)
        {
            string team = GetComponent<PlayerTeams>().ChampionTeam;
            if (teamsInformation.allCharacterList.teams[0].teamName == team) {
                GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = "VICTORY";
            }
            else 
            {
                GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = "DEFEAT";
            }
            //GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = GetComponent<PlayerTeams>().ChampionTeam.ToUpper() + " TEAM WINS";
            Color victoryColor = ColorStorage.TeamColor(team);
            GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().color = victoryColor;
            GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("TeamColor").GetComponent<Image>().color = victoryColor;
            GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("Continue").GetComponent<TextMeshProUGUI>().color = victoryColor;
        }
        else if (teamsInformation.ChampionAllegiance != null)
        {
            Color victoryColor = Color.white;
            if (teamsInformation.ChampionAllegiance == "1") 
            {
                GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = "VICTORY";
                victoryColor = ColorStorage.TeamColor(teamsInformation.allCharacterList.teams[0].teamName);
            }
            else
            {
                GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = "DEFEAT";
                victoryColor = ColorStorage.TeamColor(teamsInformation.allCharacterList.teams[1].teamName);
            }
            
            GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().color = victoryColor;
            GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("TeamColor").GetComponent<Image>().color = victoryColor;
            GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("Continue").GetComponent<TextMeshProUGUI>().color = victoryColor;
            //GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = GetComponent<PlayerTeams>().ChampionAllegiance + " ALLEGIANCE WINS!";
        }
        else GameObject.Find("Canvas").transform.Find("VictoryScreen").transform.Find("VictoryText").GetComponent<TextMeshProUGUI>().text = "DRAW";
        isBoardDisabled = true;
        GameObject.Find("Canvas").transform.Find("PortraitBoxesContainer").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("EndTurn").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("TopRightCornerUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("CornerUIManagerContainer").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("VictoryScreen").gameObject.SetActive(true);
        isVictoryScreenEnabled = true;
        activateOnce = true;
        Debug.Log("Need to fix this");
        //XP
        if (SaveSystem.DoesSaveFileExist() && SaveSystem.LoadTownData().singlePlayer)
        {

            //VICTORY
            if (teamsInformation.ChampionTeam == teamsInformation.allCharacterList.teams[0].teamName || teamsInformation.ChampionAllegiance == "1")
            {
                for (int i = 0; i < _data.CharactersOnLastMission.Count; i++)
                {
                    teamsInformation.allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>().AddKillXP();
                    _data.Characters[_data.CharactersOnLastMission[i]].xPToGain = 750 + teamsInformation.allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>().XPToGain;
                    _data.Characters[_data.CharactersOnLastMission[i]].dead = teamsInformation.allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>().health <= 0;
                    if (_data.Characters[_data.CharactersOnLastMission[i]].dead)
                    {
                        _data.Characters[_data.CharactersOnLastMission[i]].xPToGain = 0;
                       // _data.statistics.characterDeathsCountByClass[Statistics.GetClassIndex(_data.Characters[_data.CharactersOnLastMission[i]].prefab.GetComponent<PlayerInformation>().ClassName)]++;
                      //  _data.globalStatistics.characterDeathsCountByClass[Statistics.GetClassIndex(_data.Characters[_data.CharactersOnLastMission[i]].prefab.GetComponent<PlayerInformation>().ClassName)]++;
                    }
                }
                _data.townData.wasLastMissionSuccessful = true;
                _data.townData.pastEncounters.Add(_data.townData.selectedEncounter);
                _data.townData.generateNewEncounters = true;
                if(_data.townData.selectedEncounter.mapName == "Merchant Ambush") 
                {
                    TownHallData townHall = _data.townData.townHall;
                    townHall.SetByType((TownHallUpgrade)5, Events.Contains("MerchantDied") ? 1 : 2);
                }
            }
            //DEFEAT
            else
            {
                for (int i = 0; i < _data.CharactersOnLastMission.Count; i++)
                {
                    _data.Characters[_data.CharactersOnLastMission[i]].dead = teamsInformation.allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>().health <= 0;
                    _data.Characters[_data.CharactersOnLastMission[i]].xPToGain = 0;
                    //_data.statistics.characterDeathsCountByClass[Statistics.GetClassIndex(_data.Characters[_data.CharactersOnLastMission[i]].prefab.GetComponent<PlayerInformation>().ClassName)]++;
                    //_data.globalStatistics.characterDeathsCountByClass[Statistics.GetClassIndex(_data.Characters[_data.CharactersOnLastMission[i]].prefab.GetComponent<PlayerInformation>().ClassName)]++;
                }
                _data.townData.wasLastMissionSuccessful = false;
                if (_data.townData.selectedEncounter.mapName == "Merchant Ambush")
                {
                    TownHallData townHall = _data.townData.townHall;
                    townHall.SetByType((TownHallUpgrade)5, 1);
                }
            }
            // _saveData.SaveTownData();
        }

    }
    
    public void PauseGame()
    {
        GameObject.Find("Canvas").transform.Find("PortraitBoxesContainer").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("EndTurn").gameObject.SetActive(false);
        // GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row2").transform.Find("HelpButton").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("CornerUIManagerContainer").gameObject.SetActive(false);
        addButtonState = GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row3").transform.Find("AddButton").gameObject.activeSelf;
        GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row3").transform.Find("AddButton").gameObject.SetActive(false);
        portalButtonState = GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row1").transform.Find("PortalButton").gameObject.activeSelf;
        GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row1").transform.Find("PortalButton").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("EndTurnScreen").gameObject.SetActive(false);
        undoTurnButtonState = GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.activeSelf;
        GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row1").transform.Find("PauseButton").gameObject.SetActive(false);
        isBoardDisabledState = isBoardDisabled;
        isBoardDisabled = true;
        GameObject.Find("Canvas").transform.Find("PauseMenu").transform.Find("PauseMenuInGame").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("PauseMenu").transform.Find("ConfirmMenu").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.SetActive(true);
        Debug.Log("Reikia perdaryti");
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        GameObject.Find("Canvas").transform.Find("PortraitBoxesContainer").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("EndTurn").gameObject.SetActive(true);
        // GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row2").transform.Find("HelpButton").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("CornerUIManagerContainer").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row3").transform.Find("AddButton").gameObject.SetActive(addButtonState);
        GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row1").transform.Find("PortalButton").gameObject.SetActive(portalButtonState);
        GameObject.Find("Canvas").transform.Find("UndoTurn").gameObject.SetActive(undoTurnButtonState);
        GameObject.Find("Canvas").transform.Find("TopRightCornerUI").transform.Find("Row1").transform.Find("PauseButton").gameObject.SetActive(true);
        StartCoroutine(ExecuteAfterTime(0.1f, () =>
        {
            if (!GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.activeSelf)
                isBoardDisabled = isBoardDisabledState;
        }));
        GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.SetActive(false);
        Debug.Log("Reikia perdaryti");
        Time.timeScale = 1;
    }
    private void CheckWhetherToHighlightEndTurn()
    {
        bool highlightEndTurn = true;
        foreach (GameObject character in GetComponent<PlayerTeams>().allCharacterList.teams[activeTeamIndex].characters)
        {
            // if (character.GetComponent<GridMovement>().AvailableMovementPoints > 2 && character.GetComponent<PlayerInformation>().health > 0 && !character.GetComponent<PlayerInformation>().Debuffs.Contains("Stun"))
                highlightEndTurn = false;
        }
        GameObject.Find("Canvas").transform.Find("EndTurn").GetComponent<EndTurn>().ChangeSprite(highlightEndTurn);
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

    public void enableBoardWithDelay(float delay)
    {
        StartCoroutine(ExecuteAfterTime(delay, () =>
        {
            isBoardDisabled = false;
        }));
    }
}
