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
    private CharacterAction abilityInformation;
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

    public void OnButtonClick()
    {
        selectAction.ActionSelection(abilityIndex, abilityInformation);
    }

    public void AbilityInformation(int abilityIndex, CharacterAction characterAction, SelectAction selectedAction)
    {
        this.abilityIndex = abilityIndex;
        abilityInformation = characterAction;
        selectAction = selectedAction;
    }
    
}
