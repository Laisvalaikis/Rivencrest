using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAttack : BaseAction
{
    private List<Poison> _poisons;
    // Start is called before the first frame update
    void Start()
    {
        actionStateName = "AcidRain";
    }

    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            foreach (ChunkData tile in ReturnGeneratedChunks())
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
        EnableDamagePreview(tile,minAttackDamage,maxAttackDamage);
    }

    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile,MergedTileList);
    }

}
