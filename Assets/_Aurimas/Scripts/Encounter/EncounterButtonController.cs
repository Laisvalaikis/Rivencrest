using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EncounterButtonController : MonoBehaviour
{
    private BaseEventData deselectEventData;
    private EncounterButton _encounterButton;
    public View missionInformationView;
    [SerializeField] private UnityEvent onDeselect;

    public void Select()
    {
        missionInformationView.OpenView();
        DeselectButton();
    }

    public void SetDeselectData(BaseEventData button, EncounterButton encounterButton)
    {
        deselectEventData = button;
        _encounterButton = encounterButton;
    }

    public void DeselectButton()
    {
        if (_encounterButton != null)
        {
            _encounterButton.Deselect();
            _encounterButton = null;
            deselectEventData = null;
        }
    }

    public void EncounterViewDisable()
    {
        missionInformationView.ExitView();
    }

    public void DeselectAndClose()
    {
        _encounterButton.DeselectAndClose();
    }
}
