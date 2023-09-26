using System.Collections.Generic;
using UnityEngine;

public class WallSmash : BaseAction
{
    [SerializeField] private int damageToWall = 1;
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        if (chunkData.GetCurrentCharacter() != null && chunkData.GetInformationType() == InformationType.Player)
        {
            DealRandomDamageToTarget(chunkData, minAttackDamage-2, maxAttackDamage-2);
        }
        else if(chunkData.GetCurrentCharacter() != null)
        {
            DestroyObject(chunkData);
        }
        FinishAbility();
    }
    private void DestroyObject(ChunkData chunkData)
    {
        (int x, int y) = chunkData.GetIndexes();
        var pushDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0,-1)
        };
        foreach (var position in pushDirectionVectors)
        {
            ChunkData targetChunkData =
                GameTileMap.Tilemap.GetChunkDataByIndex(position.Item1 + x, position.Item2 + y);
            if (targetChunkData != null && targetChunkData.GetCurrentPlayerInformation() == null)
            {
                ChunkData targetChunkDataPlayer =
                    GameTileMap.Tilemap.GetChunkDataByIndex(position.Item1 * 2 + x, position.Item2 * 2 + y);
                if (targetChunkDataPlayer != null && targetChunkDataPlayer.GetCurrentPlayerInformation() != null 
                                                  && targetChunkDataPlayer.GetInformationType() == InformationType.Player && !IsAllegianceSame(targetChunkDataPlayer.GetPosition()))
                {
                    DealRandomDamageToTarget(targetChunkDataPlayer, minAttackDamage, maxAttackDamage);
                }
            }
        }
        chunkData.GetCurrentPlayerInformation().DealDamage(damageToWall, false, gameObject);
    }
}
