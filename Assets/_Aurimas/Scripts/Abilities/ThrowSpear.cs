using UnityEngine;

public class ThrowSpear : BaseAction
//reikes tvarkyt problema gali buti nes nera FindIndexOfTile eligijaus knowledge reikes cia
{
    [SerializeField] private GameObject spearPrefab;
    private bool laserGrid = true;

    public override void CreateAvailableChunkList(int attackRange)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        (_, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        for (int y = -attackRange; y <= attackRange; y++)
        {
            if (Mathf.Abs(y) == attackRange)
            {
                int targetY = centerY + y;
                if (targetY >= 0 && targetY < chunksArray.GetLength(1))
                {
                    ChunkData chunk = chunksArray[0, targetY];
                    if (chunk != null && !chunk.TileIsLocked())
                    {
                        _chunkList.Add(chunk);
                    }
                }
            }
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        spawnedCharacter = Instantiate(spearPrefab, chunk.GetPosition(), Quaternion.identity);
        FinishAbility();
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
