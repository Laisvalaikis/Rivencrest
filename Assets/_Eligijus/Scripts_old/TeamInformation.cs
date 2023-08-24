using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamInformation : MonoBehaviour
{
    
    [SerializeField] private SelectAction selectAction;
    [SerializeField] private PlayerTeams playerTeams;
    [SerializeField] private Image image;
    [SerializeField] private GameTileMap gameTileMap;
    [SerializeField] private List<GameObject> TeamCharacterPortraitList;
    [SerializeField] private List<GameObject> CharacterOnBoardList;
    [SerializeField] private List<PvPCharacterSelect> pvpCharacterSelects;
    //private PvPCharacterSelect _pvPCharacterSelect;
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
        CharacterOnBoardList = playerTeams.AliveCharacterList(teamIndex);
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i < CharacterOnBoardList.Count)
            {
                pvpCharacterSelects[i].characterOnBoard = CharacterOnBoardList[i];
                TeamCharacterPortraitList.Add(pvpCharacterSelects[i].GetCharacterPortraitFrame());
                pvpCharacterSelects[i].CreateCharatersPortrait();
                pvpCharacterSelects[i].SetSelectAction(selectAction);
                pvpCharacterSelects[i].gameTileMap = gameTileMap;
            }
            else
            {
                pvpCharacterSelects[i].characterOnBoard = null;
            }
            
        }
        image.enabled = CharacterOnBoardList.Count != 0;
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
            
            // for(int i = 0; i < TeamCharacterPortraitList.Count; i++)
            // {
            //     if(TeamCharacterPortraitList[i] != selected)
            //     {
            //         TeamCharacterPortraitList[i].GetComponent<Animator>().SetBool("select", false);
            //     }
            // }
            // selected.GetComponent<Animator>().SetBool("select", true);
            Debug.Log("Need to fix this");
        }
    public void DisableSelectionAll()
    {
        /*for (int i = 0; i < CharacterOnBoardList.Count; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("select", false);
        }*/
            
            // for (int i = 0; i < TeamCharacterPortraitList.Count; i++)
            // {
            //         TeamCharacterPortraitList[i].GetComponent<Animator>().SetBool("select", false);
            // }
            //
            Debug.Log("Need to fix this");
        }
    public void ChangeBoxSprites(Sprite main, Sprite extension, Sprite button)
    {
        image.sprite = main;
        for (int i = 0; i < transform.childCount; i++)
        {
            pvpCharacterSelects[i].extension.sprite = extension;
            pvpCharacterSelects[i].frame.sprite = button;
        }
    }
}
