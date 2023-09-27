using UnityEngine;

public class FlameBlast : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunk = GetSpecificGroundTile(position);
        if (CheckIfSpecificInformationType(chunk, InformationType.Player) && (IsAllegianceSame(chunk) || friendlyFire))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        FinishAbility();
    }
}
