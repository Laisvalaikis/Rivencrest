using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PvPCharacterSelect : MonoBehaviour
{
    public GameTileMap gameTileMap; //cia private padaryt
    [SerializeField] private GameObject characterPortraitFrame;
    public Image extension;
    public Image frame;
    [SerializeField] private Image characterPortraitSprite;
    private GameObject characterOnBoard;
    private PlayerInformation _playerInformation;
    private PlayerInformationData _playerInformationData;
    private SelectAction _selectAction;
   // private SelectActionButton _selectActionButton;
    private bool isButtonAvailable = true;

    void Start()
    {
        //CreateCharatersPortrait();
    }

    public void CreateCharatersPortrait()
    {
        if (characterOnBoard != null)
        {
            _playerInformationData = _playerInformation.playerInformationData;
            characterPortraitSprite.sprite = _playerInformationData.CharacterPortraitSprite;

            gameObject.SetActive(true);
            isButtonAvailable = true;
        }
        else
        {
            gameObject.SetActive(false);
            isButtonAvailable = false;
        }
    }

    public void SetPortraitCharacter(GameObject character, PlayerInformation characterInformation)
    {
        characterOnBoard = character;
        _playerInformation = characterInformation;
    }

    public GameObject GetCharacterPortraitFrame()
    {
        return characterPortraitFrame;
    }

    public void SelectPortrait()
    {
        if (gameTileMap.GetCurrentCharacter() != characterOnBoard) // GameObject.Find("GameInformation").GetComponent<GameInformation>().canButtonsBeClicked fix this bs
        {
            gameTileMap.SetCurrentCharacter(characterOnBoard);
            isButtonAvailable = false;
            _selectAction.SetCurrentCharacter(characterOnBoard);
            Debug.Log("Selected");
        }
        else if (gameTileMap.GetCurrentCharacter() == characterOnBoard)
        {
            gameTileMap.DeselectCurrentCharacter();
            _selectAction.DeSetCurrentCharacter();
            isButtonAvailable = true;
            Debug.Log("Deselected");
        }
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

    public void SetSelectAction(SelectAction selectAction)
    {
        _selectAction = selectAction;
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
