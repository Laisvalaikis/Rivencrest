using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameTileMap gameTileMap;
    private Vector2 _mousePosition;
    public void OnMove(InputAction.CallbackContext context)
    { 
        _mousePosition = context.ReadValue<Vector2>();
    }
    
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
            MouseClick();
    }

    private void MouseClick()
    {
        Vector3 mousePos = new Vector3(_mousePosition.x, _mousePosition.y, camera.nearClipPlane);
        Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);
        Debug.Log(worldPos);
        if (!gameTileMap.CharacterIsSelected())
        {
            gameTileMap.SelectTile(worldPos);
        }
        else
        {
            if (!gameTileMap.CharacterIsOnTile(worldPos))
            {
                gameTileMap.MoveSelectedCharacter(worldPos, new Vector3(0, 0.5f,0));
            }
            else if(gameTileMap.CharacterIsSelected() && gameTileMap.IsSelectedCharacterIsOnTile(worldPos))
            {
                gameTileMap.DeselectCurrentCharacter();
            }
            else
            {
                gameTileMap.SelectTile(worldPos);
            }
        }
    }


}
