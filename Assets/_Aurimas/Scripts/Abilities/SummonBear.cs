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
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        (int y, int x) coordinates = GameTileMap.Tilemap.GetChunk(transform.position).GetIndexes();
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        _chunkList.Clear();
        int rightX = coordinates.x + AttackRange;
        ChunkData chunkData = chunkDataArray[coordinates.y, rightX];
        HighlightGridTile(chunkData);
        _chunkList.Add(chunkData);
    }
    
    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        CreateGrid(startChunk, AttackRange);
    }
}
