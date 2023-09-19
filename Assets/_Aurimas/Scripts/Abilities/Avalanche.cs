using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche : BaseAction
{
    private PlayerInformation _playerInformation;
    void Start()
    {
        actionStateName = "Avalanche";
        AttackHighlight = new Color32();

    }

    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            foreach (ChunkData chunk in _chunkList)
            {
                if (CanTileBeClicked(chunk.GetPosition()))
                {
                    ChunkData target = GetSpecificGroundTile(chunk.GetPosition());
                    DealRandomDamageToTarget(target,minAttackDamage,maxAttackDamage);
                    // _playerInformation.ApplyDebuff("IceSlow");
                    
                }
            }
            FinishAbility();
        }
    }

    protected override bool CanTileBeClicked(Vector3 position)
    {
        ChunkData chunkData = GetSpecificGroundTile(position);
        if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") &&
            !IsAllegianceSame(chunkData.GetPosition()) &&
            IsCharacterAffectedByCrowdControl(position))
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

    private bool IsCharacterAffectedByCrowdControl(Vector3 position)
    {
        if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player"))
        {
            ChunkData target = GetSpecificGroundTile(position);
            PlayerInformation playerInformationLocal = target.GetCurrentPlayerInformation();
            if (playerInformationLocal.Slow1 
                || playerInformationLocal.Slow2
                || playerInformationLocal.Slow3 
                || playerInformationLocal.Debuffs.Contains("Stun")
                || playerInformationLocal.Silenced || playerInformationLocal.CantMove)
            {
                return true;
            }
        }
        return false;
    }
}
