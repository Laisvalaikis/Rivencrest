using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : BaseAction
{

    private bool isAbilityActive = false;
    private List<ChunkData> _chunkListCopy;

    void Start()
    {
        actionStateName = "GroundSlam";
        isAbilitySlow = false;
    }
    
    public override void OnTurnStart()//pradzioj ejimo
    {
        base.OnTurnStart();
    }

    public override void CreateGrid()
    {
        base.CreateGrid();
        _chunkListCopy = new List<ChunkData>(_chunkList);
        Debug.Log("We createded it");
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
        if(_chunkListCopy.Count==0)
            Debug.Log("SAD");
        foreach (var chunk in _chunkListCopy)
        {
            chunk.GetTileHighlight().SetHighlightColor(Color.green);
            if (chunk.GetCurrentCharacter() != null && chunk!=GameTileMap.Tilemap.GetChunk(transform.position))
            {
                DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
                Debug.Log("Dealt damage");
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
                chunk.GetTileHighlight().SetHighlightColor(Color.green);
            }
        }
        else if ((hoveredChunkHighlight!=null && previousChunkHighlight!=null && hoveredChunkHighlight.isHighlighted && !previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight==null && hoveredChunkHighlight.isHighlighted))
        {
            foreach (var chunk in _chunkList)
            {
                chunk.GetTileHighlight().SetHighlightColor(Color.magenta);
            }
        }
    }
    
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
    }
    public override void EnableDamagePreview(GameObject tile, List<GameObject> tileList, int minAttackDamage, int maxAttackDamage = -1)
    {
        foreach(GameObject tileInList in tileList)
        {
            if(tileInList != GetSpecificGroundTile(gameObject, 0, 0, groundLayer))
            {
                EnableDamagePreview(tileInList, minAttackDamage, maxAttackDamage);
            }
            else
            {
                EnableTextPreview(tileInList, "");
            }
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }

}
