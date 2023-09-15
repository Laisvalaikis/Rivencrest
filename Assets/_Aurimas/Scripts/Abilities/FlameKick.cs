using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameKick : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        Vector3 pushDirection = chunkData.GetPosition() - gameObject.transform.position;
        DealRandomDamageToTarget(chunkData,minAttackDamage,maxAttackDamage);
        if (CheckIfSpecificLayer(chunkData.GetPosition(), (int)pushDirection.x, (int)pushDirection.y, groundLayer)
            && !CheckIfSpecificLayer(chunkData.GetPosition(), (int)pushDirection.x, (int)pushDirection.y, blockingLayer))
        {
           //  chunkData.GetPosition() += pushDirection;
        }
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