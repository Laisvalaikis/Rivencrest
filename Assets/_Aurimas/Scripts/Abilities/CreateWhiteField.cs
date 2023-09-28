using System.Collections.Generic;
using UnityEngine;

public class CreateWhiteField : BaseAction
{
    [SerializeField] private GameObject whiteFieldPrefab;
    
    public override void ResolveAbility(ChunkData chunk)
    {
        for (int i = 0; i < _chunkList.Count; i++)
        {
            spawnedCharacter = Instantiate(whiteFieldPrefab, _chunkList[i].GetPosition() + new Vector3(0.015f, -0.5f, 0), Quaternion.identity);
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    // public override void OnTileHover(GameObject tile)
    // {
    //     foreach (List<GameObject> MovementTileList in this.AvailableTiles)
    //     {
    //         EnableTextPreview(tile, MovementTileList, "");
    //     }
    // }
    // public override void OffTileHover(GameObject tile)
    // {
    //     foreach (List<GameObject> MovementTileList in this.AvailableTiles)
    //     {
    //         DisablePreview(tile, MovementTileList);
    //     }
    // }
}
