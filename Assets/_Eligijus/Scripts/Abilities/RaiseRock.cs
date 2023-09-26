using UnityEngine;

public class RaiseRock : BaseAction
{
    public GameObject WallPrefab;
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        GameObject spawnedWall = Instantiate(WallPrefab, chunkData.GetPosition() + new Vector3(0f, -0.5f, 0),
            Quaternion.identity);
        PlayerInformation tempPlayerInformation = spawnedWall.GetComponent<PlayerInformation>();
        GameTileMap.Tilemap.SetCharacter(chunkData.GetPosition(), spawnedWall, tempPlayerInformation);
        FinishAbility();
    }
    
}
