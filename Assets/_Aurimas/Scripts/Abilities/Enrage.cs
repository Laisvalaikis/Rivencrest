using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enrage : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        FinishAbility();
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "+1 MP");
        EnableTextPreview(GetSpecificGroundTile(gameObject, 0, 0, groundLayer), "+1 MP");
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile);
        DisablePreview(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
    }
}
