using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SwordPush : BaseAction
{
    public int pushDamage = 15;
    public int centerDamage = 30;
    private List<ChunkData> _attackTiles;
    
    public override void ResolveAbility(Vector3 position)
    {
        // if (CanTileBeClicked(position))
        // {
        base.ResolveAbility(position);
        ChunkData current = GameTileMap.Tilemap.GetChunk(position);
        Side _side = Side.none;
        CreateAttackGrid(current);
        for (int i = 0; i < _attackTiles.Count; i++)
        {
            if (i > 0)
            {
                int damage = pushDamage;
                bool crit = IsItCriticalStrike(ref damage);
                DealDamage(_attackTiles[i], damage, crit);
                _side = ChunkSideByCharacter(current, _attackTiles[i]);
                int2 sideVector = GetSideVector(_side);
                Debug.Log(sideVector);
                MovePlayerToSide(_attackTiles[i], sideVector);
                
            }
            else
            {
                int damage = centerDamage;
                bool crit = IsItCriticalStrike(ref damage);
                DealDamage(_attackTiles[i], damage, crit);
            }
        }
        FinishAbility();
        // }
    }

    private void CreateAttackGrid(ChunkData selected)
    {
        (int x, int y) tileIndex = selected.GetIndexes();
        List<(int, int)> positionIndexes = new List<(int, int)> 
        {
            (tileIndex.x, tileIndex.y + 1),  // Up
            (tileIndex.x, tileIndex.y - 1),  // Down
            (tileIndex.x + 1, tileIndex.y),  // Right
            (tileIndex.x - 1, tileIndex.y)   // Left
        };
        if (_attackTiles == null)
        {
            _attackTiles = new List<ChunkData>();
        }
        else
        {
            _attackTiles.Clear();
        }
        _attackTiles.Add(selected); // added center chunk;
        foreach (var indexes in positionIndexes)
        {
            if (GameTileMap.Tilemap.CheckBounds(indexes.Item1, indexes.Item2))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(indexes.Item1, indexes.Item2);
                if (chunkData.CharacterIsOnTile())
                {
                    Debug.Log("SMTH");
                    _attackTiles.Add(chunkData);
                }
            }
        }
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
