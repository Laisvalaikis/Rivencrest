using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class GameTileMap : MonoBehaviour
{

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
    [SerializeField] private bool showChunks;
    private int _chunkCountWidth = 0;
    private int _chunkCountHeight = 0;
    private List<SaveChunks> _chunks;
    private List<ChunkData> _chunksDraw;
    private List<ChunkData> _spawned;
    private ChunkData[,] _chunksArray;
    
    private MaxHeap _maxHeap;
    
    private Thread _threadDistance;

    private bool _updateWeight = false;
    private int _countForTreeSpawn = 0;
    private GameObject _currentSelectedCharacter;

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
            _chunksDraw = new List<ChunkData>();
            _chunkCountWidth = Mathf.CeilToInt(currentMap._mapWidth / currentMap._chunkSize);
            _chunkCountHeight = Mathf.CeilToInt(currentMap._mapHeight / currentMap._chunkSize);
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
                ChunkData chunk = new ChunkData(h, w, widthSize, heightSize, widthPosition + (widthSize/2), heightPosition - (heightSize/2), false);
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

                lock (_chunksArray)
                {
                    _chunksArray[h, w] = chunk;
                }
                _chunks[h].chunks.Add(chunk);
                _chunksDraw.Add(chunk);
                
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

    public ChunkData GetChunk(Vector3 position)
    {
        int widthChunk = Mathf.CeilToInt((position.x - currentMap._initialPosition.x)/currentMap._chunkSize) - 1;
        int heightChunk = Mathf.CeilToInt((currentMap._initialPosition.y - position.y) / currentMap._chunkSize) - 1;

        
        lock (_chunksArray)
        {
            
            if (_chunksArray[heightChunk, widthChunk] != null && !_chunksArray[heightChunk, widthChunk].TileIsLocked())
            {
                return _chunksArray[heightChunk, widthChunk];
            }
            else
            {
                return null;
            }
        }

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
            _chunksDraw = new List<ChunkData>();
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
        _chunksDraw = new List<ChunkData>();
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
            SetCharacter(_currentSelectedCharacter.transform.position, null);
            _currentSelectedCharacter.transform.position = GetChunk(mousePosition).GetPosition() - offset;
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
        }
    }

    public void DeselectCurrentCharacter()
    {
        if (_currentSelectedCharacter != null)
        {
            _currentSelectedCharacter = null;
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
        if (_chunksDraw != null)
        {
            if (showChunks)
            {
                Gizmos.color = Color.cyan;

                for (int i = 0; i < _chunksDraw.Count; i++)
                {
                    ChunkData data = _chunksDraw[i];
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
    
}
