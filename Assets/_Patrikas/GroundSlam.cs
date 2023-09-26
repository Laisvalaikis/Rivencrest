using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : BaseAction
{
    private List<ChunkData> _chunkListCopy;

    void Start()
    {
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
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
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
    // public override void EnableDamagePreview(GameObject tile, List<GameObject> tileList, int minAttackDamage, int maxAttackDamage = -1)
    // {
    //     foreach(GameObject tileInList in tileList)
    //     {
    //         if(tileInList != GetSpecificGroundTile(gameObject, 0, 0, groundLayer))
    //         {
    //             EnableDamagePreview(tileInList, minAttackDamage, maxAttackDamage);
    //         }
    //         else
    //         {
    //             EnableTextPreview(tileInList, "");
    //         }
    //     }
    // }
}
