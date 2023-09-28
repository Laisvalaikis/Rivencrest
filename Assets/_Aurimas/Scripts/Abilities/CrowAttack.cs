using System.Collections.Generic;
using UnityEngine;

public class CrowAttack : BaseAction
{
    private List<Poison> _poisons;
    private int poisonBonusDamage=2;
    public override void ResolveAbility(ChunkData chunk)
    {
        foreach (var t in _chunkList)
        {
            //ChunkData target = GetSpecificGroundTile(_chunkList[i].GetPosition());
            if (t.CharacterIsOnTile())
            {
                int bonusDamage = 0;
                if (_poisons.Count > 0)
                {
                    bonusDamage += poisonBonusDamage;
                }
                DealRandomDamageToTarget(t,minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
        }
        base.ResolveAbility(chunk);
        FinishAbility();
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
