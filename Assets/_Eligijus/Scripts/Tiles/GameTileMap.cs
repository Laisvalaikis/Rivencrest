using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameTileMap : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SelectAction _selectAction;
    public static GameTileMap Tilemap;
    [System.Serializable]
    public class SaveChunks
    {
        public List<ChunkData> chunks;
        
        public SaveChunks()
        {
            chunks = new List<ChunkData>();
        }
    }

    public CharacterAction selectedCharacterAction;
    public TileMapData currentMap; 
    [SerializeField] private SpriteRenderer[] tileSpriteRenderers;
    [SerializeField] private HighlightTile[] tileHighlights;
    [SerializeField] private GameObject tiles;
    [SerializeField] private bool showChunks;
    private int _chunkCountWidth = 0;
    private int _chunkCountHeight = 0;
    private List<SaveChunks> _chunks;
    private List<ChunkData> _allChunks;
    private List<ChunkData> _spawned;
    private ChunkData[,] _chunksArray;
    
    private MaxHeap _maxHeap;
    
    private Thread _threadDistance;

    private bool _updateWeight = false;
    private int _countForTreeSpawn = 0;
    private GameObject _currentSelectedCharacter;
    private PlayerInformation _currentPlayerInformation;
    private Vector2 _mousePosition;
    private int chunckIndex;
    private bool chuncksIsSetUp = false;
    private bool chunckSetupFinished = false;
    private void Awake()
    {
        if (Tilemap == null)
        {
            Tilemap = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        if (currentMap._chunkSize > 0)
        {
            _chunks = new List<SaveChunks>();
            _allChunks = new List<ChunkData>();
            _chunkCountWidth = Mathf.CeilToInt(currentMap._mapWidth / currentMap._chunkSize);
            _chunkCountHeight = Mathf.CeilToInt(currentMap._mapHeight / currentMap._chunkSize);
            int tileSize = _chunkCountWidth * _chunkCountHeight;
            Debug.Log(tileSize);
            chunckIndex = 0;
            for (int i = tileSize; i < tileSpriteRenderers.Length; i++)
            {
                tileSpriteRenderers[i].gameObject.SetActive(false);
            }
            // _avlTree = new AVL();
            _maxHeap = new MaxHeap(_chunkCountWidth*_chunkCountHeight*2);
            _chunksArray = new ChunkData[_chunkCountHeight,_chunkCountWidth];
            _threadDistance = new Thread(CalculateDistance);
            _threadDistance.Start();
           
        }
        else
        {
            Debug.LogError("Chunk size can't be 0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_threadDistance.IsAlive && !chuncksIsSetUp)
        {
            StartCoroutine(SetupChunckTiles());
            chuncksIsSetUp = true;
        }
    }

    IEnumerator SetupChunckTiles()
    {
        for (int i = 0; i < _allChunks.Count; i++)
        {
            _allChunks[i].SetupChunk();
            yield return null;
        }
        chunckSetupFinished = true;
        EnableAllTiles();
    }

    void CalculateDistance()
    {

        float leftSizeOfHeight = currentMap._mapHeight;
        float heightPosition = currentMap._initialPosition.y;
        for (int h = 0; h < _chunkCountHeight; h++)
        {
            float leftSizeOfWidth = currentMap._mapWidth;
            float widthPosition = currentMap._initialPosition.x;
            
            float heightSize = currentMap._chunkSize;
                
            if (leftSizeOfHeight / currentMap._chunkSize < 1)
            {
                heightSize = leftSizeOfHeight / currentMap._chunkSize;
            }
            
            
            _chunks.Add(new SaveChunks());
            
            for (int w = 0; w < _chunkCountWidth; w++)
            {
                float widthSize = currentMap._chunkSize;
            
                if (leftSizeOfWidth / currentMap._chunkSize < 1)
                {
                    widthSize = leftSizeOfWidth / currentMap._chunkSize;
                }
                ChunkData chunk = new ChunkData(h, w, widthSize, heightSize, widthPosition + (widthSize/2), heightPosition - (heightSize/2), tileSpriteRenderers[chunckIndex], tileHighlights[chunckIndex], false);
                if (currentMap._mapBoundries[h].boundries[0].y - currentMap._chunkSize <= heightPosition - (heightSize) &&
                    currentMap._mapBoundries[h].boundries[0].y >=  heightPosition &&
                    currentMap._mapBoundries[h].boundries[0].x <= widthPosition && 
                    currentMap._mapBoundries[h].boundries[1].x >= widthPosition + widthSize)
                {
                    chunk.SetTileIsLocked(false);
                }
                else
                {
                    chunk.SetTileIsLocked(true);
                }
                chunckIndex++;
                lock (_chunksArray)
                {
                    _chunksArray[h, w] = chunk;
                }
                _chunks[h].chunks.Add(chunk);
                _allChunks.Add(chunk);
                
                leftSizeOfWidth -= widthSize;
                widthPosition += widthSize;
            }

            heightPosition -= heightSize;
            leftSizeOfHeight -= heightSize;
        }

        // _threadDistance.Join();
        // _updateWeight = true;
        // _threadWeight = new Thread(UpdateChunkWeight);
        // _threadWeight.Start();
        // _avlTree.DisplayTree();
    }
    
    public ChunkData[,] GetChunksArray()
    {
        return _chunksArray;
    }

    public ChunkData GetChunk(Vector3 position)
    {
        int widthChunk = Mathf.CeilToInt((position.x - currentMap._initialPosition.x)/currentMap._chunkSize) - 1;
        int heightChunk = Mathf.CeilToInt((currentMap._initialPosition.y - position.y) / currentMap._chunkSize) - 1;

        
        lock (_chunksArray)
        {
            
            if (_chunksArray.GetLength(0) > heightChunk && heightChunk >= 0
                && _chunksArray.GetLength(1) > widthChunk && widthChunk >= 0
                && _chunksArray[heightChunk, widthChunk] != null && !_chunksArray[heightChunk, widthChunk].TileIsLocked())
            {
                return _chunksArray[heightChunk, widthChunk];
            }
            else
            {
                return null;
            }
        }

    }

    public void EnableAllTiles()
    {
        tiles.SetActive(true);
    }

    public void DisableAllTiles()
    {
        tiles.SetActive(false);
    }

    public ChunkData GetRandomChunkAround(int indexX, int indexY)
    {
        
        lock (_chunksArray)
        {
            
            int randomX = Random.Range(-2,2);
            int randomY = Random.Range(-2,2);
            int tempIndexX = indexX + randomX;
            int tempIndexY = indexY + randomY;
            while (tempIndexX == indexX && tempIndexY == indexY || tempIndexX < 0 || tempIndexY < 0 || tempIndexX >= _chunks.Count || tempIndexY >= _chunks[indexX].chunks.Count)
            {
                randomX = Random.Range(-2,2);
                randomY = Random.Range(-2,2);
                tempIndexX = indexX + randomX;
                tempIndexY = indexY + randomY;
            }
            return _chunks[tempIndexY].chunks[tempIndexX];
        }
    }

    public void GenerateChunks()
    {
        if (currentMap._chunkSize > 0 && _threadDistance == null || currentMap._chunkSize > 0 && !_threadDistance.IsAlive)
        {
            _chunks = new List<SaveChunks>();
            _allChunks = new List<ChunkData>();
            _chunkCountWidth = Mathf.CeilToInt(currentMap._mapWidth / currentMap._chunkSize);
            _chunkCountHeight = Mathf.CeilToInt(currentMap._mapHeight / currentMap._chunkSize);
            // _avlTree = new AVL();
            _maxHeap = new MaxHeap(_chunkCountWidth*_chunkCountHeight*2);
            _chunksArray = new ChunkData[_chunkCountHeight,_chunkCountWidth];
            _threadDistance = new Thread(CalculateDistance);
            _threadDistance.Start();
           
        }
    }

    public void ResetChunks()
    {
        _chunks = new List<SaveChunks>();
        _allChunks = new List<ChunkData>();
        _chunkCountWidth = Mathf.CeilToInt(currentMap._mapWidth / currentMap._chunkSize);
        _chunkCountHeight = Mathf.CeilToInt(currentMap._mapHeight / currentMap._chunkSize);
        _maxHeap = new MaxHeap(_chunkCountWidth*_chunkCountHeight*2);
        _chunksArray = new ChunkData[_chunkCountHeight,_chunkCountWidth];
    }

    private ChunkData GetRandomChunk()
    {
        int randomX = Random.Range(0, _chunks.Count);
        int randomY = Random.Range(0, _chunks[randomX].chunks.Count);
        return _chunks[randomY].chunks[randomX];
    }

    public ChunkData GetChunkForPosition()
    {

        if (_countForTreeSpawn < 3)
        {
            _countForTreeSpawn++;
            if (_maxHeap.CurSize() > 0)
            {
                ChunkData data = _maxHeap.GetMax();
                if (data.IsStandingOnChunk())
                {
                    (int, int) index = data.GetIndexes();
                    data = GetRandomChunkAround(index.Item1, index.Item2);
                }
                return data;
            }
            else
            {
                ChunkData data = GetRandomChunk();
                if (data.IsStandingOnChunk())
                {
                    (int, int) index = data.GetIndexes();
                    data = GetRandomChunkAround(index.Item1, index.Item2);
                }

                return data;

            }
        }
        else
        {
            _countForTreeSpawn = 0;
            ChunkData data = GetRandomChunk();
            if (data.IsStandingOnChunk())
            {
                (int, int) index = data.GetIndexes();
                data = GetRandomChunkAround(index.Item1, index.Item2);

            }
            return data;
        }

    }

    public void SetCharacter(Vector3 mousePosition, GameObject character)
    {
        if (GetChunk(mousePosition) != null)
        {
            ChunkData chunkData = GetChunk(mousePosition);
            chunkData.SetCurrentCharacter(character);
            chunkData.GetTileHighlight().ActivatePlayerTile(true);
        }
    }

    public void ResetChunkCharacter(Vector3 mousePosition)
    {
        if (GetChunk(mousePosition) != null)
        {
            ChunkData chunk = GetChunk(mousePosition);
            chunk.SetCurrentCharacter(null, null);
            chunk.GetTileHighlight().ActivateMovementTile(false);
        }
    }

    public void SetCharacter(Vector3 mousePosition, GameObject character, PlayerInformation playerInformation)
    {
        if (GetChunk(mousePosition) != null)
        {
            ChunkData chunkData = GetChunk(mousePosition);
            Debug.Log("position: " + mousePosition + " playername: " +character.name);
            chunkData.SetCurrentCharacter(character, playerInformation);
            chunkData.GetTileHighlight().ActivatePlayerTile(true);
        }
    }

    public bool CharacterIsOnTile(Vector3 mousePosition)
    {
        if (GetChunk(mousePosition) != null)
        {
            ChunkData chunkData = GetChunk(mousePosition);
            return chunkData.GetCurrentCharacter() != null;
        }

        return false;
    }
    
    public bool IsSelectedCharacterIsOnTile(Vector3 mousePosition)
    {
        if (GetChunk(mousePosition) != null)
        {
            ChunkData chunkData = GetChunk(mousePosition);
            return chunkData.GetCurrentCharacter() != null && chunkData.GetCurrentCharacter() == _currentSelectedCharacter;
        }

        return false;
    }

    public void MoveSelectedCharacter(Vector3 mousePosition, Vector3 offset = default)
    {
        
        // GameObject FinalDestination = SelectedCharacter.GetComponent<GridMovement>().PickUpWayTiles();
        // if (SelectedCharacter.GetComponent<GridMovement>().AreThereConsumablesOnTheWay())
        // {
        //     undoAction.available = false;
        // }
        // if (FinalDestination != null)
        // {
        //     newPosition = FinalDestination;
        // }


        if (GetChunk(mousePosition) != null)
        {
            ChunkData previousCharacterChunk =
                GameTileMap.Tilemap.GetChunk(_currentSelectedCharacter.transform.position);
            ResetChunkCharacter(previousCharacterChunk.GetPosition());
            Vector3 characterPosition = GetChunk(mousePosition).GetPosition() - offset;
            _currentSelectedCharacter.transform.position = characterPosition;
            SetCharacter(mousePosition, _currentSelectedCharacter);
            
        }
        // SelectedCharacter.GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
        // bottomCornerUI.EnableAbilities(SelectedCharacter.GetComponent<PlayerInformation>().savedCharacter);
    }

    public void SelectTile(Vector3 mousePosition)
    {
        
        if (GetChunk(mousePosition) != null)
        {
            Debug.Log("Pressed");
            ChunkData chunkData = GetChunk(mousePosition);
            _currentSelectedCharacter = chunkData.GetCurrentCharacter();
            _currentPlayerInformation = chunkData.GetCurrentPlayerInformation();
            if (_currentSelectedCharacter != null)
            {
                _selectAction.SetCurrentCharacter(_currentSelectedCharacter);
            }
        }
    }

    public void DeselectCurrentCharacter()
    {
        if (_currentSelectedCharacter != null)
        {
            _currentSelectedCharacter = null;
            _currentPlayerInformation = null;
            _selectAction.gameObject.SetActive(false);
        }
    }

    public bool CharacterIsSelected()
    {
        return _currentSelectedCharacter != null;
    }

    public GameObject GetCurrentCharacter()
    {
        return _currentSelectedCharacter;
    }

    public void SetCurrentCharacter(GameObject currentCharacter)
    {
        _currentSelectedCharacter = currentCharacter;
    }

    // void OnApplicationQuit()
    // {
    //     
    //     
    // }

    private void OnDestroy()
    {
        _updateWeight = false;
        if (_threadDistance.IsAlive)
        {
            _threadDistance.Abort();
        }
        else
        {
            _threadDistance.Join(); 
        }
        if (Tilemap == this)
        {
            Tilemap = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_allChunks != null)
        {
            if (showChunks)
            {
                Gizmos.color = Color.cyan;

                for (int i = 0; i < _allChunks.Count; i++)
                {
                    ChunkData data = _allChunks[i];
                    if (!data.TileIsLocked())
                    {
                        Gizmos.color = new Color(0, 1, data.GetWeight(), 1);
                        Gizmos.DrawCube(data.GetPosition(), data.GetDimensions());
                    }
                }
            }

            Gizmos.color = Color.red;
            if (currentMap != null)
            {
                for (int i = 0; i < currentMap._mapBoundries.Count; i++)
                {
                    for (int j = 0; j < currentMap._mapBoundries[i].boundries.Count - 1; j++)
                    {
                        Gizmos.DrawLine(currentMap._mapBoundries[i].boundries[j],
                            currentMap._mapBoundries[i].boundries[j + 1]);
                    }

                }
            }
        }
    }
    
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
        Vector3 mousePos = new Vector3(_mousePosition.x, _mousePosition.y, mainCamera.nearClipPlane);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        if (!CharacterIsSelected())
        {
            SelectTile(worldPos);
        }
        else if(CharacterIsSelected() && CharacterIsOnTile(worldPos))
        {
            DeselectCurrentCharacter();
        }
    }
    
}
