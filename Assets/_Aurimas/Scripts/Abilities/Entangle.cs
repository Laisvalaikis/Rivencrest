using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entangle : BaseAction
{
    void Start()
    {
        actionStateName = "Entangle";
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        DealRandomDamageToTarget(chunkData,minAttackDamage,maxAttackDamage);
        chunkData.GetCurrentPlayerInformation().ApplyDebuff("CantMove");
        FinishAbility();
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    
}
