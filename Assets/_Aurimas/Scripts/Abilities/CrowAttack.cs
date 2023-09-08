using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CrowAttack : BaseAction
{
    private List<Poison> _poisons;
    private int poisonBonusDamage=2;
    public override void ResolveAbility(Vector3 position)
    {
        for (int i = 0; i < _chunkList.Count; i++)
        {
            //ChunkData target = GetSpecificGroundTile(_chunkList[i].GetPosition());
            if (_chunkList[i].CharacterIsOnTile())
            {
                int bonusDamage = 0;
                if (_poisons.Count > 0)
                {
                    bonusDamage += poisonBonusDamage;
                }
                DealRandomDamageToTarget(_chunkList[i],minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
        }
        base.ResolveAbility(position);
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
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
}
