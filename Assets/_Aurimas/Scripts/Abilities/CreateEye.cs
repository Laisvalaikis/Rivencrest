using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateEye : BaseAction
{
    [SerializeField] private GameObject eyePrefab;
    private bool isEyeActive = true;
    private CharacterVision _characterVision;
    private PlayerInformation _playerInformation;

    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        (int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0)).GetIndexes();
        var directionVectors = new List<(int, int)>
        {
            (AttackRange, coordinates.y + 0),
            (coordinates.x + 0, AttackRange),
            (-AttackRange, coordinates.y + 0),
            (coordinates.x + 0, -AttackRange)
        };
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            if (isGround && (!isBlockingLayer || isPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    // public override void CreateGrid()
    // {
    //     this.AvailableTiles.Clear();
    //     this.AvailableTiles.Add(new List<GameObject>());
    //     AddSurroundingsToList(transform.gameObject, 0);
    //     
    // }
    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        AddSurroundingsToList(transform.gameObject,0);
        CreateGrid(startChunk, AttackRange);
        Debug.Log("Generates");
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        eyePrefab = Instantiate(eyePrefab, position + new Vector3(0f, 0f, 1f), Quaternion.identity);
        _characterVision.EnableGrid();
        _playerInformation.VisionGameObject = eyePrefab;
        isEyeActive = true;
        FinishAbility();
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
