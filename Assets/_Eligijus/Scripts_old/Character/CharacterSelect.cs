using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    private GameProgress gameProgress;
    private List<(SavedCharacter, int)> charactersToGoOnMission;
    private List<(SavedCharacter, int)> selectedEnemies;
    private List<SavedCharacter> defaultEnemies;
    private bool enemySelection;
    public bool allowDuplicates;
    public SaveData saveData;
    public Data _data;
    // Start is called before the first frame update
    void Start()
    {
        gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        charactersToGoOnMission = new List<(SavedCharacter, int)>();
        selectedEnemies = new List<(SavedCharacter, int)>();
        //defaultEnemies = gameProgress.AllEnemySavedCharacters;
        defaultEnemies = new List<SavedCharacter>();
        SaveSystem.LoadTownData().selectedEncounter.enemyPool.ForEach(enemyName => defaultEnemies.Add(_data.AllEnemySavedCharacters.Find(x => x.prefab.name == enemyName)));
        SelectDefaultEnemies();
        enemySelection = false;
        //allowDuplicates = false;
        allowDuplicates = SaveSystem.LoadTownData().selectedEncounter.allowDuplicates;
        saveData.LoadTownData();
        gameProgress.PrepareNewTownDay();
        GameObject.Find("CanvasCamera").transform.Find("AutoFill").GetComponent<Button>().interactable = _data.Characters.Count >= 3;
        UpdateView();
    }

    public void UpdateView()
    {
        List<SavedCharacter> characterList = enemySelection ? _data.AllEnemySavedCharacters : _data.Characters;
        List<(SavedCharacter, int)> selectedCharList = enemySelection ? selectedEnemies : charactersToGoOnMission;
        Transform CharacterButtons = GameObject.Find("CanvasCamera").transform.Find("CharacterButtons");
        foreach (Transform child in CharacterButtons)
        {
            child.gameObject.SetActive(false);
        }
        for (int i = 0; i < characterList.Count; i++)
        {
            CharacterButtons.GetChild(i).GetComponent<CharacterSelection>().characterIndex = i;
            CharacterButtons.GetChild(i).Find("Character").Find("Portrait").gameObject.SetActive(true);
            CharacterButtons.GetChild(i).Find("Character").Find("Portrait").GetComponent<Image>().sprite =
                characterList[i].prefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            CharacterButtons.GetChild(i).Find("Character").Find("LevelText").GetComponent<TextMeshProUGUI>().text = characterList[i].level.ToString();
            CharacterButtons.GetChild(i).gameObject.SetActive(true);
            CharacterButtons.GetChild(i).transform.Find("Hover").GetComponent<Animator>().SetBool("select", AlreadySelected(i));
        }
        if (!enemySelection && selectedCharList.Count == 3)
        {
            DisableCharacters();
        }
        else EnableCharacters();
        var canvasCamera = GameObject.Find("CanvasCamera").transform;
        canvasCamera.Find("AutoFill").gameObject.SetActive(!enemySelection);
        canvasCamera.Find("Clear").gameObject.SetActive(!enemySelection);
        // canvasCamera.Find("Next").gameObject.SetActive(!enemySelection);
        canvasCamera.Find("Back").gameObject.SetActive(!enemySelection);
        canvasCamera.Find("AllNone").gameObject.SetActive(enemySelection);
        canvasCamera.Find("AllowDuplicates").gameObject.SetActive(enemySelection);
        canvasCamera.Find("AllowDuplicates").transform.Find("Hover").GetComponent<Animator>().SetBool("select", allowDuplicates);
        Debug.Log("Reikia sutvarkyti");
    }

    public void AutoFill()
    {
        ClearTeam();
        for(int i = 0; i < 3; i++)
        {
            AddCharacterToTeam(i);
        }
    }

    public void ClearTeam()
    {
        for(int i = charactersToGoOnMission.Count - 1; i >= 0; i--)
        {
            RemoveCharacterFromTeam(charactersToGoOnMission[i].Item2);
        }
        EnableCharacters();
    }

    public void SelectDefaultEnemies()
    {
        selectedEnemies.Clear();
        foreach (SavedCharacter defaultEnemy in defaultEnemies)
        {
            selectedEnemies.Add((_data.AllEnemySavedCharacters[getEnemyIndex(defaultEnemy)], getEnemyIndex(defaultEnemy)));
        }
    }

    private int getEnemyIndex(SavedCharacter enemy)
    {
        List<SavedCharacter> allEnemies = _data.AllEnemySavedCharacters;
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if(allEnemies[i].prefab == enemy.prefab)
            {
                return i;
            }
        }
        throw new Exception("Enemy index exception");
    }

    public void OnEnemyButtonClick()
    {
        int numOfEnemies = 3;
        if(_data.townData.selectedEncounter.numOfEnemies > 0)
        {
            numOfEnemies = _data.townData.selectedEncounter.numOfEnemies;
        }
        //if(gameProgress.GetComponent<MapSetup>().MapPrefabs.Find(x => x.name == gameProgress.townData.selectedMission) != null)
        //{
        //    numOfEnemies = gameProgress.GetComponent<MapSetup>().MapPrefabs.Find(x => x.name == gameProgress.townData.selectedMission).GetComponent<Map>().numberOfEnemies;
        //}
        if (enemySelection && ((!allowDuplicates && selectedEnemies.Count < numOfEnemies) || (allowDuplicates && selectedEnemies.Count == 0)))
        {
            Debug.Log("Per mazai pasirinktu priesu");
            return;
        }
        enemySelection = !enemySelection;
        GameObject.Find("CanvasCamera").transform.Find("Enemies").transform.Find("Hover").GetComponent<Animator>().SetBool("select", enemySelection);
        GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").gameObject.SetActive(!enemySelection);
        UpdateView();
    }

    public void SaveData()
    {
        foreach ((SavedCharacter, int) character in charactersToGoOnMission)
        {
            _data.Characters.Remove(character.Item1);
            _data.Characters.Insert(0, character.Item1);
        }
        foreach((SavedCharacter, int) enemy in selectedEnemies)
        {
            _data.selectedEnemies.Add(enemy.Item2);
        }
    }

    public void OnCharacterButtonClick(int characterIndex)
    {
        if(!enemySelection)
        {
            if (!AlreadySelected(characterIndex) && charactersToGoOnMission.Count < 3)
            {
                AddCharacterToTeam(characterIndex);
            }
            else if (AlreadySelected(characterIndex))
            {
                RemoveCharacterFromTeam(characterIndex);
            }
        }
        else
        {
            if (!AlreadySelected(characterIndex))
            {
                SelectEnemy(characterIndex);
            }
            else DeselectEnemy(characterIndex);
        }
    }

    public void RemoveCharacterFromTeam(int characterIndex)
    {
        var teamPortraitManager = GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>();
        charactersToGoOnMission.RemoveAt(charactersToGoOnMission.FindIndex(character => character.Item2 == characterIndex));
        teamPortraitManager.RemoveCharacter(characterIndex);
        GameObject.Find("CanvasCamera").transform.Find("CharacterButtons").GetChild(characterIndex).transform.Find("Hover").GetComponent<Animator>().SetBool("select", false);
        EnableCharacters();
    }

    public void AddCharacterToTeam(int characterIndex)
    {
        var teamPortraitManager = GameObject.Find("CanvasCamera").transform.Find("TeamPortraitBox").transform.Find("PortraitBoxesContainer").GetComponent<CSTeamPortraitManager>();
        charactersToGoOnMission.Add((_data.Characters[characterIndex], characterIndex));
        teamPortraitManager.AddCharacterInCS3(_data.Characters[characterIndex].prefab, characterIndex);
        GameObject.Find("CanvasCamera").transform.Find("CharacterButtons").GetChild(characterIndex).transform.Find("Hover").GetComponent<Animator>().SetBool("select", true);
        if(charactersToGoOnMission.Count == 3)
        {
            GameObject.Find("CanvasCamera").transform.Find("Embark").GetComponent<Button>().interactable = true;
            DisableCharacters();
        }
    }

    public void SelectEnemy(int enemyIndex)
    {
        selectedEnemies.Add((_data.AllEnemySavedCharacters[enemyIndex], enemyIndex));
        GameObject.Find("CanvasCamera").transform.Find("CharacterButtons").GetChild(enemyIndex).transform.Find("Hover").GetComponent<Animator>().SetBool("select", true);
    }

    public void DeselectEnemy(int enemyIndex)
    {
        selectedEnemies.RemoveAll(enemy => enemy.Item2 == enemyIndex);
        GameObject.Find("CanvasCamera").transform.Find("CharacterButtons").GetChild(enemyIndex).transform.Find("Hover").GetComponent<Animator>().SetBool("select", false);
    }

    //private void ClearSelectedEnemies()
    //{
    //    for(int i = selectedEnemies.Count - 1; i >= 0; i--)
    //    {
    //        DeselectEnemy(selectedEnemies[i].Item2);
    //    }
    //}

    private void EnableCharacters()
    {
        foreach (Transform characterButton in GameObject.Find("CanvasCamera").transform.Find("CharacterButtons"))
        {
            if (characterButton.gameObject.activeSelf)
            {
                characterButton.transform.Find("Character").Find("Portrait").GetComponent<Image>().color = Color.white;
                characterButton.GetComponent<CharacterPortrait>().available = true;
            }
        }
    }

    private void DisableCharacters()
    {
        foreach (Transform characterButton in GameObject.Find("CanvasCamera").transform.Find("CharacterButtons"))
        {
            if (characterButton.gameObject.activeSelf && !AlreadySelected(characterButton.GetComponent<CharacterPortrait>().characterIndex))
            {
                characterButton.transform.Find("Character").Find("Portrait").GetComponent<Image>().color = Color.grey;
                characterButton.GetComponent<CharacterPortrait>().available = false;
            }
        }
    }

    private bool AlreadySelected(int characterIndex)
    {
        List<(SavedCharacter, int)> characterList = enemySelection ? selectedEnemies : charactersToGoOnMission;
        foreach((SavedCharacter, int) character in characterList)
        {
            if(character.Item2 == characterIndex)
            {
                return true;
            }
        }
        return false;
    }

    public void AllNone()
    {
        bool all = !AllSelected();
        for (int i = 0; i < _data.AllEnemySavedCharacters.Count; i++)
        {
            if (all)
            {
                SelectEnemy(i);
            }
            else DeselectEnemy(i);
        }
    }

    private bool AllSelected()
    {
        for(int i = 0; i < _data.AllEnemySavedCharacters.Count; i++)
        {
            if(!AlreadySelected(i))
            {
                return false;
            }
        }
        return true;
    }

    public void ToggleAllowDuplication()
    {
        allowDuplicates = !allowDuplicates;
        GameObject.Find("CanvasCamera").transform.Find("AllowDuplicates").transform.Find("Hover").GetComponent<Animator>().SetBool("select", allowDuplicates);
    }
    //Epic poses
    public void DisplaySelectedCharacters()
    {
        // Transform posingCharacters = GameObject.Find("PoseCharacters").transform;
        // for (int i = 0; i < posingCharacters.childCount; i++)
        // {
        //     posingCharacters.GetChild(i).gameObject.SetActive(true);
        //
        //     posingCharacters.GetChild(i).GetComponent<Animator>().runtimeAnimatorController =
        //         charactersToGoOnMission[i].Item1.prefab.transform.Find("CharacterModel").GetComponent<Animator>().runtimeAnimatorController;
        //     posingCharacters.GetChild(i).GetComponent<Animator>().SetTrigger("playerHit");
        // }
        //Canvas
        // Transform canvas = GameObject.Find("CanvasCamera").transform;
        // // canvas.Find("Enemies").gameObject.SetActive(false);
        // canvas.Find("Clear").gameObject.SetActive(false);
        // canvas.Find("AutoFill").gameObject.SetActive(false);
        // canvas.Find("CharacterButtons").gameObject.SetActive(false);
        // canvas.Find("TeamPortraitBox").gameObject.SetActive(false);
        // //
        // canvas.Find("Next").gameObject.SetActive(false);
        // canvas.Find("Back").gameObject.SetActive(false);
        // canvas.Find("Embark").gameObject.SetActive(true);
        // canvas.Find("TemporaryBack").gameObject.SetActive(true);
    }
    public void StopDisplayingSelectedCharacters()
    {
        // Transform posingCharacters = GameObject.Find("PoseCharacters").transform;
        // for (int i = 0; i < posingCharacters.childCount; i++)
        // {
        //     posingCharacters.GetChild(i).gameObject.SetActive(false);
        // }
        // //Canvas
        // Transform canvas = GameObject.Find("CanvasCamera").transform;
        // // canvas.Find("Enemies").gameObject.SetActive(true);
        // canvas.Find("Clear").gameObject.SetActive(true);
        // canvas.Find("AutoFill").gameObject.SetActive(true);
        // canvas.Find("CharacterButtons").gameObject.SetActive(true);
        // canvas.Find("TeamPortraitBox").gameObject.SetActive(true);
        // // //
        // canvas.Find("Next").gameObject.SetActive(true);
        // canvas.Find("Back").gameObject.SetActive(true);
        // canvas.Find("Embark").gameObject.SetActive(false);
        // canvas.Find("TemporaryBack").gameObject.SetActive(false);
    }
}
