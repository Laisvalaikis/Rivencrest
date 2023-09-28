using UnityEngine;

public class FlameKick : BaseAction
{
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        Vector3 pushDirection = chunk.GetPosition() - gameObject.transform.position;
        DealRandomDamageToTarget(chunk,minAttackDamage,maxAttackDamage);
        //chunkData.GetPosition() += pushDirection;
        FinishAbility();
    }
}
