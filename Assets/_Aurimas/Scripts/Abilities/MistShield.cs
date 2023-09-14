using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistShield : BaseAction
{
    private PlayerInformation _playerInformation;
    private bool isAbilityActive = false;
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        isAbilityActive = true;
        //_playerInformation.MistShield = true;
        _playerInformation.Protected = true;
        FinishAbility();
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
