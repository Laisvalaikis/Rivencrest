using UnityEngine;

public class SwitchPlaces : BaseAction
{
    private ChunkData _firstSeleted;
    private ChunkData _secondSelected;
    public override void ResolveAbility(Vector3 position)
    {
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        if (chunkData.CharacterIsOnTile())
        {
            if (_firstSeleted == null || !_firstSeleted.CharacterIsOnTile())
            {
                _firstSeleted = chunkData;
            }
            else if(_secondSelected == null || !_secondSelected.CharacterIsOnTile())
            {
                _secondSelected = chunkData;
                SwitchCharacters(_firstSeleted, _secondSelected);
                FinishAbility();
                base.ResolveAbility(position);
                _firstSeleted = null;
                _secondSelected = null;
            }
          
        }
    }
    private void SwitchCharacters(ChunkData characterOne, ChunkData characterTwo)
    {
        GameObject character = characterOne.GetCurrentCharacter();
        PlayerInformation playerInformationLocal = characterOne.GetCurrentPlayerInformation();
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterTwo.GetPosition(), new Vector3(0, 0.5f, 1), characterOne.GetCurrentCharacter());
        GameTileMap.Tilemap.MoveSelectedCharacterWithoutReset(characterOne.GetPosition(), new Vector3(0, 0.5f, 1),
            characterTwo.GetCurrentCharacter());
        GameTileMap.Tilemap.SetCharacter(characterOne.GetPosition(), characterTwo.GetCurrentCharacter(), characterTwo.GetCurrentPlayerInformation());
        GameTileMap.Tilemap.SetCharacter(characterTwo.GetPosition(), character, playerInformationLocal);
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
