using UnityEngine;

public class Entangle : BaseAction
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        DealRandomDamageToTarget(chunkData,minAttackDamage,maxAttackDamage);
        chunkData.GetCurrentPlayerInformation().ApplyDebuff("CantMove");
        FinishAbility();
    }
}
