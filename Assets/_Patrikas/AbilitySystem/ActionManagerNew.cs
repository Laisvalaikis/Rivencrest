using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerNew : MonoBehaviour
{
    [SerializeField]
    private List<Ability> _abilities;

    [SerializeField]
    private int availableAttackPoints;

    [SerializeField] 
    private int availableMovementPoints;

    private bool hasSlowAbilityBeenCast = false;

    public List<Ability> GetAbilities()
    {
        return _abilities;
    }
    
    public virtual void RemoveActionPoints()
    {
        availableAttackPoints--;
    }
    
    public void RemoveAllActionPoints() 
    {
        availableMovementPoints = 0;
        hasSlowAbilityBeenCast = true;

    }
}

