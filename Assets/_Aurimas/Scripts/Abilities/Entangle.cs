using UnityEngine;

public class Entangle : BaseAction
{
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk,minAttackDamage,maxAttackDamage);
        chunk.GetCurrentPlayerInformation().ApplyDebuff("CantMove");
        FinishAbility();
    }
}
