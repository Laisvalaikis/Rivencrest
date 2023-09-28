using System.Collections.Generic;
using UnityEngine;

public class StunAttack : BaseAction
{
    private List<Poison> _poisons;
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        PoisonPlayer();
    }

    private void PoisonPlayer()
    {
        foreach (Poison x in _poisons)
        {
            if (x.poisonValue > 0 && x.chunk.GetCurrentPlayerInformation().GetHealth() > 0)
            {
                DealDamage(x.chunk, x.poisonValue, false);
            }
            x.turnsLeft--;
        }
        
    }
}
