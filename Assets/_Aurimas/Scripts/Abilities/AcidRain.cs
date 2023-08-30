using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : BaseAction
{
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
                    target.GetCurrentPlayerInformation().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
                }
            }
            FinishAbility();
        }
        
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        Poison();
    }

    private void Poison()
    {
        int poisonDamage = 0;

        // foreach (Poison x in Poisons)
        // {
        //     x.turnsLeft--;
        // }
        // poisonDamage = TotalPoisonDamage();
        // Poisons.RemoveAll(x => x.turnsLeft <= 0);
        // if (poisonDamage > 0 && health > 0)
        // {
        //     if (BlessingsAndCurses.Find(x => x.blessingName == "Antitoxic") != null)
        //     {
        //         poisonDamage = 0;
        //     }
        //     DealDamage(poisonDamage, false, gameObject, "Poison");
        // }
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
