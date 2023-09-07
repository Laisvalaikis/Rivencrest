using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateEye : BaseAction
{
    [SerializeField] private GameObject eyePrefab;
    private bool isEyeActive = true;
    private CharacterVision _characterVision;
    private PlayerInformation _playerInformation;
    
    public override void ResolveAbility(Vector3 position)
    {
        
        for (int i = 0; i < _chunkList.Count; i++)
        {
            spawnedCharacter = Instantiate(eyePrefab, _chunkList[i].GetPosition() + new Vector3(0.015f, -0.8f, 0), Quaternion.identity);
        }
        base.ResolveAbility(position);
        //_characterVision.EnableGrid();
        //_playerInformation.VisionGameObject = eyePrefab;
        //isEyeActive = true;
        FinishAbility();
    }
    
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        
        (int y, int x) coordinates = GameTileMap.Tilemap.GetChunk(transform.position).GetIndexes();
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        _chunkList.Clear();
        
        int topY = coordinates.y - AttackRange;
        ChunkData chunkData = chunkDataArray[topY, coordinates.x];
        HighlightGridTile(chunkData);
        _chunkList.Add(chunkData);
        
        
    }
    
    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        CreateGrid(startChunk, AttackRange);
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
