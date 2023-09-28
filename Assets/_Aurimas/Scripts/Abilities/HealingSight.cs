using UnityEngine;

public class HealingSight : BaseAction
{
    public int minHealAmount = 3;
    public int maxHealAmount = 7;
    private CharacterVision _characterVision;
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        int randomHeal = Random.Range(minHealAmount, maxHealAmount);
        bool crit = IsItCriticalStrike(ref randomHeal);
        chunk.GetCurrentPlayerInformation().Heal(randomHeal, crit);
        _characterVision.VisionRange = 6;
        FinishAbility();
        
    }
}
