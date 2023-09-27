using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche : BaseAction
{
    private PlayerInformation _playerInformation;
    void Start()
    {
        AttackHighlight = new Color32(123,156, 178,255);
        AttackHighlightHover = new Color32(103, 136, 158, 255);
        CharacterOnGrid = new Color32(146, 212, 255, 255);
    }
    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            foreach (ChunkData chunk in _chunkList)
            {
                if (CanTileBeClicked(chunk))
                {
                    DealRandomDamageToTarget(chunk,minAttackDamage,maxAttackDamage);
                    // _playerInformation.ApplyDebuff("IceSlow");
                }
            }
            FinishAbility();
        }
    }

    public override bool CanTileBeClicked(Vector3 position)
    {
        ChunkData chunkData = GetSpecificGroundTile(position);
        if (CheckIfSpecificInformationType(chunkData, InformationType.Player) &&
            !IsAllegianceSame(chunkData) &&
            IsCharacterAffectedByCrowdControl(chunkData))
        {
            return true;
        }
        return false;
    }
    private bool IsCharacterAffectedByCrowdControl(ChunkData chunk)
    {
        if (CheckIfSpecificInformationType(chunk, InformationType.Player))
        {
            PlayerInformation playerInformationLocal = chunk.GetCurrentPlayerInformation();
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
