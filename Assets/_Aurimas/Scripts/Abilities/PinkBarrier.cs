using UnityEngine;

public class PinkBarrier : BaseAction
{
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        chunk.GetCurrentPlayerInformation().BarrierProvider = gameObject;
       // GetSpecificGroundTile(position, 0, 0, blockingLayer).GetComponent<GridMovement>().AvailableMovementPoints++;
        FinishAbility();
    }
}
