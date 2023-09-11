using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPulse : BaseAction
{
    private ChunkData _currentChunk;
    void Start()
    {
        actionStateName = "SpearPulse";
    }

    public override void ResolveAbility(Vector3 position)
    {
        // if (CanTileBeClicked(position))
        // {
            for (int i = 0; i < _chunkList.Count; i++)
            {
                DealRandomDamageToTarget(_chunkList[i], minAttackDamage, maxAttackDamage);
            }
            base.ResolveAbility(position);
            FinishAbility();
        // }
        
    }
    
    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        if (_currentChunk != null && _currentChunk.CharacterIsOnTile())
        {
            CreateGrid(startChunk, AttackRange);
        }
    }
    
    public override void OnMoveArrows(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk != _currentChunk)
        {
            _currentChunk = hoveredChunk;
            if (_currentChunk != null && _currentChunk.CharacterIsOnTile())
            {
                ClearGrid();
                GenerateDiamondPattern(_currentChunk, AttackRange);
            }
        }
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
    }
    

    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile,minAttackDamage,maxAttackDamage);
    }

    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile,MergedTileList);
    }
}
