using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameTileMap gameTileMap;
    private Vector2 _mousePosition;

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
            MouseClick();
    }

    public void SetCurrentAbility(CharacterAction ability)
    {
        Debug.LogWarning("YEEEEEEYYYYYYTTTTTTTTTT " + ability.GetType());
    }
    private void MouseClick()
    {
        /*Vector3 mousePos = new Vector3(_mousePosition.x, _mousePosition.y, GetComponent<Camera>().nearClipPlane);
        Vector3 worldPos = GetComponent<Camera>().ScreenToWorldPoint(mousePos);
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
        }*/
    }
}
