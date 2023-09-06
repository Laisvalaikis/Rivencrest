using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateEye : BaseAction
{
    [SerializeField] private GameObject eyePrefab;
    private bool isEyeActive = true;
    private CharacterVision _characterVision;
    private PlayerInformation _playerInformation;
    //private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        spawnedCharacter = Instantiate(eyePrefab, position + new Vector3(0f,-0.8f,1f) , Quaternion.identity);
        //_characterVision.EnableGrid();
        //_playerInformation.VisionGameObject = eyePrefab;
        //isEyeActive = true;
        FinishAbility();
    }
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        (int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0)).GetIndexes();
        var directionVectors = new List<(int, int)>
        {
            (coordinates.x + 1, AttackRange + 1),
            (coordinates.x + 0, AttackRange + 1),
            (coordinates.x + (-1), AttackRange + 1),
            (coordinates.x + 0, AttackRange + (-1))
        };
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        foreach (var x in directionVectors)
        {
            if (x.Item1 >= 0 && x.Item1 < chunkDataArray.GetLength(0) && AttackRange >= 0 && AttackRange < chunkDataArray.GetLength(1))
            {
                ChunkData chunkData = chunkDataArray[x.Item1, AttackRange];
            }
        }
        
        
        
    }
    // public override void CreateGrid()
    // {
    //     ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
    //     CreateGrid(startChunk, AttackRange);
    //     HighlightAll();
    //     Debug.Log("Generates");
    // }

    
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
}
