using UnityEngine;

public class PlayerAttack : BaseAction
{

    private void Start()
    {
        AttackAbility = true;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            Debug.LogError("FIX FINISH ABILITY");
            FinishAbility();
        }
    }

    public override bool CanTileBeClicked(ChunkData chunk)
    {
        if (((CheckIfSpecificInformationType(chunk, InformationType.Player) || CheckIfSpecificInformationType(chunk, InformationType.Object))
            && IsAllegianceSame(chunk) && !GetComponent<PlayerInformation>().CantAttackCondition) || friendlyFire)
        {
            return true;
        }
        return false;
    }
    
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Sharp blade"))
        {
            minAttackDamage += 2;
            maxAttackDamage += 2;
        }
        isAbilitySlow = !DoesCharacterHaveBlessing("Agility");
    }
    
}
