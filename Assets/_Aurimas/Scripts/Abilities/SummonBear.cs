using UnityEngine;

public class SummonBear : BaseAction
{
    [SerializeField] private GameObject bearPrefab;

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        spawnedCharacter = Instantiate(bearPrefab, position + new Vector3(0f,-0.5f), Quaternion.identity);
        FinishAbility();
    }

    public override void CreateAvailableChunkList(int attackRange)
    {
        (int y, int x) coordinates = GameTileMap.Tilemap.GetChunk(transform.position).GetIndexes();
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        _chunkList.Clear();
        int rightX = coordinates.x + AttackRange;
        ChunkData chunkData = chunkDataArray[coordinates.y, rightX];
        _chunkList.Add(chunkData);
    }
}
