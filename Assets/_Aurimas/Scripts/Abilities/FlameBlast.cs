using UnityEngine;

public class FlameBlast : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") && (IsAllegianceSame(position) || friendlyFire))
        {
            ChunkData chunkData = GetSpecificGroundTile(position);
            DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        }
        FinishAbility();
    }
}
