using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPulse : BaseAction
{
    void Start()
    {
        actionStateName = "SpearPulse";
    }

    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
            DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
        
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
    }
    

    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile,minAttackDamage,maxAttackDamage);
    }

    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile,MergedTileList);
    }
}
