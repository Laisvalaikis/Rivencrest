using Unity.Mathematics;
using UnityEngine;

public class Scream : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        ChunkData current = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0));
        Side side = ChunkSideByCharacter(current, chunkData);
        int2 sideVector = GetSideVector(side);
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        MovePlayerToSide(current, -sideVector);
        FinishAbility();
    }
}
