using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpear : BaseAction
//reikes tvarkyt problema gali buti nes nera FindIndexOfTile eligijaus knowledge reikes cia
{
    [SerializeField] private GameObject spearPrefab;
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        spawnedCharacter = Instantiate(spearPrefab, position, Quaternion.identity);
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

    // public override void RefillActionPoints()
    // {
    //     AvailableAttacks = 1;
    //     AbilityPoints++;
    //     if (AbilityPoints >= AbilityCooldown && spawnedCharacter != null)
    //     {
    //         Destroy(spawnedCharacter);
    //         spawnedCharacter = null;
    //     }
    // }
    //
    // public override void SpecificAbilityAction(GameObject character = null)
    // {
    //     AbilityPoints++;
    //     spawnedCharacter = null;
    // }
}
