using System.Collections.Generic;
using UnityEngine;

public class AcidRain : BaseAction
{
    private List<Poison> _poisons;
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            foreach (ChunkData tile in _chunkList)
            {
                if (CanTileBeClicked(tile))
                {
                    _poisons.Add(new Poison(tile, 2, 2));
                }
            }
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
        foreach (Poison poison in _poisons)
        {
            if (poison.poisonValue > 0 && poison.chunk.GetCurrentPlayerInformation().GetHealth() > 0)
            {
                DealDamage(poison.chunk, poison.poisonValue, false);
            }
            poison.turnsLeft--;
        }
    }
}
