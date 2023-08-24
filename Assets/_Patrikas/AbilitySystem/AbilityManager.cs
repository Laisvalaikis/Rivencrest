using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameTileMap gameTileMap;
    private Vector2 _mousePosition;
    private CharacterAction _currentAbility;

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mousePosition = Mouse.current.position.ReadValue();
            ExecuteCurrentAbility();
        }
    }

    public void SetCurrentAbility(CharacterAction ability)
    {
        _currentAbility = ability;
    }

    public void ExecuteCurrentAbility()
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
