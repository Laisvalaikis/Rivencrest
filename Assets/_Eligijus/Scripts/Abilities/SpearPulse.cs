using UnityEngine;

public class SpearPulse : BaseAction
{
    private ChunkData _currentChunk;

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            for (int i = 0; i < _chunkList.Count; i++)
            {
                DealRandomDamageToTarget(_chunkList[i], minAttackDamage, maxAttackDamage);
            }
            base.ResolveAbility(chunk);
            FinishAbility();
        }
    }
    
    public override void CreateGrid()
    {
        if (_currentChunk != null && _currentChunk.CharacterIsOnTile())
        {
            CreateAvailableChunkList(AttackRange);
            HighlightAllGridTiles();
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
}
