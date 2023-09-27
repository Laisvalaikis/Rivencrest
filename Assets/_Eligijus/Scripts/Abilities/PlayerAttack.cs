using UnityEngine;

public class PlayerAttack : BaseAction
{

    private void Start()
    {
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

    public override bool CanTileBeClicked(Vector3 position)
    {
        ChunkData chunk = GetSpecificGroundTile(position);
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
