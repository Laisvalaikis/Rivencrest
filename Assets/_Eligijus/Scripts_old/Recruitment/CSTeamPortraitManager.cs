using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CSTeamPortraitManager : MonoBehaviour
{
    public List<PortraitButtonData> PortraitButtonList;
    public List<Animator> posingCharacters;
    [HideInInspector]public GameObject ActiveButton = null;
    public Sprite EmptySprite;
    public Data _data;
    public void AddCharacter(GameObject givenCharacterPrefab)
    {
        if (ActiveButton != null && FindByButton(ActiveButton) != null)
        {
            FindByButton(ActiveButton).CharacterPrefab = givenCharacterPrefab;
            ActiveButton.transform.Find("ButtonPortrait").GetComponent<Image>().sprite =
                givenCharacterPrefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
        }
        else if (FindFirstUnoccupied() != null) {
            var FirstUnoccupiedButton = FindFirstUnoccupied();
            FirstUnoccupiedButton.CharacterPrefab = givenCharacterPrefab;
            FirstUnoccupiedButton.button.transform.Find("ButtonPortrait").GetComponent<Image>().sprite =
                givenCharacterPrefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
        }
    }
    public void AddCharacterInCS3(GameObject givenCharacterPrefab, int charIndex)
    {
        if (FindFirstUnoccupied() != null)
        {
            var FirstUnoccupiedButton = FindFirstUnoccupied();
            FirstUnoccupiedButton.CharacterPrefab = givenCharacterPrefab;
            FirstUnoccupiedButton.characterIndex = charIndex;
            posingCharacters[FirstUnoccupiedButton.buttonIndex].gameObject.SetActive(true);
            posingCharacters[FirstUnoccupiedButton.buttonIndex].runtimeAnimatorController = givenCharacterPrefab.transform.Find("CharacterModel").GetComponent<Animator>().runtimeAnimatorController;
            FirstUnoccupiedButton.button.transform.Find("ButtonPortrait").GetComponent<Image>().sprite =
                givenCharacterPrefab.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            FirstUnoccupiedButton.button.GetComponent<LongPressButton>().enabled = true;
        }
    }
    public PortraitButtonData FindByButton(GameObject button)
    {
        foreach(PortraitButtonData x in PortraitButtonList)
        {
            if(x.button == button)
            {
                return x;
            }
        }
        return null;
    }
    public PortraitButtonData FindFirstUnoccupied()
    {
        foreach (PortraitButtonData x in PortraitButtonList)
        {
            if (x.CharacterPrefab == null)
            {
                return x;
            }
        }
        return null;
    }
    public void DisableButtonSelections()
    {
        foreach (PortraitButtonData x in PortraitButtonList)
        {
            x.button.transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("select", false);
        }
    }
    public void SelectButton(GameObject button)
    {
        if (button == ActiveButton) {
            DisableButtonSelections();
            ActiveButton = null;
        }
        else
        {
            DisableButtonSelections();
            button.transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("select", true);
            ActiveButton = button;
        }
    }
    public void OnHover(GameObject button)
    {
        button.transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", true);
    }
    public void OffHover(GameObject button)
    {
        button.transform.Find("ButtonFrame").GetComponent<Animator>().SetBool("hover", false);
    }
    public void ClearPortraits()
    {
        foreach (PortraitButtonData x in PortraitButtonList)
        {
            x.CharacterPrefab = null;
            x.button.transform.Find("ButtonPortrait").GetComponent<Image>().sprite = EmptySprite;
            if (SceneManager.GetActiveScene().name == "CharacterSelect3" && x.characterIndex != -1)
            {
                GameObject.Find("CanvasCamera").transform.Find("CharacterButtons").GetChild(x.characterIndex).transform.Find("Hover").GetComponent<Animator>().SetBool("select", false);
                //EnableCharacters();
            }
            x.characterIndex = -1;
        }
        DisableButtonSelections();
        ActiveButton = null;
    }
    public void RemoveCharacter()
    {
        foreach(PortraitButtonData x in PortraitButtonList)
        {
            if(x.button == ActiveButton)
            {
                x.CharacterPrefab = null;
                x.button.transform.Find("ButtonPortrait").GetComponent<Image>().sprite = EmptySprite;
                if(!AlreadySelected(_data.currentCharacterIndex) || _data.currentCharacterIndex == x.characterIndex)
                {
                    GameObject.Find("CanvasCamera").transform.Find("Add").GetComponent<Button>().interactable = true;
                }
                x.characterIndex = -1;
            }
        }
        GameObject.Find("CanvasCamera").transform.Find("Embark").GetComponent<Button>().interactable = false;
    }
    public void RemoveCharacter(int charIndex)
    {
        foreach (PortraitButtonData x in PortraitButtonList)
        {
            if (x.characterIndex == charIndex)
            {
                Remove(x);
            }
        }
        GameObject.Find("CanvasCamera").transform.Find("Embark").GetComponent<Button>().interactable = false;
    }
    public void RemoveCharacter(GameObject button)
    {
        GameObject.Find("GameProgress").GetComponent<CharacterSelect>().RemoveCharacterFromTeam(FindByButton(button).characterIndex);
    }
    private void Remove(PortraitButtonData x)
    {
        if(x.characterIndex != -1)
        {
            x.CharacterPrefab = null;
            posingCharacters[x.buttonIndex].gameObject.SetActive(false);
            x.button.transform.Find("ButtonPortrait").GetComponent<Image>().sprite = EmptySprite;
            x.characterIndex = -1;
            Reorder();
            ToggleLongClick();
            //EnableCharacters();
        }
    }
    private void Reorder()
    {
        foreach (PortraitButtonData x in PortraitButtonList)
        {
            if (x.characterIndex != -1)
            {
                GameObject tempPrefab = x.CharacterPrefab;
                int tempIndex = x.characterIndex;
                x.CharacterPrefab = null;
                posingCharacters[x.buttonIndex].gameObject.SetActive(false);
                x.button.transform.Find("ButtonPortrait").GetComponent<Image>().sprite = EmptySprite;
                x.characterIndex = -1;
                AddCharacterInCS3(tempPrefab, tempIndex);
            }
        }
    }
    private void ToggleLongClick()
    {
        foreach (PortraitButtonData x in PortraitButtonList)
        {
            x.button.GetComponent<LongPressButton>().enabled = x.characterIndex != -1;
        }
    }
    public void DisplayCharacterInfo(GameObject button)
    {
        foreach (PortraitButtonData x in PortraitButtonList)
        {
            if (x.button == button && x.characterIndex != -1)
            {
                // GameObject.Find("GameProgress").GetComponent<GameProgress>().DisplayCharacterTable(x.characterIndex);
                GameObject.Find("Canvas").transform.Find("CharacterTable").GetComponent<CharacterTable>().DisplayCharacterTable(x.characterIndex);
                Debug.Log("Pakeisti sita vieta taip pat");
            }
        }
    }
    public bool AlreadySelected(int charIndex)
    {
        bool alreadySelected = false;
        foreach(PortraitButtonData button in PortraitButtonList)
        {
            if (button.characterIndex == charIndex)
            {
                alreadySelected = true;
            }
        }
        return alreadySelected;
    }
}
