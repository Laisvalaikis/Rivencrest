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
    private HighlightTile _previousChunk;
    private List<ChunkData> _path;
    private List<ChunkData> _lastPath;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_currentAbility == null) return;
        _mousePosition = context.ReadValue<Vector2>();
        Vector3 worldPos = camera.ScreenToWorldPoint(_mousePosition);
        ChunkData[,] chunkArray = gameTileMap.GetChunksArray();
        ChunkData hoveredChunk = gameTileMap.GetChunk(worldPos);
        if (hoveredChunk == null) return;
        
        HighlightTile hoveredChunkHighlight = hoveredChunk.GetTileHighlight();
        if (hoveredChunkHighlight == null || hoveredChunkHighlight == _previousChunk) return;
        
        if (hoveredChunkHighlight.isHighlighted)
        {
            if (_lastPath != null)

            {
                foreach (ChunkData chunk in _lastPath)
                {
                    chunk.GetTileHighlight().DeactivateArrowTile();

                }
            }
            if (_lastPath != null && _lastPath.Any() && IsAdjacent(hoveredChunk, _lastPath[^1]))
            {
                UpdatePath(hoveredChunk, _lastPath, chunkArray);
            }
            else
            {
                _lastPath = GetDiagonalPath(gameTileMap.GetChunk(_currentAbility.transform.position), hoveredChunk, chunkArray);
            }
            SetTileArrow(_lastPath,0,_lastPath.Count-1);

        }
        _previousChunk = hoveredChunkHighlight;
    }
    
    private List<ChunkData> GetDiagonalPath(ChunkData start, ChunkData end, ChunkData[,] chunkArray)
    {
        List<ChunkData> stairStepPath = new List<ChunkData>();

        // Get the starting and ending indexes
        int startX = start.GetIndexes().Item2;
        int startY = start.GetIndexes().Item1;
        int endX = end.GetIndexes().Item2;
        int endY = end.GetIndexes().Item1;

        int x = startX, y = startY;

        // Determine the direction of the moves
        int xStep = (endX > startX) ? 1 : -1;
        int yStep = (endY > startY) ? 1 : -1;

        while (x != endX || y != endY)
        {
            if (x != endX)
            {
                stairStepPath.Add(chunkArray[y, x]);
                x += xStep;
            }
        
            if (y != endY)
            {
                stairStepPath.Add(chunkArray[y, x]);
                y += yStep;
            }
        }
        // Add the end point
        stairStepPath.Add(chunkArray[endY, endX]);
        return stairStepPath;
    }
    private void UpdatePath(ChunkData newEnd, List<ChunkData> existingPath, ChunkData[,] chunkArray)
    {
        int expectedLength = GetExpectedPathLength(existingPath[0], newEnd);
    
        if (existingPath.Count > expectedLength)
        {
            ChunkData startingPoint = existingPath[0];
            existingPath.Clear();
            existingPath.AddRange(GetDiagonalPath(startingPoint, newEnd, chunkArray));
            // Reset all arrows as the path has been cleared
            SetTileArrow(existingPath, 0, existingPath.Count - 1);
        }
        else
        {
            // Update the arrow for the old end point first
            if (existingPath.Count > 1)
            {
                SetTileArrow(existingPath, existingPath.Count - 2, existingPath.Count - 1);
            }

            // Add the new end point and set its arrow
            existingPath.Add(newEnd);
            SetTileArrow(existingPath, existingPath.Count - 1, existingPath.Count - 1);
        }
    }
    private int GetExpectedPathLength(ChunkData start, ChunkData end)
    {
        var (startX, startY) = start.GetIndexes();
        var (endX, endY) = end.GetIndexes();
    
        return Math.Abs(endX - startX) + Math.Abs(endY - startY);
    }
    private bool IsAdjacent(ChunkData a, ChunkData b)
    {
        var (ax, ay) = a.GetIndexes();
        var (bx, by) = b.GetIndexes();

        return Math.Abs(ax - bx) + Math.Abs(ay - by) == 1;
    }

    private void SetTileArrow(List<ChunkData> path, int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            ChunkData current = path[i];
            ChunkData prev = i > 0 ? path[i - 1] : null;
            ChunkData next = i < path.Count - 1 ? path[i + 1] : null;

            int arrowType = DetermineArrowType(current, prev, next);
            path[i].GetTileHighlight().SetArrowSprite(arrowType);
        }
    }

    private int DetermineArrowType(ChunkData current, ChunkData prev, ChunkData next)
    {
        if (prev == null && next == null) return 0;  // Invalid case

        var (cy, cx) = current.GetIndexes();
        var (py, px) = prev?.GetIndexes() ?? (0, 0);
        var (ny, nx) = next?.GetIndexes() ?? (0, 0);
        
        if (prev == null)  // Start
        {
            if (cx < nx) return 1;  // Right Start
            if (cx > nx) return 2;  // Left Start
            if (cy < ny) return 3;  // Down Start
            if (cy > ny) return 4;  // Up Start
        }
        else if (next == null)  // End
        {
            if (cx > px) return 5;  // Right End
            if (cx < px) return 6;  // Left End
            if (cy > py) return 7;  // Down End
            if (cy < py) return 8;  // Up End
        }
        else  // Intermediate or Corner
        {
            if (cx == px && cx == nx) return 9;  // Vertical Intermediate
            if (cy == py && cy == ny) return 10; // Horizontal Intermediate
            Debug.Log(cx + " " + px);
            Debug.Log(cy + " " + py);
            
            if ((cx > px && cy == py && cx == nx && cy > ny) || (cx == px && cy > py && cx > nx && cy == ny))
                return 11;
            
            if ((px == cx && py < cy && cx < nx && cy == ny) || (px > cx && cy == py && cx == nx && cy > ny))
                return 12;
            
            if ((cx == px && cy < py && cx < nx && cy == ny) || (cx < px && cy == py && cx == nx && cy < ny))
                return 14;

            if ((cx == px && cy < py && cx > nx && cy == ny) || (cx > px && cy == py && cx == nx && cy < ny))
                return 13;
            
            
            /*if (cx != px && cy == py && px < cx && cy > ny) return 11; // top right Corner
            if (cx != px && cy == py && px > cx && cy > ny) return 13; // top left Corner
            
            if (cx != px && cy == py && px < cx && cy < ny) return 14; // bottom right Corner
            if (cx != px && cy == py && px > cx && cy < ny) return 12; // bottom left Corner*/
            
            // if (cx != px && cy != py) return 12; // Top Left Corner
            // if (cx != nx && cy != ny) return 13; // Bottom Right Corner nope
            // if (cx != nx && cy != py) return 14; // Top Right Corner

        }
        return 0;  
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
