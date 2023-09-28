using UnityEngine;

public class MindControl : BaseAction
{
    private PlayerInformation _playerInformation;
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        chunk.GetCurrentPlayerInformation().ApplyDebuff("MindControl", gameObject);
        FinishAbility();
    }
}
