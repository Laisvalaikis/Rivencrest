using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PvPCharacterSelect : MonoBehaviour
{
    public GameTileMap gameTileMap; //cia private padaryt
    public GameObject characterOnBoard;
    [SerializeField] private GameObject characterPortraitFrame;
    public Image extension;
    public Image frame;
    [SerializeField] private Image characterPortraitSprite;
    private PlayerInformationData _playerInformation;
    private bool isButtonAvailable = true;
    
    void Start()
    {
        //CreateCharatersPortrait();
    }

    public void CreateCharatersPortrait()
    {
        if (characterOnBoard != null)
        {
            _playerInformation = characterOnBoard.GetComponent<PlayerInformation>().playerInformationData;
            // _playerInformation.TeamManager = transform.parent.gameObject;
            characterPortraitSprite.sprite = _playerInformation.CharacterPortraitSprite;

            gameObject.SetActive(true);
            isButtonAvailable = true;
        }
        else
        {
            gameObject.SetActive(false);
            isButtonAvailable = false;
        }
    }

    public GameObject GetCharacterPortraitFrame()
    {
        return characterPortraitFrame;
    }

    public void SelectPortrait()
    {
        if (isButtonAvailable) // GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked fix this bs
        {
            gameTileMap.SetCurrentCharacter(characterOnBoard);
            
            if (characterOnBoard.GetComponent<PlayerInformation>().health > 0)
            {
                // if (characterPortraitFrame.GetComponent<Animator>().GetBool("select"))
                // {
                //     GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().DeselectTeam(characterOnBoard);
                // }
                // else
                // {
                //     GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().SelectACharacter(characterOnBoard);
                // }
            }
        }
    }
    public void DeselectPortrait()
    {
        if (isButtonAvailable)
        {
            gameTileMap.DeselectCurrentCharacter();
        }
    }
    public void OnHover()
    {
        // if (isButtonAvailable && GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked)
        // {
        //     if (characterOnBoard.GetComponent<PlayerInformation>().health > 0)
        //     {
        //         characterPortraitFrame.GetComponent<Animator>().SetBool("hover", true);
        //         GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = true;
        //     }
        //     else
        //     {
        //         isButtonAvailable = false;
        //     }
        // }
    }
    public void OffHover()
    {
        // if (isButtonAvailable && GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked)
        // {
        //     characterPortraitFrame.GetComponent<Animator>().SetBool("hover", false);
        //     GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = false;
        // }
    }
    /*private void Update()
    {
        if(characterOnBoard.GetComponent<PlayerInformation>().health <= 0)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.grey;
        }
    }*/
}
