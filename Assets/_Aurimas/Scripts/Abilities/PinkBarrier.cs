using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBarrier : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        GetSpecificGroundTile(position).GetCurrentPlayerInformation().BarrierProvider = gameObject;
       // GetSpecificGroundTile(position, 0, 0, blockingLayer).GetComponent<GridMovement>().AvailableMovementPoints++;
        FinishAbility();

        
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
}
