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
        for (int i = 0; i < _chunkArray.GetLength(1); i++)
        {
            ChunkData damageChunk = _chunkArray[index, i];
            DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
        }
        
        FinishAbility();
    }

    private int FindChunkIndex(ChunkData chunkData)
    {
        int index = -1;
        for (int i = 0; i < _chunkArray.GetLength(1); i++)
        {
            if (_chunkArray[0,i] == chunkData)
            {
                index = 0;
                break;
            }
            
            if(_chunkArray[1,i] == chunkData)
            {
                index = 1;
                break;
            }
            
            if (_chunkArray[2,i] == chunkData)
            {
                index = 2;
                break;
            }
            
            if (_chunkArray[3,i] == chunkData)
            {
                index = 3;
                break;
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
        int count = startRadius + (AttackRange * 2) - 2;
        int topLeftCornerY = centerY + AttackRange;
        int topLeftCornerX = centerX - AttackRange;
        int bottomRightCornerY = centerY - AttackRange;
        int bottomRightCornerX = centerX + AttackRange;

        _topleftCornerindex = new Vector2(topLeftCornerX, topLeftCornerY);
        _rightBottomCornerIndex = new Vector2(bottomRightCornerX, bottomRightCornerY);
        _chunkArray = new ChunkData[4,count];
        
        
        for (int i = 0; i < count; i++)
        {
            if (GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX+i, topLeftCornerY) != null)
            {
                _chunkList.Add(GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX+i, topLeftCornerY));
                _chunkArray[0, i] = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX + i, topLeftCornerY);
            }
            else if (GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX-i, bottomRightCornerY) != null)
            {
                _chunkList.Add(GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX-i, bottomRightCornerY));
                _chunkArray[1, i] = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX-i, bottomRightCornerY);
            }
            else if (GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY+i) != null)
            {
                _chunkList.Add(GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY+i));
                _chunkArray[2, i] = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY + i);
            }
            else if (GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY-i) != null)
            {
                _chunkList.Add(GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY-i));
                _chunkArray[3, i] = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY - i);
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
