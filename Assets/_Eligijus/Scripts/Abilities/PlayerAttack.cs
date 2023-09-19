using UnityEngine;

public class PlayerAttack : BaseAction
{

    private void Start()
    {
        actionStateName = "Attack";
        AttackAbility = true;
    }

    public override void ResolveAbility(Vector3 position)
    {
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
            DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
            Debug.LogError("FIX FINISH ABILITY");
            FinishAbility();
        }
    }

    protected override bool CanTileBeClicked(Vector3 position)
    {
        if ((CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(position, 0, 0, blockingLayer, "Wall"))
            && IsAllegianceSame(position) && !GetComponent<PlayerInformation>().CantAttackCondition)
        {
            return true;
        }
        return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
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
