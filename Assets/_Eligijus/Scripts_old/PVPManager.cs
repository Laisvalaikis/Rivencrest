using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVPManager : MonoBehaviour
{
    public List<SavedCharacter> allAvailableCharacters;
    public List<GameObject> allAvailableMaps;
    private List<List<PVPSavedCharacter>> selectedTeams;
    private int selectedMap;
    private int currentTeam;
    public Sprite emptySprite;
    [HideInInspector] public static PVPManager instance;
    public GameObject cornerUIManager;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        selectedTeams = new List<List<PVPSavedCharacter>>
        {
            new List<PVPSavedCharacter>(),
            new List<PVPSavedCharacter>()
        };
        currentTeam = 0;
        selectedMap = -1;
    }

    public void OnAwake()
    {
        if(SceneManager.GetActiveScene().name == "PVPEncounter")
        {
            EncounterSetup();
        }
    }

    private void EncounterSetup()
    {
        GameObject gameInformation;
        PlayerTeams teamSettings;
        SetCharacters(out gameInformation, out teamSettings);

        GameObject map = MapAndCameraSetup();

        SetCoordinates(teamSettings, map);

        SetAIDestinations(gameInformation, map);
    }

    private static void SetAIDestinations(GameObject gameInformation, GameObject map)
    {
        //AI destinations
        var destinationsGameObject = map.transform.Find("AIDestinations");
        var AIsettings = gameInformation.GetComponent<AIManager>();
        List<GameObject> destinations = new List<GameObject>();
        for (int i = 0; i < destinationsGameObject.childCount; i++)
        {
            destinations.Add(destinationsGameObject.GetChild(i).gameObject);
        }
        AIsettings.AIDestinations = destinations;
    }

    public void OnLongCharacterButtonPress(int index)
    {
        var table = GameObject.Find("CanvasCamera").transform.Find("CharacterTable");
        var character = allAvailableCharacters[index];
        FillTable(table, character);
        table.gameObject.SetActive(true);
    }

    private static void SetCoordinates(PlayerTeams teamSettings, GameObject map)
    {
        //coordinates
        var coordinatesGameObject = map.transform.Find("Coordinates");
        teamSettings.allCharacterList.teams[1].coordinates.Clear();
        for (int i = 0; i < 3; i++)
        {
            teamSettings.allCharacterList.teams[0].coordinates[i] = coordinatesGameObject.GetChild(0).GetChild(i);
        }
        for (int i = 0; i < 3; i++)
        {
            teamSettings.allCharacterList.teams[1].coordinates.Add(coordinatesGameObject.GetChild(1).GetChild(i));
        }
    }

    private GameObject MapAndCameraSetup()
    {
        //camera
        GameObject map = Instantiate(allAvailableMaps[selectedMap], GameObject.Find("Map").transform);
        GameObject.Find("CM vcam1").GetComponent<CameraController>().DefaultToFollow = map.transform.Find("ToFollow").gameObject;
        GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitX.x = map.transform.Find("LowerLimit").position.x;
        GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitX.y = map.transform.Find("UpperLimit").position.x;
        GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitY.x = map.transform.Find("LowerLimit").position.y;
        GameObject.Find("CM vcam1").GetComponent<CameraController>().panLimitY.y = map.transform.Find("UpperLimit").position.y;
        GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = map.transform.Find("ToFollow");
        return map;
    }

    private void SetCharacters(out GameObject gameInformation, out PlayerTeams teamSettings)
    {
        //characters
        gameInformation = GameObject.Find("GameInformation");
        teamSettings = gameInformation.GetComponent<PlayerTeams>();
        for (int i = 0; i < 3; i++)
        {
            teamSettings.allCharacterList.teams[0].characters[i] = allAvailableCharacters[selectedTeams[0][i].index].prefab;
        }
        for (int i = 0; i < 3; i++)
        {
            teamSettings.allCharacterList.teams[1].characters[i] = allAvailableCharacters[selectedTeams[1][i].index].prefab;
        }
    }

    public void OnStart()
    {
        if(SceneManager.GetActiveScene().name == "PVPCharacterSelect")
        {
            UpdateView();
        }
        else if(SceneManager.GetActiveScene().name == "PVPMapSelect")
        {
            UpdateMatchPreview();
            UpdateView();
        }
        else if(SceneManager.GetActiveScene().name == "PVPEncounter")
        {
            //ApplyAbilitySelections();
        }
        else if(SceneManager.GetActiveScene().name == "PVPAbilitySelect")
        {
            SetupAbilitySelectTables();
            UpdateView();
        }
    }

    private void SetupAbilitySelectTables()
    {
        var abilityTables = GameObject.Find("CanvasCamera").transform.Find("AbilitySelectTables");
        for(int i = 0; i < 3; i++)
        {
            var table = abilityTables.GetChild(i);
            var character = allAvailableCharacters[selectedTeams[currentTeam][i].index];
            FillTable(table, character, i);
        }
        abilityTables.parent.Find("Next").GetComponent<Button>().interactable = true;
    }

    private void FillTable(Transform table, SavedCharacter savedCharacter, int index = -1)
    {
        var character = savedCharacter.prefab.GetComponent<PlayerInformation>();
        table.Find("CharacterArt").GetComponent<Image>().sprite = character.CroppedSplashArt;
        table.Find("TableBorder").GetComponent<Image>().color = character.ClassColor2;
        table.Find("Info").Find("ClassName").GetComponent<Text>().text = character.ClassName;
        table.Find("Info").Find("ClassName").GetComponent<Text>().color = character.ClassColor;
        table.Find("Info").Find("Role").GetComponent<Text>().text = character.role;
        table.Find("Info").Find("Role").GetComponent<Text>().color = character.ClassColor;
        table.Find("Info").Find("Role").Find("DAMAGE").gameObject.SetActive(character.role == "DAMAGE");
        table.Find("Info").Find("Role").Find("TANK").gameObject.SetActive(character.role == "TANK");
        table.Find("Info").Find("Role").Find("SUPPORT").gameObject.SetActive(character.role == "SUPPORT");
        table.Find("Info").Find("MaxHP").GetComponent<Text>().text = character.MaxHealth + 4 + " HP";
        table.Find("Info").Find("MaxHP").GetComponent<Text>().color = character.ClassColor;
        bool melee = character.GetComponent<PlayerAttack>().AttackRange == 1;
        table.Find("Info").Find("Range").GetComponent<Text>().text = melee ? "MELEE" : "RANGED";
        table.Find("Info").Find("Range").GetComponent<Text>().color = character.ClassColor;
        table.Find("Info").Find("Range").Find("1").gameObject.SetActive(melee);
        table.Find("Info").Find("Range").Find("2").gameObject.SetActive(!melee);
        if(SceneManager.GetActiveScene().name == "PVPAbilitySelect")
        {
            var charIndex = index;
            table.Find("Reset").GetComponent<Button>().onClick.RemoveAllListeners();
            table.Find("Reset").GetComponent<Button>().onClick.AddListener(() =>
            {
                OnResetButtonClick(charIndex);
            });
        }
        else if(SceneManager.GetActiveScene().name == "PVPCharacterSelect")
        {
            table.Find("Reset").gameObject.SetActive(false);
            table.Find("Info").Find("AbilityPointCount").gameObject.SetActive(false);
        }
        for (int j = 0; j < table.Find("Abilities").transform.childCount; j++)
        {
            table.Find("Abilities").GetChild(j).gameObject.SetActive(true);
            table.Find("Abilities").GetChild(j).Find("Color").GetComponent<Image>().sprite = character.GetComponent<ActionManager>().AbilityBackground;
            table.Find("Abilities").GetChild(j).Find("AbilityIcon").GetComponent<Image>().color = character.ClassColor;
            if (character.GetComponent<ActionManager>().FindActionByIndex(j) != null)
            {
                table.Find("Abilities").GetChild(j).Find("AbilityIcon").GetComponent<Image>().sprite = character.GetComponent<ActionManager>().FindActionByIndex(j).AbilityIcon;
            }
            else
            {
                table.Find("Abilities").GetChild(j).gameObject.SetActive(false);
            }
            var abilityName = character.GetComponent<ActionManager>().FindActionByIndex(j).actionName;
            table.Find("Abilities").GetChild(j).GetComponent<Button>().onClick.RemoveAllListeners();
            table.Find("Abilities").GetChild(j).GetComponent<Button>().onClick.AddListener(() =>
            {
                EnableHelpTable(abilityName, savedCharacter);
            });
            if (SceneManager.GetActiveScene().name == "PVPAbilitySelect")
            {
                var indices = (index, j);
                table.Find("Abilities").GetChild(j).Find("Add").GetComponent<Button>().onClick.RemoveAllListeners();
                table.Find("Abilities").GetChild(j).Find("Add").GetComponent<Button>().onClick.AddListener(() =>
                {
                    AddAbility(indices);
                });
            }
            else if(SceneManager.GetActiveScene().name == "PVPCharacterSelect")
            {
                table.Find("Abilities").GetChild(j).Find("Add").gameObject.SetActive(false);
            }
        }
    }

    private void OnResetButtonClick(int characterIndex)
    {
        for(int i = 0; i < 4; i++)
        {
            RemoveAbility((characterIndex, i));
        }
    }

    private void AddAbility((int charIndex, int abilityIndex) indices)
    {
        if (selectedTeams[currentTeam][indices.Item1].unlockedAbilities[indices.Item2] == '0')
        {
            PVPSavedCharacter temp = selectedTeams[currentTeam][indices.Item1];
            temp.unlockedAbilities = temp.unlockedAbilities.Remove(indices.Item2, 1).Insert(indices.Item2, "1");
            temp.abilityPoints--;
            selectedTeams[currentTeam][indices.Item1] = temp;
            UpdateView();
        }
    }

    private void RemoveAbility((int charIndex, int abilityIndex) indices)
    {
        if(selectedTeams[currentTeam][indices.Item1].unlockedAbilities[indices.Item2] == '1')
        {
            PVPSavedCharacter temp = selectedTeams[currentTeam][indices.Item1];
            temp.unlockedAbilities = temp.unlockedAbilities.Remove(indices.Item2, 1).Insert(indices.Item2, "0");
            temp.abilityPoints++;
            selectedTeams[currentTeam][indices.Item1] = temp;
            UpdateView();
        }
    }

    private void EnableHelpTable(string abilityName, SavedCharacter character)
    {
        GameObject.Find("HelpTableController").GetComponent<HelpTableController>().EnableTableForPVP(abilityName, character);
    }

    public void ApplyAbilitySelections()
    {
        var gameInformation = GameObject.Find("GameInformation");
        var teamSettings = gameInformation.GetComponent<PlayerTeams>();
        for (int i = 0; i < 3; i++)
        {
            teamSettings.allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>().savedCharacter = allAvailableCharacters[selectedTeams[0][i].index];
            teamSettings.allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>().savedCharacter.unlockedAbilities = selectedTeams[0][i].unlockedAbilities;
            teamSettings.allCharacterList.teams[0].characters[i].GetComponent<PlayerInformation>().savedCharacter.level = 3;
            cornerUIManager.GetComponent<ButtonManager>().GenerateAbilities();
        }
        for (int i = 0; i < 3; i++)
        {
            teamSettings.allCharacterList.teams[1].characters[i].GetComponent<PlayerInformation>().savedCharacter = allAvailableCharacters[selectedTeams[1][i].index];
            teamSettings.allCharacterList.teams[1].characters[i].GetComponent<PlayerInformation>().savedCharacter.unlockedAbilities = selectedTeams[1][i].unlockedAbilities;
            teamSettings.allCharacterList.teams[1].characters[i].GetComponent<PlayerInformation>().savedCharacter.level = 3;
            cornerUIManager.GetComponent<ButtonManager>().GenerateAbilities();
            Debug.Log("Cia kazkas darom su Ui Manager");
        }
    }

    private void UpdateView()
    {
        if (SceneManager.GetActiveScene().name == "PVPCharacterSelect")
        {
            UpdateTeamNumber();
            UpdateButtons();
            UpdateCharacterButtons();
            UpdateTeamPortraits();
        }
        else if (SceneManager.GetActiveScene().name == "PVPMapSelect")
        {
            UpdateMapButtons();
            UpdateButtons();
        }
        else if (SceneManager.GetActiveScene().name == "PVPAbilitySelect")
        {
            UpdateAbilities();
            UpdateButtons();
        }
    }

    private void UpdateAbilities()
    {
        var abilityTables = GameObject.Find("CanvasCamera").transform.Find("AbilitySelectTables");
        for (int i = 0; i < 3; i++)
        {
            var x = abilityTables.GetChild(i);
            var character = selectedTeams[currentTeam][i];
            x.Find("Info").Find("AbilityPointCount").GetComponent<Text>().text = $"{character.abilityPoints}";
            x.Find("Reset").GetComponent<Button>().interactable = character.unlockedAbilities != "0000";
            for(int j = 0; j < x.Find("Abilities").childCount; j++)
            {
                bool enable = character.unlockedAbilities[j] == '0' && character.abilityPoints > 0;
                var y = x.Find("Abilities").GetChild(j);
                y.Find("Add").gameObject.SetActive(enable);
                y.Find("Color").GetComponent<Image>().color = character.unlockedAbilities[j] == '0' ? Color.grey : Color.white;
            }
        }
        
    }

    private void UpdateMapButtons()
    {
        var mapButtons = GameObject.Find("CanvasCamera").transform.Find("MapButtons");
        for(int i = 0; i < mapButtons.childCount; i++)
        {
            mapButtons.GetChild(i).Find("Portrait").GetComponent<Image>().sprite = allAvailableMaps[i].GetComponent<Map>().mapImage;
            //mapButtons.GetChild(i).Find("Portrait").GetComponent<Image>().color = (selectedMap != -1 && selectedMap != i) ? Color.grey : Color.white;
            mapButtons.GetChild(i).Find("Hover").GetComponent<Animator>().SetBool("select", selectedMap == i);
        }
    }

    private void UpdateMatchPreview()
    {
        var portraits = GameObject.Find("CanvasCamera").transform.Find("MatchPreview").Find("Portraits");
        for(int i = 0; i < portraits.childCount; i++)
        {
            portraits.GetChild(i).Find("Portrait").GetComponent<Image>().sprite = allAvailableCharacters[selectedTeams[i / 3][i % 3].index].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
        }
    }

    private void UpdateCharacterButtons()
    {
        var characterButtons = GameObject.Find("CanvasCamera").transform.Find("CharacterButtons");
        for (int i = 0; i < characterButtons.childCount; i++)
        {
            if (i < allAvailableCharacters.Count)
            {
                characterButtons.GetChild(i).Find("Portrait").GetComponent<Image>().sprite = allAvailableCharacters[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
                characterButtons.GetChild(i).Find("Portrait").GetComponent<Image>().color = (TeamFull() && !IsSelected(i)) ? Color.grey : Color.white;
                characterButtons.GetChild(i).GetComponent<CharacterPortrait>().available = !(TeamFull() && !IsSelected(i));
                characterButtons.GetChild(i).gameObject.SetActive(true);
                characterButtons.GetChild(i).Find("Hover").GetComponent<Animator>().SetBool("select", IsSelected(i));
            }
            else
            {
                characterButtons.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void UpdateTeamPortraits()
    {
        var teamPortraits = GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").Find("PortraitBoxesContainer");
        for(int i = 0; i < teamPortraits.childCount; i++)
        {
            if(i < selectedTeams[currentTeam].Count)
            {
                teamPortraits.GetChild(i).Find("ButtonPortrait").GetComponent<Image>().sprite =
                    allAvailableCharacters[selectedTeams[currentTeam][i].index].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            }
            else
            {
                teamPortraits.GetChild(i).Find("ButtonPortrait").GetComponent<Image>().sprite = emptySprite;
            }
        }
    }

    private void UpdateButtons()
    {
        if (SceneManager.GetActiveScene().name == "PVPCharacterSelect")
        {
            var canvasCamera = GameObject.Find("CanvasCamera").transform;
            canvasCamera.Find("Clear").GetComponent<Button>().interactable = selectedTeams[currentTeam].Count > 0;
            canvasCamera.Find("Next").GetComponent<Button>().interactable = TeamFull();
            //canvasCamera.Find("Next").Find("Text").GetComponent<Text>().text = currentTeam == 0 ? "NEXT" : "START";
        }
        else if (SceneManager.GetActiveScene().name == "PVPMapSelect")
        {
            GameObject.Find("CanvasCamera").transform.Find("Next").GetComponent<Button>().interactable = selectedMap != -1;
        }
        else if (SceneManager.GetActiveScene().name == "PVPAbilitySelect")
        {
            GameObject.Find("CanvasCamera").transform.Find("Next").GetComponent<Button>().interactable = selectedTeams[currentTeam][0].abilityPoints + selectedTeams[currentTeam][1].abilityPoints + selectedTeams[currentTeam][2].abilityPoints == 0;
        }
    }

    public void OnCharacterButtonClick(int characterIndex)
    {
        if (SceneManager.GetActiveScene().name == "PVPCharacterSelect")
        {
            if (IsSelected(characterIndex))
            {
                RemoveCharacter(characterIndex);
            }
            else
            {
                AddCharacter(characterIndex);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PVPMapSelect")
        {
            selectedMap = (selectedMap == characterIndex) ? -1 : characterIndex;
            UpdateView();
        }
    }

    public void OnTeamPortraitClick(int index)
    {
        if(index < selectedTeams[currentTeam].Count)
        {
            selectedTeams[currentTeam].RemoveAt(index);
            UpdateView();
        }
    }

    public void OnClearButtonClick()
    {
        selectedTeams[currentTeam].Clear();
        UpdateView();
    }

    public void OnNextButtonClick()
    {
        if (SceneManager.GetActiveScene().name == "PVPCharacterSelect")
        {
            SceneManager.LoadScene("PVPAbilitySelect");
        }
        else if (SceneManager.GetActiveScene().name == "PVPMapSelect")
        {
            SceneManager.LoadScene("PVPEncounter");
        }
        else if(SceneManager.GetActiveScene().name == "PVPAbilitySelect")
        {
            if(currentTeam == 0)
            {
                currentTeam++;
                SceneManager.LoadScene("PVPCharacterSelect");
            }
            else
            {
                SceneManager.LoadScene("PVPMapSelect");
            }
        }
    }

    public void OnBackButtonClick()
    {
        if (SceneManager.GetActiveScene().name == "PVPCharacterSelect")
        {
            if (currentTeam == 0)
            {
                SceneManager.LoadScene("SceneSelect");
            }
            else
            {
                currentTeam--;
                SceneManager.LoadScene("PVPAbilitySelect");
            }
        }
        else if (SceneManager.GetActiveScene().name == "PVPMapSelect")
        {
            SceneManager.LoadScene("PVPAbilitySelect");
        }
        else if (SceneManager.GetActiveScene().name == "PVPAbilitySelect")
        {
            SceneManager.LoadScene("PVPCharacterSelect");
        }
    }

    private void UpdateTeamNumber()
    {
        GameObject.Find("CanvasCamera").transform.Find("Team").GetComponent<Text>().text = $"TEAM {currentTeam + 1}";
    }

    private bool IsSelected(int characterIndex)
    {
        foreach(var x in selectedTeams[currentTeam])
        {
            if(x.index == characterIndex)
            {
                return true;
            }
        }
        return false;
    }

    private bool TeamFull()
    {
        return selectedTeams[currentTeam].Count >= 3;
    }

    public void AddCharacter(int characterIndex)
    {
        if(!TeamFull())
        {
            selectedTeams[currentTeam].Add(new PVPSavedCharacter{
                index = characterIndex,
                unlockedAbilities = allAvailableCharacters[characterIndex].unlockedAbilities,
                abilityPoints = 0
            });
            UpdateView();
        }
    }

    public void RemoveCharacter(int characterIndex)
    {
        selectedTeams[currentTeam].RemoveAll(x => x.index == characterIndex);
        UpdateView();
    }

    public void OnTeamPortraitHover(int index)
    {
        GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").Find("PortraitBoxesContainer")
            .GetChild(index).Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", true);
    }

    public void OffTeamPortraitHover(int index)
    {
        GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").Find("PortraitBoxesContainer")
            .GetChild(index).Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", false);
    }

    public struct PVPSavedCharacter
    {
        public int index;
        public string unlockedAbilities;
        public int abilityPoints;
    }
}

