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
    private BaseAction _currentAbility;

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _mousePosition = Mouse.current.position.ReadValue();
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

    public void ExecuteCurrentAbility()
    {
        if (_currentAbility != null)
        {
            Debug.Log("Executing a non-null ability");
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
