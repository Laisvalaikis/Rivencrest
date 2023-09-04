using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEye : BaseAction
{
    [SerializeField] private GameObject eyePrefab;
    private bool isEyeActive = false;
    private CharacterVision _characterVision;
    private GameObject _createdEye;
    private PlayerInformation _playerInformation;

    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        (int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0)).GetIndexes();
        var DirectionVectors = new List<(int, int)>
        {
            (coordinates.x + 1, coordinates.y + 0),
            (coordinates.x + 0, coordinates.y + 1),
            (coordinates.x + (-1), coordinates.y + 0),
            (coordinates.x + 0, coordinates.y + (-1))
        };
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        foreach (var x in DirectionVectors)
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
    public override void CreateGrid()
    {
        this.AvailableTiles.Clear();
        this.AvailableTiles.Add(new List<GameObject>());
        AddSurroundingsToList(transform.gameObject, 0);
        
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        _createdEye = Instantiate(_createdEye, position + new Vector3(0, 0, 1), Quaternion.identity);
        _characterVision.EnableGrid();
        _playerInformation.VisionGameObject = _createdEye;
        isEyeActive = true;
        FinishAbility();
    }
}
