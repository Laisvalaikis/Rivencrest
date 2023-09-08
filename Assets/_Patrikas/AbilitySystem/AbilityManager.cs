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
        HighlightTile previousChunkHighlight=null;
        if (previousChunk != null)
        {
            previousChunkHighlight = previousChunk.GetTileHighlight();
        }

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunk.GetTileHighlight().isHighlighted))
        {
            previousChunkHighlight.SetHighlightColor(Color.green);
        }

        _currentAbility.OnMove(hoveredChunk,previousChunk);
        if (hoveredChunk == null)
        {
            previousChunk = null;
            return;
        }
        HighlightTile hoveredChunkHighlight = hoveredChunk.GetTileHighlight();
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted)
        {
            hoveredChunkHighlight.SetHighlightColor(Color.red);
        }

        if (previousChunkHighlight != null)
        {
            previousChunkHighlight.SetHighlightColor(Color.green);
        }
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
