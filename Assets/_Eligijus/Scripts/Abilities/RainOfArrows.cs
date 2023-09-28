using System.Collections.Generic;
using UnityEngine;

public class RainOfArrows : BaseAction
{
    private List<ChunkData> _cometTiles;
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (_cometTiles.Count > 0)
        {
            foreach (ChunkData tile in _cometTiles)
            {
                DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
                //tile.GetComponent<HighlightTile>().DangerUI.SetActive(false);
            }
            _cometTiles.Clear();
        }
    }
      
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        _cometTiles.Clear();
        List<ChunkData> damageChunks = CreateDamageTileList(chunk);
        foreach (ChunkData chunkData in damageChunks)
        {
            if (chunkData.IsStandingOnChunk() && !IsAllegianceSame(chunkData))
            {
                _cometTiles.Add(chunkData);
            }
        }
        FinishAbility();
    }
    
    public List<ChunkData> CreateDamageTileList(ChunkData chunk)
    {
        (int x, int y) = chunk.GetIndexes();
        List<ChunkData> damageTiles = new List<ChunkData>();
        var spellDirectionVectors = new List<(int, int)>
        {
            (0, 0),
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        foreach (var direction in spellDirectionVectors)
        {
            if (GameTileMap.Tilemap.CheckBounds(direction.Item1, direction.Item2))
            {
                ChunkData temp = GameTileMap.Tilemap.GetChunkDataByIndex(direction.Item1, direction.Item2);
                damageTiles.Add(temp);
            }
        }
        return damageTiles;
    }
    
}
