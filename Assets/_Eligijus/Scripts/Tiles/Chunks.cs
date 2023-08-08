using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Chunks : MonoBehaviour
{

    [System.Serializable]
    private class SaveChunks
    {
        public List<ChunkData> chunks;
        
        public SaveChunks()
        {
            chunks = new List<ChunkData>();
        }
    }
    
    [System.Serializable]
    private class CollumBoundries
    {
        public List<Vector2> boundries;
    }
    

    // Start is called before the first frame update
    
    [SerializeField] private List<CollumBoundries> _mapBoundries;
    [SerializeField] private Vector2 _initialPosition;
    [SerializeField] private float _chunkSize = 3f;
    [SerializeField] private float _mapHeight = 10f;
    [SerializeField] private float _mapWidth = 10f;
    [SerializeField] private bool showChunks = false;
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
    
    void Start()
    {

        if (_chunkSize > 0)
        {
            _chunks = new List<SaveChunks>();
            _chunksDraw = new List<ChunkData>();
            _chunkCountWidth = Mathf.CeilToInt(_mapWidth / _chunkSize);
            _chunkCountHeight = Mathf.CeilToInt(_mapHeight / _chunkSize);
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

        float leftSizeOfHeight = _mapHeight;
        float heightPosition = _initialPosition.y;
        for (int h = 0; h < _chunkCountHeight; h++)
        {
            float leftSizeOfWidth = _mapWidth;
            float widthPosition = _initialPosition.x;
            
            float heightSize = _chunkSize;
                
            if (leftSizeOfHeight / _chunkSize < 1)
            {
                heightSize = leftSizeOfHeight / _chunkSize;
            }
            
            
            _chunks.Add(new SaveChunks());
            
            for (int w = 0; w < _chunkCountWidth; w++)
            {
                float widthSize = _chunkSize;
            
                if (leftSizeOfWidth / _chunkSize < 1)
                {
                    widthSize = leftSizeOfWidth / _chunkSize;
                }
                ChunkData chunk = new ChunkData(h, w, widthSize, heightSize, widthPosition + (widthSize/2), heightPosition - (heightSize/2), false);
                if (_mapBoundries[h].boundries[0].y - _chunkSize <= heightPosition - (heightSize) &&
                    _mapBoundries[h].boundries[0].y >=  heightPosition &&
                    _mapBoundries[h].boundries[0].x <= widthPosition && 
                    _mapBoundries[h].boundries[1].x >= widthPosition + widthSize)
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
        int widthChunk = Mathf.CeilToInt((position.x - _initialPosition.x)/_chunkSize) - 1;
        int heightChunk = Mathf.CeilToInt((_initialPosition.y - position.z) / _chunkSize) - 1;

        
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
        if (_chunkSize > 0 && _threadDistance == null || _chunkSize > 0 && !_threadDistance.IsAlive)
        {
            _chunks = new List<SaveChunks>();
            _chunksDraw = new List<ChunkData>();
            _chunkCountWidth = Mathf.CeilToInt(_mapWidth / _chunkSize);
            _chunkCountHeight = Mathf.CeilToInt(_mapHeight / _chunkSize);
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
        _chunkCountWidth = Mathf.CeilToInt(_mapWidth / _chunkSize);
        _chunkCountHeight = Mathf.CeilToInt(_mapHeight / _chunkSize);
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

    void OnApplicationQuit()
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
            for (int i = 0; i < _mapBoundries.Count; i++)
            {
                for (int j = 0; j < _mapBoundries[i].boundries.Count-1; j++)
                {
                    Gizmos.DrawLine(_mapBoundries[i].boundries[j], _mapBoundries[i].boundries[j+1]);
                }
                
            }
        }
    }
    
}
