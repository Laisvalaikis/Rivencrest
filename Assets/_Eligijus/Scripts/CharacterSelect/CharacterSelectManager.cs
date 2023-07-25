using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public List<PortraitButtonData> portraitButtons;
    public List<CharacterSelect> characterButtons;
    [SerializeField] private Button embark;
    [SerializeField] private Button back;
    [SerializeField] private View characterSelectView;
    public bool allowDuplicates;
    public Sprite emptySprite;
    private Data _data;
    private List<(SavedCharacter, int)> charactersToGoOnMission;
    private bool characterSelectOpen = false;

    private void Start()
    {
        _data = Data.Instance;
        charactersToGoOnMission = new List<(SavedCharacter, int)>();
        allowDuplicates = _data.townData.selectedEncounter.allowDuplicates;
        UpdateView();
    }
    
    public void AddCharacter(PlayerInformationData playerinformation, int charIndex)
    {
        if (FindFirstUnoccupied() != null)
        {
            var FirstUnoccupiedButton = FindFirstUnoccupied();
            FirstUnoccupiedButton.playerInformation = playerinformation;
            FirstUnoccupiedButton.characterIndex = charIndex;
            portraitButtons[FirstUnoccupiedButton.buttonIndex].animator.gameObject.SetActive(true);
            // portraitButtons[FirstUnoccupiedButton.buttonIndex].animator = givenCharacterPrefab.transform.Find("CharacterModel").GetComponent<Animator>().runtimeAnimatorController;
            portraitButtons[FirstUnoccupiedButton.buttonIndex].characterImage.sprite = playerinformation.characterSprite;
            // FirstUnoccupiedButton.button.GetComponent<LongPressButton>().enabled = true;
        }
    }

    public PortraitButtonData FindFirstUnoccupied()
    {
        foreach (PortraitButtonData x in portraitButtons)
        {
            if (x.playerInformation == null)
            {
                return x;
            }
        }
        return null;
    }

    public void RemoveCharacter(int charIndex)
    {
        foreach (PortraitButtonData x in portraitButtons)
        {
            if (x.characterIndex == charIndex)
            {
                Remove(x);
                break;
            }
        }
        embark.interactable = false;
    }

    public void OnClickTeamCharacterButton(int buttonIndex)
    {
        // if (!characterSelectOpen)
        // {
        //     characterSelectOpen = true;
        //     characterSelectView.OpenView();
        // }
        // else
        // {
        //     RemoveCharacterFromTeam(portraitButtons[buttonIndex].characterIndex);
        // }
        
        if (!characterSelectOpen)
        {
            characterSelectView.OpenView();
            characterSelectOpen = true;
        }
        else
        {
            characterSelectView.ExitView();
            characterSelectOpen = false;
        }
    }

    private void Remove(PortraitButtonData x)
    {
        if(x.characterIndex != -1)
        {
            portraitButtons[x.buttonIndex].animator.gameObject.SetActive(false);
            x.characterImage.sprite = emptySprite;
            x.playerInformation = null;
            x.characterIndex = -1;
            Reorder();
            // ToggleLongClick();
            //EnableCharacters();
        }
    }
    private void Reorder()
    {
        foreach (PortraitButtonData x in portraitButtons)
        {
            if (x.characterIndex != -1)
            {
                PlayerInformationData tempInformation = x.playerInformation;
                int tempIndex = x.characterIndex;
                portraitButtons[x.buttonIndex].animator.gameObject.SetActive(false);
                x.characterImage.sprite = emptySprite;
                x.playerInformation = null;
                x.characterIndex = -1;
                AddCharacter(tempInformation, tempIndex);
            }
        }
    }
    
    public void UpdateView()
    {
        for (int i = 0; i < characterButtons.Count; i++)
        {
            if (i < _data.Characters.Count)
            {
                characterButtons[i].gameObject.SetActive(true);
                characterButtons[i].characterIndex = i;
                characterButtons[i].portrait.gameObject.SetActive(true);
                characterButtons[i].portrait.sprite = _data.Characters[i].playerInformation.CharacterPortraitSprite;
                characterButtons[i].levelText.text = _data.Characters[i].level.ToString();
                // characterButtons[i].onHover.SetBool("select", AlreadySelected(i));
            }
            else
            {
                characterButtons[i].gameObject.SetActive(false);
            }
        }
        if (charactersToGoOnMission.Count == 3)
        {
            DisableCharacters();
        }
        else
        {
            EnableCharacters();
        }
        back.gameObject.SetActive(true);
    }
    

    public void SaveData()
    {
        foreach ((SavedCharacter, int) character in charactersToGoOnMission)
        {
            _data.Characters.Remove(character.Item1);
            _data.Characters.Insert(0, character.Item1);
        }
        _data.townData.allowEnemySelection = true;
        _data.townData.allowDuplicates = allowDuplicates;
    }

    public void OnCharacterButtonClick(int characterIndex)
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
    
    public void RemoveCharacterFromTeam(int characterIndex)
    {
        if (characterIndex > -1)
        {
            charactersToGoOnMission.RemoveAt(
                charactersToGoOnMission.FindIndex(character => character.Item2 == characterIndex));
            RemoveCharacter(characterIndex);
            // characterButtons[characterIndex].onHover.SetBool("select", false);
        }

        EnableCharacters();
    }
    public void AddCharacterToTeam(int characterIndex)
    {
        charactersToGoOnMission.Add((_data.Characters[characterIndex], characterIndex));
        AddCharacter(_data.Characters[characterIndex].playerInformation, characterIndex);
        // characterButtons[characterIndex].onHover.SetBool("select", true);
        if(charactersToGoOnMission.Count == 3)
        {
            embark.interactable = true;
            DisableCharacters();
        }
    }

    private void EnableCharacters()
     {
         for(int i=0; i<characterButtons.Count; i++)
         {
             if (characterButtons[i].gameObject.activeSelf)
             {
                 //characterButtons.transform.Find("Character").Find("Portrait").GetComponent<Image>().color = Color.white; //naudoti game object bet kazkaip su portrait neina
                 characterButtons[i].portrait.color = Color.white;
                // characterButtons[i].GetComponent<CharacterPortrait>().available = true;
                 // characterButtons[i].characterButton.available = true;
             }
         }
     }
     
     private void DisableCharacters()
     {
         for(int i=0; i<characterButtons.Count; i++)
         {
             if (characterButtons[i].gameObject.activeSelf && !AlreadySelected(characterButtons[i].characterIndex))
             {
                 characterButtons[i].portrait.color = Color.grey;
                 // characterButtons[i].characterButton.available = false;
             }
         }
     }

    private bool AlreadySelected(int characterIndex)
    {
        foreach((SavedCharacter, int) character in charactersToGoOnMission)
        {
            if(character.Item2 == characterIndex)
            {
                return true;
            }
        }
        return false;
    }

    //Epic poses
        //public void DisplaySelectedCharacters()
        //{
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
        //}
    
        //public void StopDisplayingSelectedCharacters()
        //{
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
        //}
    
    // private void ToggleLongClick()
    // {
    //     foreach (PortraitButtonData x in portraitButtons)
    //     {
    //         x.button.GetComponent<LongPressButton>().enabled = x.characterIndex != -1;
    //     }
    // }
    
    // public void DisplayCharacterInfo(GameObject button)
    // {
    //     foreach (PortraitButtonData x in portraitButtons)
    //     {
    //         if (x.button == button && x.characterIndex != -1)
    //         {
    //             // GameObject.Find("GameProgress").GetComponent<GameProgress>().DisplayCharacterTable(x.characterIndex);
    //             GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().DisplayCharacterTable(x.characterIndex);
    //             Debug.Log("Pakeisti sita vieta taip pat");
    //         }
    //     }
    // }
}
