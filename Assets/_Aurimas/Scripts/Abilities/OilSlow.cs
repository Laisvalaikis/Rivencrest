using UnityEngine;

public class OilSlow : BaseAction
{
    private GameObject SlowedTarget = null;
    
    public override void OnTurnStart()
    {
        if (SlowedTarget != null)
        {
            SlowedTarget = null;
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        chunk.GetCurrentPlayerInformation().ApplyDebuff("OilSlow");
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();

        
    }
}
