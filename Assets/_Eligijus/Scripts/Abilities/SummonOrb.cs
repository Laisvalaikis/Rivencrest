using System.Collections.Generic;
using UnityEngine;

public class SummonOrb : BaseAction
{
    [SerializeField] private GameObject orbPrefab;
    private PlayerInformation _orbInformation;
    private ChunkData _orbChunkData;
    private List<ChunkData> _attackList;
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        SpawnOrb(chunk);
        GenerateAttackGrid(chunk);
        FinishAbility();
    }

    private void SpawnOrb(ChunkData chunkData)
    {
        GameObject spawnedWall = Instantiate(orbPrefab, chunkData.GetPosition() - new Vector3(0f, 0.5f, 0f),
            Quaternion.identity);
        _orbInformation = spawnedWall.GetComponent<PlayerInformation>();
        _orbInformation.SetInformationType(InformationType.Object);
        chunkData.SetCurrentCharacter(spawnedWall, _orbInformation);
        chunkData.GetTileHighlight().ActivatePlayerTile(true);
        _orbChunkData = chunkData;
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (_orbChunkData != null && _orbInformation.GetHealth() > 0)
        {
            _orbChunkData.SetCurrentCharacter(null, null);
            _orbChunkData = null;
            Destroy(_orbInformation.gameObject);
            foreach (var t in _attackList)
            {
                int randomDamage = UnityEngine.Random.Range(minAttackDamage, maxAttackDamage);
                bool crit = IsItCriticalStrike(ref randomDamage);
                DealDamage(t, randomDamage, crit);
            }
            _attackList.Clear();
        }
    }

    public void GenerateAttackGrid(ChunkData centerChunk)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _attackList = new List<ChunkData>();
        int startRadius = 1;
        for (int range = 0; range < AttackRange; range++)
        {
            
            int count = startRadius + (range * 2);
            int topLeftCornerX = centerX - range;
            int topLeftCornerY = centerY - range;
            int bottomRightCornerX = centerX + range;
            int bottomRightCornerY = centerY + range;


            for (int i = 0; i < count; i++)
            {
                if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX + i, topLeftCornerY))
                {
                    ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX + i, topLeftCornerY);
                    _chunkList.Add(chunkData);
                    HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                }

                if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX - i, bottomRightCornerY))
                {
                    ChunkData chunkData =
                        GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX - i, bottomRightCornerY);
                    _chunkList.Add(chunkData);
                    HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                }

                if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX, topLeftCornerY + i))
                {
                    ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY + i);
                    _chunkList.Add(chunkData);
                    HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                }

                if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX, bottomRightCornerY - i))
                {
                    ChunkData chunkData =
                        GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY - i);
                    _chunkList.Add(chunkData);
                    HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                }
            }
        }
    }
}
