using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : BaseAction
{
    private List<ChunkData> _chunkListCopy;

    void Start()
    {
        minAttackDamage = 1;
        maxAttackDamage = 3;
        isAbilitySlow = false;
        AttackHighlight = new Color32(123,156, 178,255);
        AttackHighlightHover = AttackHoverCharacter;
        CharacterOnGrid = AttackHighlight;
    }
    public override void CreateGrid()
    {
        base.CreateGrid();
        _chunkListCopy = new List<ChunkData>(_chunkList);
    }
    protected override void HighlightGridTile(ChunkData chunkData)
    {
        SetNonHoveredAttackColor(chunkData);
        chunkData.GetTileHighlight().ActivateColorGridTile(true);
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");
        DealDamageToAdjacent();
        FinishAbility();
    }
    private void DealDamageToAdjacent()
    {
        foreach (var chunk in _chunkListCopy)
        {
            SetNonHoveredAttackColor(chunk);
            if (chunk.GetCurrentCharacter() != null && chunk!=GameTileMap.Tilemap.GetChunk(transform.position))
            {
                DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            }
        }
    }
    public override bool CanTileBeClicked(ChunkData chunk)
    {
        return chunk.GetCurrentCharacter() != GameTileMap.Tilemap.GetCurrentCharacter(); //Prety sure ground slamas temamateus hittina?
    }
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        if ((hoveredChunkHighlight == null && previousChunkHighlight!=null && previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight!=null && !hoveredChunkHighlight.isHighlighted && previousChunkHighlight.isHighlighted))
        {
            foreach (var chunk in _chunkList)
            {
                SetNonHoveredAttackColor(chunk);
                DisableDamagePreview(chunk);
            }
        }
        else if ((hoveredChunkHighlight!=null && previousChunkHighlight!=null && hoveredChunkHighlight.isHighlighted && !previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight==null && hoveredChunkHighlight.isHighlighted))
        {
            foreach (var chunk in _chunkList)
            {
                SetHoveredAttackColor(chunk);
            }
        }
    }
}
