using UnityEngine;

public class HealingSight : BaseAction
{
    public int minHealAmount = 3;
    public int maxHealAmount = 7;
    private CharacterVision _characterVision;
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        int randomHeal = Random.Range(minHealAmount, maxHealAmount);
        bool crit = IsItCriticalStrike(ref randomHeal);
        GetSpecificGroundTile(position).GetCurrentPlayerInformation().Heal(randomHeal, crit);
        _characterVision.VisionRange = 6;
        FinishAbility();
        
    }

    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
}
