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
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        GetSpecificGroundTile(position).GetCurrentPlayerInformation().ApplyDebuff("OilSlow");
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        FinishAbility();

        
    }
}
