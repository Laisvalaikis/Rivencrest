using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : BaseAction
{
    // Start is called before the first frame update
    void Start()
    {
        actionStateName = "AcidRain";
    }

    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            foreach (ChunkData tile in ReturnGeneratedChunks())
            {
                if (CanTileBeClicked(position))
                {
                    ChunkData target=GetSpecificGroundTile(tile.GetPosition());
                    
                }
            }
            FinishAbility();
        }
        
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
