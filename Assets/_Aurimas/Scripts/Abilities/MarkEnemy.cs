using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkEnemy : BaseAction
{
    private ChunkData target;
    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        if (target != null && target.GetCurrentPlayerInformation().Marker != null)
        {
            target.GetCurrentPlayerInformation().Marker = null;
            target = null;
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        target = GetSpecificGroundTile(position);
        target.GetCurrentPlayerInformation().Marker = gameObject;
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
