using UnityEngine;

public class HealSingle : BaseAction
{
    private PlayerInformation _playerInformation;
    public int minHealAmount = 3;
    public int maxHealAmount = 7;
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        int randomHeal = Random.Range(minHealAmount, maxHealAmount);
        bool crit = IsItCriticalStrike(ref randomHeal);
        _playerInformation.Heal(randomHeal,crit);
        FinishAbility();
    }
}
