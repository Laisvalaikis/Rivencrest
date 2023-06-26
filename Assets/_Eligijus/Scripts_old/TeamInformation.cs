using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamInformation : MonoBehaviour
{
    public List<GameObject> TeamCharacterPortraitList;
    public List<GameObject> CharacterOnBoardList;
    public int teamIndex;

    void Awake()
    {
        TeamCharacterPortraitList = new List<GameObject>();
        CharacterOnBoardList = new List<GameObject>();
    }
    public void AddPortraitToList(GameObject character)
    {
        TeamCharacterPortraitList.Add(character);
    }
    public void AddCharacterToList(GameObject character)
    {
        TeamCharacterPortraitList.Add(character);
    }
    public void ModifyList()
    {
        TeamCharacterPortraitList.Clear();
        CharacterOnBoardList = GameObject.Find("GameInformation").GetComponent<PlayerTeams>().AliveCharacterList(teamIndex);
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i < CharacterOnBoardList.Count)
            {
                transform.GetChild(i).GetComponent<PvPCharacterSelect>().characterOnBoard = CharacterOnBoardList[i];
            }
            else
            {
                transform.GetChild(i).GetComponent<PvPCharacterSelect>().characterOnBoard = null;
            }
            transform.GetChild(i).GetComponent<PvPCharacterSelect>().CreateCharatersPortrait();
        }
        if(CharacterOnBoardList.Count == 0)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
    }
    public void DisableSelection(GameObject selected)
    {
        /*for (int i = 0; i < CharacterOnBoardList.Count; i++)
        {
            if(transform.GetChild(i) != selected)
            {
                transform.GetChild(i).GetComponent<Animator>().SetBool("select", false);
            }
            selected.GetComponent<Animator>().SetBool("select", true);
        }*/
            
            for(int i = 0; i < TeamCharacterPortraitList.Count; i++)
            {
                if(TeamCharacterPortraitList[i] != selected)
                {
                    TeamCharacterPortraitList[i].GetComponent<Animator>().SetBool("select", false);
                }
            }
            selected.GetComponent<Animator>().SetBool("select", true);
            Debug.Log("Need to fix this");
        }
    public void DisableSelectionAll()
    {
        /*for (int i = 0; i < CharacterOnBoardList.Count; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("select", false);
        }*/
            
            for (int i = 0; i < TeamCharacterPortraitList.Count; i++)
            {
                    TeamCharacterPortraitList[i].GetComponent<Animator>().SetBool("select", false);
            }
            
        }
    public void ChangeBoxSprites(Sprite main, Sprite extension, Sprite button)
    {
        GetComponent<Image>().sprite = main;
        for (int i = 0; i < transform.childCount; i++)
        {
                transform.GetChild(i).Find("Extension").GetComponent<Image>().sprite = extension;
                transform.GetChild(i).Find("Frame").GetComponent<Image>().sprite = button;
        }
    }
}
