using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley : BaseAction
{
    [SerializeField] private int spellDamage = 6;
    private ChunkData[,] _chunkArray;
    private List<Poison> _poisons;
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
                _poisons.Add(new Poison(damageChunk, 2, 1));
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
    
    private int globalIndex = -1;
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        if (globalIndex != -1)
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData chunkToHighLight = _chunkArray[globalIndex, i];
                chunkToHighLight?.GetTileHighlight().SetHighlightColor(Color.green);
            }
        }
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            
            globalIndex = FindChunkIndex(hoveredChunk);
            if (globalIndex != -1)
            {
                for (int i = 0; i < _chunkArray.GetLength(1); i++)
                {
                    ChunkData chunkToHighLight = _chunkArray[globalIndex, i];
                    chunkToHighLight?.GetTileHighlight().SetHighlightColor(Color.magenta);
                }
            }
        }
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
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        PoisonPlayer();
    }

    private void PoisonPlayer()
    {
        foreach (Poison x in _poisons)
        {
            if (x.poisonValue > 0 && x.chunk.GetCurrentPlayerInformation().GetHealth() > 0)
            {
                DealDamage(x.chunk, x.poisonValue, false);
            }
            x.turnsLeft--;
        }
        
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
