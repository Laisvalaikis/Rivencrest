using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilenceBeam : BaseAction
{
    private ChunkData[,] _chunkArray;
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        int index = FindChunkIndex(chunkData);
        if (index != -1)
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData damageChunk = _chunkArray[index, i];
                DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
            }

            FinishAbility();
        }
    }
    
    private int FindChunkIndex(ChunkData chunkData)
    {
        int index = -1;
        for (int i = 0; i < _chunkArray.GetLength(1); i++)
        {
            if (_chunkArray[0,i] != null)
            {
                if (_chunkArray[0,i] == chunkData)
                {
                    index = 0;
                    
                }
            }
            
            if(_chunkArray[1,i] != null)
            {
                if (_chunkArray[1,i] == chunkData)
                {
                    index = 1;
                    
                }
            }
            
            if (_chunkArray[2,i] != null)
            {
                if (_chunkArray[2,i] == chunkData)
                {
                    index = 2;
                    
                }
            }

            if (_chunkArray[3,i] != null)
            {
                if (_chunkArray[3,i] == chunkData)
                {
                    index = 3;
                    
                }
            }

        }
        return index;
    }
    
    
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        
        //Merging into one list
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        int count = AttackRange; // -2
        
        _chunkArray = new ChunkData[4,count];

        int start = 1;
        for (int i = 0; i < count; i++) 
        {
            if (GameTileMap.Tilemap.CheckBounds(centerX + i + start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX + i + start, centerY);
                _chunkList.Add(chunkData);
                HighlightGridTile(chunkData);
                _chunkArray[0, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX - i - start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX-i - start, centerY);
                _chunkList.Add(chunkData);
                HighlightGridTile(chunkData);
                _chunkArray[1, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY + i + start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY + i + start);
                _chunkList.Add(chunkData);
                HighlightGridTile(chunkData);
                _chunkArray[2, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY - i - start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY - i - start);
                _chunkList.Add(chunkData);
                HighlightGridTile(chunkData);
                _chunkArray[3, i] = chunkData;
            }
        }

    }
    
    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        CreateGrid(startChunk, AttackRange);
    }
    
    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
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
