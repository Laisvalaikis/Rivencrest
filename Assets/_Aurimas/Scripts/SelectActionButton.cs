using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectActionButton : MonoBehaviour
{
    private SelectAction selectAction;
    private int abilityIndex;
    private Ability abilityInformation;
    private HelpTable _helpTable;
    public Image abilityButtonBackground;
    public Image AbilityButtonImage;
    public Image characterPortrait;
    public TextMeshProUGUI healthBar;
    public Image staminaButtonBackground;
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnHover()
    {
        _helpTable.EnableTableForCharacters(abilityInformation);
    }

    public void OffHover()
    {
        _helpTable.DisableHelpTable();
    }

    public void OnButtonClick()
    {
        selectAction.ActionSelection(abilityIndex, abilityInformation.Action);
    }

    public void AbilityInformation(int abilityIndex, HelpTable helpTable, Ability characterAction, SelectAction selectedAction)
    {
        this.abilityIndex = abilityIndex;
        abilityInformation = characterAction;
        selectAction = selectedAction;
        _helpTable = helpTable;
    }
    
}
