using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PvPCharacterSelect : MonoBehaviour
{
    [HideInInspector] public GameObject characterOnBoard;
    [HideInInspector] public GameObject characterPortraitFrame;
    private bool isButtonAvailable = true;
    void Start()
    {
        //CreateCharatersPortrait();
    }

    public void CreateCharatersPortrait()
    {
        if (characterOnBoard != null)
        {
            characterPortraitFrame = transform.Find("Character").gameObject;
            transform.parent.gameObject.GetComponent<TeamInformation>().AddPortraitToList(characterPortraitFrame);
            characterOnBoard.GetComponent<PlayerInformation>().characterPortrait = characterPortraitFrame;
            characterOnBoard.GetComponent<PlayerInformation>().TeamManager = transform.parent.gameObject;
            transform.Find("Character").GetChild(0).gameObject.GetComponent<Image>().sprite =
                    characterOnBoard.GetComponent<PlayerInformation>().CharacterPortraitSprite;
            gameObject.SetActive(true);
            isButtonAvailable = true;
        }
        else
        {
            gameObject.SetActive(false);
            isButtonAvailable = false;
        }
    }
    public void SelectPortrait()
    {
        if (isButtonAvailable && GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked)
        {
            if (characterOnBoard.GetComponent<PlayerInformation>().health > 0)
            {
                if (characterPortraitFrame.GetComponent<Animator>().GetBool("select"))
                {
                    GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().DeselectTeam(characterOnBoard);
                }
                else
                {
                    GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectACharacter(characterOnBoard);
                }
            }
        }
    }
    public void OnHover()
    {
        if (isButtonAvailable && GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked)
        {
            if (characterOnBoard.GetComponent<PlayerInformation>().health > 0)
            {
                characterPortraitFrame.GetComponent<Animator>().SetBool("hover", true);
                GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = true;
            }
            else
            {
                isButtonAvailable = false;
            }
        }
    }
    public void OffHover()
    {
        if (isButtonAvailable && GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked)
        {
            characterPortraitFrame.GetComponent<Animator>().SetBool("hover", false);
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = false;
        }
    }
    /*private void Update()
    {
        if(characterOnBoard.GetComponent<PlayerInformation>().health <= 0)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.grey;
        }
    }*/
}
