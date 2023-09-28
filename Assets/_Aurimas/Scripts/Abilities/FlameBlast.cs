using UnityEngine;

public class FlameBlast : BaseAction
{
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        if (CheckIfSpecificInformationType(chunk, InformationType.Player) && (IsAllegianceSame(chunk) || friendlyFire))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        FinishAbility();
    }
}
