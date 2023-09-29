using System;
using System.Collections.Generic;
using UnityEngine;

public class ChainHook : BaseAction
{
    //private string actionStateName = "ChainHook";
    //public int minAttackDamage = 0;
    //public int maxAttackDamage = 1;
    
    void Start()
    {
        AttackHighlight = new Color32(123,156, 178,255);
        AttackHighlightHover = new Color32(103, 136, 158, 255);
        CharacterOnGrid = new Color32(146, 212, 255, 255);
        laserGrid = true;
        isAbilitySlow = false;
    }

    protected override void GeneratePlusPattern(ChunkData centerChunk, int length)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        bool[] canExtend = { true, true, true, true };

        for (int i = 1; i <= length; i++)
        {
            List<(int, int, int)> positions = new List<(int, int, int)> 
            {
                (centerX, centerY + i, 0),  // Up
                (centerX, centerY - i, 1),  // Down
                (centerX + i, centerY, 2),  // Right
                 (centerX - i, centerY, 3)   // Left
            };
            foreach (var (x, y, direction) in positions)
            {
                if (!canExtend[direction])
                {
                    continue;
                }
                if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
                {
                    ChunkData chunk = chunksArray[x, y];
                    if (chunk != null && !chunk.TileIsLocked())
                    {
                        if (chunk.GetInformationType() == InformationType.Object)
                        {
                            canExtend[direction] = false;
                            continue;
                        }
                        _chunkList.Add(chunk);
                        if (chunk.GetCurrentCharacter() != null)
                        {
                            canExtend[direction] = false;
                        }
                    }
                }
            }
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        GameObject character = chunk.GetCurrentCharacter();
        if (character!=null)
        {
            //target.transform.Find("VFX").Find("VFXImpact").gameObject.GetComponent<Animator>().SetTrigger("burgundy2");
            if (!IsAllegianceSame(chunk))
            { 
                int multiplier = GetMultiplier(chunk.GetPosition());
                if (multiplier != 0)
                { 
                    DealRandomDamageToTarget(chunk, minAttackDamage + multiplier * 2, maxAttackDamage + multiplier * 2);
                }
            }
            //target.transform.position = transform.position + new Vector3(hookVectors[tileIndex].Item1, hookVectors[tileIndex].Item2, 0f);
            //character.transform.position = TileToPullTo(target).transform.position;
            ChunkData chunkToPullTo = TileToPullTo(chunk);
            GameTileMap.Tilemap.MoveSelectedCharacter(chunkToPullTo.GetPosition(),new Vector3(0, 0.5f, 1),character);
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            FinishAbility();
        }
        
        //Grappling hook blessing
        // else if (CheckIfSpecificTag(clickedTile, 0, 0, blockingLayer, "Wall") && grapplingHook) 
        // {
        //     if (TileToDashTo(clickedTile) != null)
        //     { 
        //         transform.position = TileToDashTo(clickedTile).transform.position + new Vector3(0f, 0f, -1f);
        //         GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FocusSelectedCharacter(gameObject);
        //     }
        //     transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
        //     FinishAbility();
        // }
    }
    
    private int GetMultiplier(Vector3 position)
    {
        Vector3 vector3 = position - transform.position;
        int multiplier = Mathf.Abs((int)vector3.x + (int)vector3.y) - 1;
        return multiplier;
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

    private ChunkData _tileToPullTo;
    private SpriteRenderer _characterSpriteRenderer; 
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

        GameObject currentCharacter = hoveredChunk?.GetCurrentCharacter();
        PlayerInformation currentPlayerInfo = currentCharacter?.GetComponent<PlayerInformation>();

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
        {
            previousChunkHighlight.SetHighlightColor(AttackHighlight);
            ResetCharacterSpriteRendererAndTilePreview();
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted)
        {
            SetHoveredChunkHighlight(hoveredChunk, currentPlayerInfo);
        }
        if (previousChunkHighlight != null)
        {
            if (_tileToPullTo != null && currentCharacter == null)
            {
                _tileToPullTo.GetTileHighlight().TogglePreviewSprite(false);
                if(_characterSpriteRenderer != null)
                {
                    _characterSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                }
            }
            previousChunkHighlight.SetHighlightColor(AttackHighlight);
        }
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

private void SetHoveredChunkHighlight(ChunkData hoveredChunk, PlayerInformation currentPlayerInfo)
{
    SetHoveredAttackColor(hoveredChunk);
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
    
    /*
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        ChainHook ability = new ChainHook();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;
        ability.grapplingHook = this.grapplingHook;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "No escape") != null)
        {
            ability.AttackRange++;
        }
        if (blessings.Find(x => x.blessingName == "Grappling hook") != null)
        {
            ability.grapplingHook = true;
        }


        return ability;
    }*/
}