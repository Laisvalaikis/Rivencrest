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
      
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        _cometTiles.Clear();
        List<ChunkData> damageTiles = CreateDamageTileList(position);
        foreach (ChunkData tile in damageTiles)
        {
            if (tile.IsStandingOnChunk() && !IsAllegianceSame(tile.GetPosition()))
            {
                _cometTiles.Add(tile);
            }
        }
        FinishAbility();
    }
    
    public List<ChunkData> CreateDamageTileList(Vector3 position)
    {
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
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
