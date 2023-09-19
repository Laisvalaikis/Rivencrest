using System.Collections.Generic;
using UnityEngine;

public class AcidRain : BaseAction
{
    private List<Poison> _poisons;
    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            foreach (ChunkData tile in _chunkList)
            {
                if (CanTileBeClicked(position))
                {
                    ChunkData target = GetSpecificGroundTile(tile.GetPosition());
                    _poisons.Add(new Poison(target, 2, 2));
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
