using System;
using UnityEngine;

public class ThrowBehind : BaseAction
{
    private Side _side;
    public override void ResolveAbility(ChunkData chunk)
    {
        if (GameTileMap.Tilemap.CharacterIsOnTile(chunk))
        {
            base.ResolveAbility(chunk);
            ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0));
            ChunkSideByCharacter(playerChunk, chunk);
            MoveCharacter(playerChunk, chunk);
            // DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }
    

    private void MoveCharacter(ChunkData chunk, ChunkData target)
    {
        (int x, int y) playerChunkIndex = chunk.GetIndexes();
        (int x, int y) targetChunkIndex = target.GetIndexes();
        if (_side == Side.isFront)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
        else if (_side == Side.isBack)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, targetChunkIndex.y) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, targetChunkIndex.y);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
        else if (_side == Side.isRight)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y - range) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y  - range);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
        else if (_side == Side.isLeft)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y + range) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y  + range);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
    }

    protected override void FinishAbility()
    {
        _side = Side.none;
        base.FinishAbility();
    }
    
}
