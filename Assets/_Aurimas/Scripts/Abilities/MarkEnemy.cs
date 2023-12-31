using UnityEngine;

public class MarkEnemy : BaseAction
{
    private ChunkData _target;
    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        if (_target != null && _target.GetCurrentPlayerInformation().Marker != null)
        {
            _target.GetCurrentPlayerInformation().Marker = null;
            _target = null;
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        chunk.GetCurrentPlayerInformation().Marker = gameObject;
        FinishAbility();
        
    }
}
