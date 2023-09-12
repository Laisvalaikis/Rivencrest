using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAbility : BaseAction
{
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
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
        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, blockingLayer, "Wall");
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
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
}
