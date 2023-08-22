using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAction : MonoBehaviour
{
    private ActionManagerNew actionManagerNew;
    private List<Ability> playerAbilities;
    private GameObject currentPlayer;
    [SerializeField] private List<SelectActionButton> abilityButtons;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void GetAbilities()
    {
        playerAbilities = currentPlayer.GetComponent<ActionManagerNew>().ReturnAbilities();
    }
}
