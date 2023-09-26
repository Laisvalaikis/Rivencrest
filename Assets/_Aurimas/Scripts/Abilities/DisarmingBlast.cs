using UnityEngine;

public class DisarmingBlast : BaseAction   //jei cia tik A.I ability gal ir nereikia to OnTileHover aurio pamastymai
{
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        FinishAbility();
    }
}
