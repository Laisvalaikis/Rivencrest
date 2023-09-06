using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class ThrowBehind : BaseAction
{
    private bool inFront = false;
    private bool inBack = false;
    private bool inLeft = false;
    private bool inRight = false;
    public override void ResolveAbility(Vector3 position)
    {
        if (GameTileMap.Tilemap.CharacterIsOnTile(position))
        {
            base.ResolveAbility(position);
            ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
            ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(transform.position + new Vector3(0, 0.5f, 0));
            ChunkSideByCharacter(playerChunk, chunkData);
            MoveCharacter(playerChunk, chunkData);
            // DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }

    private void ChunkSideByCharacter(ChunkData playerChunk, ChunkData chunkDataTarget)
    {
        
        (int x, int y) playerChunkIndex = playerChunk.GetIndexes();
        (int x, int y) chunkIndex = chunkDataTarget.GetIndexes();
        if (playerChunkIndex.x > chunkIndex.x)
        {
            inFront = true;
        }
        else if (playerChunkIndex.x < chunkIndex.x)
        {
            inBack = true;
        }
        else if (playerChunkIndex.y < chunkIndex.y)
        {
            inRight = true;
        }
        else if (playerChunkIndex.y > chunkIndex.y)
        {
            inLeft = true;
        }

    }

    private void MoveCharacter(ChunkData chunk, ChunkData target)
    {
        (int x, int y) playerChunkIndex = chunk.GetIndexes();
        (int x, int y) targetChunkIndex = target.GetIndexes();
        if (inFront)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y);
                Debug.Log(target.GetCurrentPlayerInformation());
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
        else if (inBack)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, targetChunkIndex.y) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, targetChunkIndex.y);
                Debug.Log(target.GetCurrentPlayerInformation());
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
        else if (inRight)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y - range) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y  - range);
                Debug.Log(positionChunk.GetPosition());
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
        else if (inLeft)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y + range) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y  + range);
                Debug.Log(target.GetCurrentPlayerInformation());
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector3(0, 0.5f, 1), target.GetCurrentPlayerInformation().gameObject);
            }
        }
    }

    protected override void FinishAbility()
    {
        inFront = false;
        inBack = false;
        inRight = false;
        inLeft = false;
        base.FinishAbility();
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
