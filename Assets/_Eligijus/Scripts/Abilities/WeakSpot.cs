using UnityEngine;

public class WeakSpot : BaseAction
{
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();
    }
}
