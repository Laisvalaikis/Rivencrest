using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChainHook : BaseAction
{
    //private string actionStateName = "ChainHook";
    //public int minAttackDamage = 0;
    //public int maxAttackDamage = 1;
    private bool grapplingHook = false;
    private bool canTileBeHovered = true;
    
    void Start()
    {
        AttackHighlight = new Color32(0x7B,0x9C,0xB2,0xFF);
        OtherHighlight = new Color32(0x67, 0x88, 0x9E, 0xFF);
        laserGrid = true;
        actionStateName = "ChainHook";
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

                        HighlightGridTile(chunk);

                        if (chunk.GetCurrentCharacter() != null)
                        {
                            canExtend[direction] = false;
                        }
                    }
                }
            }
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
        GameObject character = chunk.GetCurrentCharacter();
        if (character!=null)
        {
            //target.transform.Find("VFX").Find("VFXImpact").gameObject.GetComponent<Animator>().SetTrigger("burgundy2");
            if (!isAllegianceSame(position))
            { 
                int multiplier = GetMultiplier(position);
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
        int multiplier = Mathf.Abs((int)vector3.x + (int)vector3.y) - 1;//gal dar prireiks, o gal ziurek kitiem spellam taip padarysi ¯\_(ツ)_/¯
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

    private ChunkData tileToPullTo;
    private Sprite characterSprite;
    private SpriteRenderer characterSpriteRenderer; 
    
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
        SetHoveredChunkHighlight(hoveredChunk, hoveredChunkHighlight, currentCharacter, currentPlayerInfo);
    }

    if (previousChunkHighlight != null)
    {
        if (tileToPullTo != null && currentCharacter == null)
        {
            tileToPullTo.GetTileHighlight().TogglePreviewSprite(false);
            if(characterSpriteRenderer != null)
            {
                characterSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
        }
        previousChunkHighlight.SetHighlightColor(AttackHighlight);
    }
}

private void ResetCharacterSpriteRendererAndTilePreview()
{
    if (tileToPullTo != null)
    {
        if (characterSpriteRenderer != null)
        {
            characterSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
        tileToPullTo.GetTileHighlight().TogglePreviewSprite(false);
    }
}

private void SetHoveredChunkHighlight(ChunkData hoveredChunk, HighlightTile hoveredChunkHighlight, GameObject currentCharacter, PlayerInformation currentPlayerInfo)
{
    if (currentCharacter == null)
    {
        hoveredChunkHighlight.SetHighlightColor(OtherHighlight);
    }
    else
    {
        hoveredChunkHighlight.SetHighlightColor(AttackHighlightHover);

        if (currentPlayerInfo != null)
        {
            characterSprite = currentPlayerInfo.playerInformationData.characterSprite;
            tileToPullTo = TileToPullTo(hoveredChunk);
            HighlightTile tileToPullToHighlight = tileToPullTo.GetTileHighlight();
            tileToPullToHighlight.TogglePreviewSprite(true);
            tileToPullToHighlight.SetPreviewSprite(characterSprite);
            characterSpriteRenderer = currentPlayerInfo.spriteRenderer;
            characterSpriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
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