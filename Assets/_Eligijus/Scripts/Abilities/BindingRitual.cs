using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingRitual : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            
            foreach (ChunkData tile in ReturnGeneratedChunks())
            {
                GameObject target = tile.GetCurrentCharacter();
                DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            }
            
            FinishAbility();
        }
        
    }
    
    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        
        //Merging into one list
        _chunkList.Clear();
        
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!MergedTileList.Contains(tile))
                {
                    MergedTileList.Add(tile);
                }
            }
        }
        //RemoveInner
        for (int i = this.AvailableTiles.Count - 1; i > 0; i--)
        {
            foreach (GameObject tile in this.AvailableTiles[i - 1])
            {
                MergedTileList.Remove(tile);
            }
        }
        if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
        {
            MergedTileList.Remove(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
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
