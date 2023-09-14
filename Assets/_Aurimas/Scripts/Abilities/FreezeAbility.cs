using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAbility : BaseAction
{
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        var directionVectors = new List<(int, int)> //cube around player
        {
            (1, 0),
            (1, 1),
            (0, 1),
            (-1, 1),
            (-1, 0),
            (-1, -1),
            (0, -1),
            (1, -1)
            
        };
        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(centerChunk.GetChunkCenterPosition(), x.Item1, x.Item2, blockingLayer, "Wall");
            if (isGround && (!isBlockingLayer || isPlayer || isWall))
            {
                ChunkData chunk = GameTileMap.Tilemap.GetRandomChunkAround(1,1);
                _chunkList.Add(chunk);
                HighlightGridTile(chunk);
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
