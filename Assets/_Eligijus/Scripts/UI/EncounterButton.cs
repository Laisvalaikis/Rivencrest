using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class EncounterButton : Button
{

    [SerializeField] private Image image;
    [SerializeField] private Sprite selectedImage;
    [SerializeField] private EncounterController encounterController;
    [SerializeField] private EncounterButtonController encounterButtonController;
    private Encounter selectedEncounter;
    public bool deselectOnClickedSameTypeButton = true;
    private bool isSelectedEncounter = false;
    private Sprite _normalImage;
    private bool wasSelected = false;

    public override void OnSelect(BaseEventData eventData)
    {
        Debug.Log("nanana");
        if(!isSelectedEncounter)
        {
            
            SwapSpritesSelect();
            SelectEncounter();
            encounterButtonController.Select();
            isSelectedEncounter = true;
            encounterButtonController.SetDeselectData(eventData, this);
            base.OnSelect(eventData);
            // encounterButtonController.
        }

    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (isSelectedEncounter && !wasSelected)
        {
            wasSelected = true;
        }
        else if (isSelectedEncounter && wasSelected)
        {
            encounterButtonController.EncounterViewDisable();
        }

        base.OnPointerClick(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
    }

    public void DeselectAndClose()
    {
        encounterButtonController.DeselectButton();
        SwapSpritesDeselect();
        SelectEncounter();
        isSelectedEncounter = false;
        wasSelected = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Deselect()
    {
        if (wasSelected && isSelectedEncounter)
        {
            SwapSpritesDeselect();
            isSelectedEncounter = false;
            wasSelected = false;
        }
    }

    private void SwapSpritesSelect()
    {
        if (image != null && selectedImage != null)
        {
            _normalImage = image.sprite;
            image.sprite = selectedImage;
        }
    }

    private void SwapSpritesDeselect()
    {
        if (image != null && selectedImage != null)
        {
            image.sprite = _normalImage;
        }
    }

    public void AddEncounter(Encounter encounter)
    {
        selectedEncounter = encounter;
    }

    public void SelectEncounter()
    {
        encounterController.ChangeSelectedEncounter(selectedEncounter);
    }
}
