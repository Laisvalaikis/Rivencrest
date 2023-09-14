using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAbility : BaseAction
{
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        (int x, int y) = centerChunk.GetIndexes();
        List<ChunkData> damageTiles = new List<ChunkData>();
        var directionVectors = new List<(int, int)> //cube around player
        {
            (AttackRange, 0),
            (AttackRange, AttackRange),
            (0, AttackRange),
            (-AttackRange, AttackRange),
            (-AttackRange, 0),
            (-AttackRange, -AttackRange),
            (0, -AttackRange),
            (AttackRange, -AttackRange)
        };
        if (GameTileMap.Tilemap.CheckBounds(AttackRange, AttackRange))
        {
            ChunkData temp = GameTileMap.Tilemap.GetChunkDataByIndex(AttackRange, AttackRange);
            damageTiles.Add(temp);
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
        ChunkData chunkData = GetSpecificGroundTile(position);
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        FinishAbility();
    }
    public List<ChunkData> CreateDamageTileList(Vector3 position)
    {
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
        (int x, int y) = chunk.GetIndexes();
        List<ChunkData> damageTiles = new List<ChunkData>();
        var directionVectors = new List<(int, int)>
        {
            (AttackRange, 0),
            (AttackRange, AttackRange),
            (0, AttackRange),
            (-AttackRange, AttackRange),
            (-AttackRange, 0),
            (-AttackRange, -AttackRange),
            (0, -AttackRange),
            (AttackRange, -AttackRange)
        };
        foreach (var direction in directionVectors)
        {
            if (GameTileMap.Tilemap.CheckBounds(direction.Item1, direction.Item2))
            {
                ChunkData temp = GameTileMap.Tilemap.GetChunkDataByIndex(direction.Item1, direction.Item2);
                damageTiles.Add(temp);
            }
        }
        return damageTiles;
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
