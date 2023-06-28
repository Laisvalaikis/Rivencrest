using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string buttonState;
    private bool _isSelected = false;
    private GameInformation gameInformation;
    public HelpTable _helpTable;
    public GameObject actionButtonFrame;
    public ButtonManager buttonManager;
    void Start()
    {
        gameInformation = GameObject.Find("GameInformation").GetComponent<GameInformation>();
    }

    public void ChangePlayersState()
    {

        transform.GetChild(0).GetComponent<Animator>().SetBool("select", true);
        _isSelected = true;
        _helpTable.closeHelpTable();
        GameObject character;
        if (gameInformation.SelectedCharacter != null)
        {
            character = gameInformation.SelectedCharacter;
        }
        else {
            character = gameInformation.InspectedCharacter;
        }
        character.GetComponent<PlayerInformation>().currentState = buttonState;
        if (buttonState == "Movement")
        {
            gameInformation.DisableGrids();;
            character.GetComponent<GridMovement>().EnableGrid();
        }
        else //galima prideti else if jei kazkokie jau special abilities.
        {
            gameInformation.DisableGrids();
            if (character.GetComponent<ActionManager>().FindActionByName(buttonState) != null)
            {
                character.GetComponent<ActionManager>().FindActionByName(buttonState).EnableGrid();
            }
            
        }
        buttonManager.DisableSelection(actionButtonFrame);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            _helpTable.EnableTableForInGameRightClick(buttonState);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _helpTable.hasActionButtonBeenEntered = true;
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", true);
        _helpTable.EnableTableForInGameRightClick(buttonState);
        gameInformation.isBoardDisabled = true;
        EnableGridPreview(buttonState);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject character;
        if (gameInformation.SelectedCharacter != null)
        {
            character = gameInformation.SelectedCharacter;
        }
        else {
            character = gameInformation.InspectedCharacter;
        }
        _helpTable.hasActionButtonBeenEntered = false;
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", false);
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().isBoardDisabled = gameInformation.helpTableOpen;
        _helpTable.closeHelpTable();
        EnableGridPreview(character.GetComponent<PlayerInformation>().currentState);
        
    }

    private void EnableGridPreview(string selectedButtonState)
    {
        GameObject character;
        if (gameInformation.SelectedCharacter != null)
        {
            character = gameInformation.SelectedCharacter;
        }
        else {
            character = gameInformation.InspectedCharacter;
        }
        if (selectedButtonState == "Movement")
        {
            gameInformation.DisableGrids();
            character.GetComponent<GridMovement>().EnableGrid();
        }
        else //galima prideti else if jei kazkokie jau special abilities.
        {
            gameInformation.DisableGrids();
            // panaudoti DisableWayTiles()
            if (character.GetComponent<ActionManager>().FindActionByName(selectedButtonState) != null)
            {
                character.GetComponent<ActionManager>().FindActionByName(selectedButtonState).EnableGrid();
            }
            
        }
    }
}
