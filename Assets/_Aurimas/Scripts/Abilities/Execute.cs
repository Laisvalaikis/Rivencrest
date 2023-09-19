using UnityEngine;

public class Execute : BaseAction
{
    public int minimumDamage = 4;
    private PlayerInformationData _playerInformationData;
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GetSpecificGroundTile(position);
        int damage = ExecuteDamage(position);
        chunkData.GetCurrentPlayerInformation().DealDamage(damage, false, gameObject);
        if(chunkData.GetCurrentPlayerInformation().health <= 0)
        {
            GetComponent<PlayerInformation>().Heal(5, false);
        }
        FinishAbility();
    }
    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == radius)
                {
                    int targetX = centerX + x;
                    int targetY = centerY + y;
                    
                    if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        if (chunk != null && !chunk.TileIsLocked())
                        {
                            _chunkList.Add(chunk);
                            HighlightGridTile(chunk);
                        }
                    }
                }
            }
        }
    }

    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        CreateGrid(startChunk, AttackRange);
    }
    private int ExecuteDamage(Vector3 target) 
    {
        int damage = minimumDamage + Mathf.FloorToInt(float.Parse((
            (_playerInformationData.MaxHealth) * 0.15
        ).ToString()));
    
        return damage;
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
