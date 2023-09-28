using UnityEngine;

public class DisarmingBlast : BaseAction   //jei cia tik A.I ability gal ir nereikia to OnTileHover aurio pamastymai
{
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        FinishAbility();
    }
}
