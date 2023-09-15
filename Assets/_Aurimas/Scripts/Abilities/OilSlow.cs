using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlow : BaseAction
{
    private GameObject SlowedTarget = null;
    
    public override void OnTurnStart()
    {
        if (SlowedTarget != null)
        {
            SlowedTarget = null;
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        GetSpecificGroundTile(position).GetCurrentPlayerInformation().ApplyDebuff("OilSlow");
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
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
