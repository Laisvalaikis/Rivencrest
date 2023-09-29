using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityToUse
{
    public BaseAction action;
    public ChunkData chunkToCallOn;
    public bool isOffensive = true;
    public float coefficient = 0.5f;
}

public class AIBehaviour : MonoBehaviour
{
    private ChunkData[,] chunkArray;
    [SerializeField] private ActionManagerNew actionManager;
    private List<Ability> _abilities;
    private float confidence = 0.0f; //decrease when low health or debuffs, increase when high health and buffs. More likely to end turn on dangerZone and not fear chunk
    
    
    public void Start()
    {
        chunkArray = GameTileMap.Tilemap.GetChunksArray();
        _abilities = actionManager.GetAbilities();
    }
    private bool ShouldCharacterFearChunk(ChunkData chunk)
    {
        return false;
    }
    
    private void ExecuteAbilities()
    {
        while (true)
        {
            AbilityToUse abilityToUse = FindBestAbilityToUse();
            if (abilityToUse.action != null)
            {
                abilityToUse.action.ResolveAbility(abilityToUse.chunkToCallOn);
            }
            else
            {
                break; //No abilities that reach a character. In this case AI should move to a chunk from which it could reach
            }
        }
    }

    //Check how viable an ability is by calculating the coefficient
    //Coefficient calculations should be modified through testing
    //Dont forget to implement hard / easy modes
    private float CalculateCoefficient(AbilityToUse abilityToUse)
    {
        //todo: implement black box magic
        //if ability will 100% kill enemy player
        if (abilityToUse.action.minAttackDamage >=
            abilityToUse.chunkToCallOn.GetCurrentPlayerInformation().GetHealth() && abilityToUse.isOffensive)
        {
            return 1f;
        }
        return 0.5f;
    }
    
    
    //Looks through all available abilities and finds one with best coefficient
    //Should be called after each ability
    private AbilityToUse FindBestAbilityToUse()
    {
        float maxCoef = -1f;
        AbilityToUse abilityToUse = new AbilityToUse();
        foreach (var ability in _abilities)
        {
            if (ability.enabled)
            {
                BaseAction action = ability.Action;
                AbilityToUse tempAbility = AbilityToBeUsed(action);
                if (tempAbility.action != null)
                {
                    var coef = CalculateCoefficient(tempAbility);
                    if (coef > maxCoef)
                    {
                        maxCoef = coef;
                        abilityToUse = tempAbility;
                    }
                }
            }
        }
        abilityToUse.coefficient = maxCoef;
        return abilityToUse;
    }
    
    
    //Returns action and chunk on which to call ability
    //Picks chunk with lowest health player
    private AbilityToUse AbilityToBeUsed(BaseAction action)
    {
        AbilityToUse abilityToUse = new AbilityToUse();
        action.CreateAvailableChunkList(action.AttackRange);
        var chunksForAttack = action.GetChunkList();
        int minHealth = int.MaxValue;
        foreach (var chunk in chunksForAttack)
        {
            PlayerInformation playerInformation = chunk.GetCurrentPlayerInformation();
            if (playerInformation != null && action.CanTileBeClicked(chunk))
            {
                if (playerInformation.GetHealth() < minHealth)
                {
                    abilityToUse.chunkToCallOn = chunk;
                    abilityToUse.action = action;
                    minHealth = playerInformation.GetHealth();
                }
            }
        }
        return abilityToUse;
    }
}
