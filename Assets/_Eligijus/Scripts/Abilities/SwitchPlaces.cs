using UnityEngine;

public class SwitchPlaces : BaseAction
{
    private ChunkData _firstSeleted;
    private ChunkData _secondSelected;
    public override void ResolveAbility(ChunkData chunk)
    {
        if (chunk.CharacterIsOnTile())
        {
            if (_firstSeleted == null || !_firstSeleted.CharacterIsOnTile())
            {
                _firstSeleted = chunk;
            }
            else if(_secondSelected == null || !_secondSelected.CharacterIsOnTile())
            {
                _secondSelected = chunk;
                SwitchCharacters(_firstSeleted, _secondSelected);
                FinishAbility();
                base.ResolveAbility(chunk);
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
        GameTileMap.Tilemap.SetCharacter(characterOne, characterTwo.GetCurrentCharacter(), characterTwo.GetCurrentPlayerInformation());
        GameTileMap.Tilemap.SetCharacter(characterTwo, character, playerInformationLocal);
    }
}
