using UnityEngine;

public class MistShield : BaseAction
{
    private PlayerInformation _playerInformation;
    private bool isAbilityActive = false;
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        isAbilityActive = true;
        //_playerInformation.MistShield = true;
        _playerInformation.Protected = true;
        FinishAbility();
    }
}
