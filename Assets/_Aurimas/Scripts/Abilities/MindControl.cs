using UnityEngine;

public class MindControl : BaseAction
{
    private PlayerInformation _playerInformation;
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        chunkData.GetCurrentPlayerInformation().ApplyDebuff("MindControl", gameObject);
        FinishAbility();
    }
}
