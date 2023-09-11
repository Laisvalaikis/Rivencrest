using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingRitual : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        // if (CanTileBeClicked(position))
        // {
        foreach (ChunkData tile in ReturnGeneratedChunks())
        {
            DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
        }
        
        base.ResolveAbility(position);
            
            
            
            FinishAbility();
        // }
        
    }

    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                if (highlightTile != null)
                {
                    highlightTile.SetHighlightColor(Color.magenta);
                }
            }
        }
        else if (hoveredChunk == null || !hoveredChunk.GetTileHighlight().isHighlighted)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                if (highlightTile != null)
                {
                    highlightTile.SetHighlightColor(Color.green);
                }
            }
        }
    }
    
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        
        //Merging into one list
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == radius)
                {
                    int targetX = centerX + x;
                    int targetY = centerY + y;

                    // Ensuring we don't go out of array bounds.
                    if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        if (chunk != null && !chunk.TileIsLocked())
                        {
                            _chunkList.Add(chunk);
                            HighlightGridTile(chunk);
                            //chunk.EnableTileRenderingGameObject();
                            //chunk.EnableTileRendering();
                        }
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

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        
    }
    
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
}
