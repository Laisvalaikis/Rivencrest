using UnityEngine;


public class CreateEye : BaseAction
{
    [SerializeField] private GameObject eyePrefab;
    private bool isEyeActive = true;
    private CharacterVision _characterVision;
    private PlayerInformation _playerInformation;
    public override void ResolveAbility(ChunkData chunk)
    {
       foreach (var t in _chunkList)
       {
           spawnedCharacter = Instantiate(eyePrefab, t.GetPosition() + new Vector3(0.015f, -0.8f, 0), Quaternion.identity);
       }
       base.ResolveAbility(chunk);
       //_characterVision.EnableGrid();
       //_playerInformation.VisionGameObject = eyePrefab;
       //isEyeActive = true;
       FinishAbility();
    }

    public override void CreateAvailableChunkList(int attackRange)
    {
        (int y, int x) coordinates = GameTileMap.Tilemap.GetChunk(transform.position).GetIndexes();
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        _chunkList.Clear();
        
        int topY = coordinates.y - AttackRange;
        ChunkData chunkData = chunkDataArray[topY, coordinates.x];
        _chunkList.Add(chunkData);
    }
}
