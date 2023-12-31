using UnityEngine;

public class ReadyAimFire : BaseAction
{
    private ChunkData[,] _chunkArray;
    private int _index;
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        _index = FindChunkIndex(chunk);
        FinishAbility();
    }
     private int FindChunkIndex(ChunkData chunkData)
    {
        int index = -1;
        for (int i = 0; i < _chunkArray.GetLength(1); i++)
        {
            if (_chunkArray[0,i] != null && _chunkArray[0,i] == chunkData)
            {
                index = 0;
                    
            }
            
            if(_chunkArray[1,i] != null && _chunkArray[1,i] == chunkData)
            {
                index = 1;
                    
            }
            
            if (_chunkArray[2,i] != null && _chunkArray[2,i] == chunkData)
            {
                index = 2;
                    
            }

            if (_chunkArray[3,i] != null && _chunkArray[3,i] == chunkData)
            {
                index = 3;
                    
            }
        }
        return index;
    }
    public override void CreateGrid()
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        int count = AttackRange; // -2
        _chunkArray = new ChunkData[4,count];

        int start = 1;
        for (int i = 0; i < count; i++) 
        {
            if (GameTileMap.Tilemap.CheckBounds(centerX + i + start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX + i + start, centerY);
                _chunkList.Add(chunkData);
                _chunkArray[0, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX - i - start, centerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX-i - start, centerY);
                _chunkList.Add(chunkData);
                _chunkArray[1, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY + i + start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY + i + start);
                _chunkList.Add(chunkData);
                _chunkArray[2, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(centerX, centerY - i - start))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY - i - start);
                _chunkList.Add(chunkData);
                _chunkArray[3, i] = chunkData;
            }
        }
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (_index != -1)
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                if (_chunkArray[_index, i].CharacterIsOnTile())
                {
                    DealRandomDamageToTarget(_chunkArray[_index, i], minAttackDamage, maxAttackDamage);
                    break;
                }
            }
        }
    }

}
