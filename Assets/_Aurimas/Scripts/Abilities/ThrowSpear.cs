using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpear : BaseAction
//reikes tvarkyt problema gali buti nes nera FindIndexOfTile eligijaus knowledge reikes cia
{
    [SerializeField] private GameObject spearPrefab;
    private bool laserGrid = false;

    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        for (int y = -radius; y <= radius; y++)
        {
            if (Mathf.Abs(y) == radius)
            {
                int targetY = centerY + y;
                if (targetY >= 0 && targetY < chunksArray.GetLength(1))
                {
                    ChunkData chunk = chunksArray[0, targetY];
                    if (chunk != null && !chunk.TileIsLocked())
                    {
                        _chunkList.Add(chunk);
                        HighlightGridTile(chunk);
                        
                    }
                }
            }
        }
    }
    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        CreateGrid(startChunk, AttackRange);
    }
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
