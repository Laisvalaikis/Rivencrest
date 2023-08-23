using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction : MonoBehaviour
{
    public virtual void OnTileClick(Vector3 mousePosition)
    {
        
    }
    
    public virtual void DealRandomDamageToTarget(GameObject target, int minDamage, int maxDamage)
    {
        
    }
    
    public virtual void ResolveAbility(GameObject clickedTile)
    {
        Debug.Log("We are in CharacterAction");
    }
}//visos visu abilities funkcijos
