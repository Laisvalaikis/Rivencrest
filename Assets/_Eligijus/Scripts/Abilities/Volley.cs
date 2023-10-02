using System;
using System.Collections.Generic;
using UnityEngine;

public class Volley : BaseAction //STILL FUCKED FOR THE TIEM BEING
{
    [SerializeField] private int spellDamage = 6;
    private ChunkData[,] _chunkArray;
    private List<Poison> _poisons;
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        int index = FindChunkIndex(chunk);
        if (index != -1)
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData damageChunk = _chunkArray[index, i];
                _poisons.Add(new Poison(damageChunk, 2, 1));
                DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
            }

            FinishAbility();
        }
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
    
    
    private ChunkData _tileToPullTo;
    private SpriteRenderer _characterSpriteRenderer; 
    
    private int _globalIndex = -1;
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;

        GameObject currentCharacter = GameTileMap.Tilemap.GetCurrentCharacter();
        PlayerInformation currentPlayerInfo = currentCharacter?.GetComponent<PlayerInformation>();
        
        if (_globalIndex != -1)
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
                if (chunkToHighLight != null)
                {
                    SetNonHoveredAttackColor(chunkToHighLight);
                    ResetCharacterSpriteRendererAndTilePreview();
                }
            }
        }
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            _globalIndex = FindChunkIndex(hoveredChunk);
            if (_globalIndex != -1)
            {
                for (int i = 0; i < _chunkArray.GetLength(1); i++)
                {
                    ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
                    if (chunkToHighLight != null)
                    {
                        SetHoveredAttackColor(chunkToHighLight); //todo: make it so that character move preview is shown (see chainhook)
                        if (currentPlayerInfo != null)
                        {
                            Sprite characterSprite = currentPlayerInfo.playerInformationData.characterSprite;
                            _tileToPullTo = TileToPullTo(hoveredChunk);
                            HighlightTile tileToPullToHighlight = _tileToPullTo.GetTileHighlight();
                            tileToPullToHighlight.TogglePreviewSprite(true);
                            tileToPullToHighlight.SetPreviewSprite(characterSprite);
                            _characterSpriteRenderer = currentPlayerInfo.spriteRenderer;
                            _characterSpriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                        }
                    }                
                }
            }
        }
    }
    
    private ChunkData TileToPullTo(ChunkData chunk)
    {
        Vector3 position = transform.position;
        ChunkData currentPlayerChunk = GameTileMap.Tilemap.GetChunk(position);
        (int chunkY, int chunkX) = chunk.GetIndexes();
        (int playerY, int playerX) = currentPlayerChunk.GetIndexes();

        int deltaX = chunkX - playerX;
        int deltaY = chunkY - playerY;

        // Determine the direction from the player to the chunk
        int directionX = deltaX != 0 ? deltaX / Math.Abs(deltaX) : 0;
        int directionY = deltaY != 0 ? deltaY / Math.Abs(deltaY) : 0;

        // Get the chunk next to the player in the determined direction
        Vector3 targetPosition = new Vector3(position.x + directionX, position.y - directionY, position.z);
        ChunkData targetChunk = GameTileMap.Tilemap.GetChunk(targetPosition);

        return targetChunk;
    }
    
    private void ResetCharacterSpriteRendererAndTilePreview()
    {
        if (_tileToPullTo != null)
        {
            if (_characterSpriteRenderer != null)
            {
                _characterSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
            _tileToPullTo.GetTileHighlight().TogglePreviewSprite(false);
        }
    }
    public override void CreateAvailableChunkList(int radius)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();

        int startRadius = 1;
        int count = startRadius + (AttackRange * 2)-2;
        int topLeftCornerX = centerX - AttackRange;
        int topLeftCornerY = centerY - AttackRange;
        int bottomRightCornerX = centerX + AttackRange;
        int bottomRightCornerY = centerY + AttackRange;

        _chunkArray = new ChunkData[4,count];

        const int rowStart = 1; // start is 1, because we need ignore corner tile
        for (int i = 0; i < count; i++) 
        {
            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX + i + rowStart, topLeftCornerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX + i + rowStart, topLeftCornerY);
                _chunkList.Add(chunkData);
                _chunkArray[0, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX - i - rowStart, bottomRightCornerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX-i - rowStart, bottomRightCornerY);
                _chunkList.Add(chunkData);
                _chunkArray[1, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX, topLeftCornerY + i + rowStart))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY + i + rowStart);
                _chunkList.Add(chunkData);
                _chunkArray[2, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX, bottomRightCornerY - i - rowStart))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY - i - rowStart);
                _chunkList.Add(chunkData);
                _chunkArray[3, i] = chunkData;
            }
        }
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        PoisonPlayer();
    }
    private void PoisonPlayer()
    {
        foreach (Poison x in _poisons)
        {
            if (x.poisonValue > 0 && x.chunk.GetCurrentPlayerInformation().GetHealth() > 0)
            {
                DealDamage(x.chunk, x.poisonValue, false);
            }
            x.turnsLeft--;
        }
    }
}
