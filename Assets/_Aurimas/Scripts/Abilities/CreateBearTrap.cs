using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBearTrap : BaseAction
{
    //reikia bearTrap sukurti kaip ir praeitame projekte
    [SerializeField] private GameObject bearTrapPrefab;
    private int i = 0;

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        FinishAbility();
        spawnedCharacter = Instantiate(bearTrapPrefab, position, Quaternion.identity);
        i = 0;

    }

    public override void OnTurnStart()
    {
        if (spawnedCharacter != null)
        {
            i++;
            if (i >= 2)
            {
                Destroy(spawnedCharacter);
                i = 0;
            }
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
}
