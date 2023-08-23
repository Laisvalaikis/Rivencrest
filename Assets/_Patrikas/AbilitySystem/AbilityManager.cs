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
            ExecuteCurrentAbility();
    }

    public void SetCurrentAbility(CharacterAction ability)
    {
        _currentAbility = ability;
        Debug.LogError(ability.GetType());
    }

    public void ExecuteCurrentAbility()
    {
        if (_currentAbility != null)
        {
            Debug.LogError("TRIED ABILITY EXECUTION");
            //_currentAbility.ResolveAbility();
            Vector3 mousePos = new Vector3(_mousePosition.x, _mousePosition.y, GetComponent<Camera>().nearClipPlane);
            _currentAbility.OnTileClick(mousePos);
        }
    }
    
}
