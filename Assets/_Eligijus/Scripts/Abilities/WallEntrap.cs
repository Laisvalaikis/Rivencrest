using System.Collections.Generic;
using UnityEngine;

public class WallEntrap : BaseAction
{
    [SerializeField] private GameObject wallPrefab;
    private List<PlayerInformation> _playerInformations;
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        SpawnAdjacentWalls();
        FinishAbility();
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (_playerInformations.Count > 0)
        {
            foreach (PlayerInformation x in _playerInformations)
            {
                x.DealDamage(1, false, gameObject);
            }
        }
    }

    private void SpawnAdjacentWalls()
    {
        (int x, int y) coordinates = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0)).GetIndexes();
        var directionVectors = new List<(int, int)>
        {
            (coordinates.x + 1, coordinates.y + 0),
            (coordinates.x + 0, coordinates.y + 1),
            (coordinates.x + (-1), coordinates.y + 0),
            (coordinates.x + 0, coordinates.y + (-1))
        };
        ChunkData[,] chunkDataArray = GameTileMap.Tilemap.GetChunksArray();
        foreach (var x in directionVectors)
        {
            if (x.Item1 >= 0 && x.Item1 < chunkDataArray.GetLength(0) && x.Item2 >= 0 && x.Item2 < chunkDataArray.GetLength(1))
            {
                ChunkData chunkData = chunkDataArray[x.Item1, x.Item2];
                GameObject spawnedWall = Instantiate(wallPrefab, chunkData.GetPosition() - new Vector3(0f, 0.5f, 0f),
                    Quaternion.identity);
                PlayerInformation tempPlayerInformation = spawnedWall.GetComponent<PlayerInformation>();
                GameTileMap.Tilemap.SetCharacter(chunkData.GetPosition(), spawnedWall, tempPlayerInformation);
                _playerInformations.Add(tempPlayerInformation);
            }
        }
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
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
