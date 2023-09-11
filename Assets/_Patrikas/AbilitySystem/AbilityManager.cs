using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameTileMap gameTileMap;
    private Vector2 _mousePosition;
    private BaseAction _currentAbility;
    private ChunkData previousChunk;
    private List<ChunkData> _path;
    private List<ChunkData> _lastPath;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_currentAbility == null) return;
        _mousePosition = context.ReadValue<Vector2>();
        Vector3 worldPos = camera.ScreenToWorldPoint(_mousePosition);
        ChunkData hoveredChunk = gameTileMap.GetChunk(worldPos);
        
        _currentAbility.OnMoveArrows(hoveredChunk,previousChunk);
        _currentAbility.OnMoveHover(hoveredChunk,previousChunk);
        if(previousChunk!=hoveredChunk)
            previousChunk = hoveredChunk;
    }
    
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ExecuteCurrentAbility();
        }
    }

    public void SetCurrentAbility(BaseAction ability)
    {
        if (_currentAbility != null)
        {
            _currentAbility.ClearGrid();
        }

        _currentAbility = ability;
        _currentAbility.CreateGrid();
    }
    
    public bool IsAbilitySelected()
    {
        return _currentAbility != null;
    }

    public void DeselectAbility()
    {
        _currentAbility = null;
    }

    private void ExecuteCurrentAbility()
    {
        if (_currentAbility != null)
        {
            Vector3 mousePos = new Vector3(_mousePosition.x, _mousePosition.y, camera.nearClipPlane);
            Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);
            ChunkData chunk = gameTileMap.GetChunk(worldPos);
            if (chunk != null)
            {
                _currentAbility.ResolveAbility(chunk.GetPosition());
                _currentAbility.OnTileClick(worldPos);
            }
        }
    }
}
