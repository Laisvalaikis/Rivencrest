using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley : BaseAction
{
    [SerializeField] private int spellDamage = 6;
    private Vector2 _topleftCornerindex;
    private Vector2 _rightBottomCornerIndex;
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
                // Debug.Log("Array indexes 0: " + _chunkArray[0,i].GetIndexes() + " VS " + chunkData.GetIndexes());
                if (_chunkArray[0,i] == chunkData)
                {
                    index = 0;
                    
                }
            }
            
            if(_chunkArray[1,i] != null)
            {
                // Debug.Log("Array indexes 1: " + _chunkArray[1,i].GetIndexes() + " VS " + chunkData.GetIndexes());
                if (_chunkArray[1,i] == chunkData)
                {
                    index = 1;
                    
                }
            }
            
            if (_chunkArray[2,i] != null)
            {
                // Debug.Log("Array indexes 2: " + _chunkArray[2,i].GetIndexes() + " VS " + chunkData.GetIndexes());
                if (_chunkArray[2,i] == chunkData)
                {
                    index = 2;
                    
                }
            }

            if (_chunkArray[3,i] != null)
            {
                // Debug.Log("Array indexes 3: " + _chunkArray[3,i].GetIndexes() + " VS " + chunkData.GetIndexes());
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

        int startRadius = 1;
        int count = startRadius + (AttackRange * 2)-2; // -2
        int topLeftCornerX = centerX - AttackRange;
        int topLeftCornerY = centerY - AttackRange;
        int bottomRightCornerX = centerX + AttackRange;
        int bottomRightCornerY = centerY + AttackRange;
        

        _topleftCornerindex = new Vector2(topLeftCornerX, topLeftCornerY);
        _rightBottomCornerIndex = new Vector2(bottomRightCornerX, bottomRightCornerY);
        _chunkArray = new ChunkData[4,count];

        int rowStart = 1; // start is 1, because we need ignore corner tile
        for (int i = 0; i < count; i++) 
        {
            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX + i + rowStart, topLeftCornerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX + i + rowStart, topLeftCornerY);
                _chunkList.Add(chunkData);
                HighlightGridTile(chunkData);
                _chunkArray[0, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX - i - rowStart, bottomRightCornerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX-i - rowStart, bottomRightCornerY);
                _chunkList.Add(chunkData);
                HighlightGridTile(chunkData);
                _chunkArray[1, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX, topLeftCornerY + i + rowStart))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY + i + rowStart);
                _chunkList.Add(chunkData);
                HighlightGridTile(chunkData);
                _chunkArray[2, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX, bottomRightCornerY - i - rowStart))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY - i - rowStart);
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
