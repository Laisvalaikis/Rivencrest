using Unity.Mathematics;
using UnityEngine;

public class ShadowBlink : BaseAction
{
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        ChunkData current = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0));
        Side _side = ChunkSideByCharacter(current, chunk);
        int2 sideVector = GetSideVector(_side);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        MovePlayerToSide(current, sideVector, chunk);
        FinishAbility();
    }
    
}
