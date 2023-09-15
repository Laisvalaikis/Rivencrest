using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche : BaseAction
{
    private PlayerInformation _playerInformation;
    // Start is called before the first frame update
    void Start()
    {
        actionStateName = "Avalanche";
    }

    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            foreach (ChunkData tile in ReturnGeneratedChunks())
            {
                if (CanTileBeClicked(tile.GetPosition()))
                {
                    ChunkData target = GetSpecificGroundTile(tile.GetPosition());
                    DealRandomDamageToTarget(target,minAttackDamage,maxAttackDamage);
                    // _playerInformation.ApplyDebuff("IceSlow");
                    
                }
            }
            FinishAbility();
        }
    }

    public override bool CanTileBeClicked(Vector3 tile)
    {
        ChunkData chunkData = GetSpecificGroundTile(tile);
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") &&
            !isAllegianceSame(chunkData.GetPosition()) &&
            isCharacterAffectedByCrowdControl(tile))
        {
            return true;
        }

        return false;
    }

    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile,minAttackDamage,maxAttackDamage);
    }

    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile,MergedTileList);
    }

    private bool isCharacterAffectedByCrowdControl(Vector3 position)
    {
        if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player"))
        {
            ChunkData target = GetSpecificGroundTile(position);
            if (target.GetCurrentPlayerInformation().Slow1 
                || target.GetCurrentPlayerInformation().Slow2
                || target.GetCurrentPlayerInformation().Slow3 
                || target.GetCurrentPlayerInformation().Debuffs.Contains("Stun")
                || target.GetCurrentPlayerInformation().Silenced || target.GetCurrentPlayerInformation().CantMove)
            {
                return true;
            }
        }
        return false;
    }
}
