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

    private int globalIndex = -1;
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        if (globalIndex != -1) // cia sita darom, nes buvo buggas kad jei hoverini nuo vienos row kampo prie kitos, highlightintos lieka abi eiles
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData chunkToHighLight = _chunkArray[globalIndex, i];
                chunkToHighLight?.GetTileHighlight().SetHighlightColor(Color.green);
            }
        }
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            
            int index = FindChunkIndex(hoveredChunk);
            globalIndex = index;
            if (index != -1)
            {
                Debug.Log(index);
                for (int i = 0; i < _chunkArray.GetLength(1); i++)
                {
                    ChunkData chunkToHighLight = _chunkArray[index, i];
                    chunkToHighLight?.GetTileHighlight().SetHighlightColor(Color.magenta);
                }
            }
        }
        else if (hoveredChunk == null || !hoveredChunk.GetTileHighlight().isHighlighted || FindChunkIndex(previousChunk)==FindChunkIndex(hoveredChunk))
        {
            int index = FindChunkIndex(previousChunk);
            if (index != -1)
            {
                for (int i = 0; i < _chunkArray.GetLength(1); i++)
                {
                    ChunkData chunkToHighLight = _chunkArray[index, i];
                    chunkToHighLight?.GetTileHighlight().SetHighlightColor(Color.green);
                }
            }
        }
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
