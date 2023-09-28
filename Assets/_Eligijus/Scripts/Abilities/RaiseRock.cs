using UnityEngine;

public class RaiseRock : BaseAction
{
    public GameObject WallPrefab;
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        GameObject spawnedWall = Instantiate(WallPrefab, chunk.GetPosition() + new Vector3(0f, -0.5f, 0),
            Quaternion.identity);
        PlayerInformation tempPlayerInformation = spawnedWall.GetComponent<PlayerInformation>();
        GameTileMap.Tilemap.SetCharacter(chunk, spawnedWall, tempPlayerInformation);
        FinishAbility();
    }
}
