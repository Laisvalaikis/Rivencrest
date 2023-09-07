using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlaces : BaseAction
{
    private ChunkData firstSeleted;
    private ChunkData secondSelected;
    public override void ResolveAbility(Vector3 position)
    {
        
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        if (chunkData.CharacterIsOnTile())
        {
            if (firstSeleted == null || !firstSeleted.CharacterIsOnTile())
            {
                firstSeleted = chunkData;
            }
            else if(secondSelected == null || !secondSelected.CharacterIsOnTile())
            {
                secondSelected = chunkData;
                SwitchCharacters(firstSeleted, secondSelected);
                FinishAbility();
                base.ResolveAbility(position);
                firstSeleted = null;
                secondSelected = null;
            }
          
        }

        
    }

    private void SwitchCharacters(ChunkData characterOne, ChunkData characterTwo)
    {
        GameObject character = characterOne.GetCurrentCharacter();
        PlayerInformation playerInformation = characterOne.GetCurrentPlayerInformation();
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterTwo.GetPosition(), new Vector3(0, 0.5f, 1), characterOne.GetCurrentCharacter());
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterOne.GetPosition(), new Vector3(0, 0.5f, 1),
            characterTwo.GetCurrentCharacter());
        GameTileMap.Tilemap.SetCharacter(characterOne.GetPosition(), characterTwo.GetCurrentCharacter(), characterTwo.GetCurrentPlayerInformation());
        GameTileMap.Tilemap.SetCharacter(characterTwo.GetPosition(), character, playerInformation);
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
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
