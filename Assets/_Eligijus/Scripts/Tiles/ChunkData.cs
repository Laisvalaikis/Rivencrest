using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkData
{

    public ChunkData(int indexHeight, int indexWidth, float width, float height, float positionWidth, float positionHeight)
    {
        _indexHeight = indexHeight;
        _indexWidth = indexWidth;
        _width = width;
        _height = height;
        _positionWidth = positionWidth;
        _positionHeight = positionHeight;
    }
    
    public ChunkData(int indexHeight, int indexWidth)
    {
        _indexHeight = indexHeight;
        _indexWidth = indexWidth;
    }

    public ChunkData()
    {
    }

    private float _width;
    private float _height;
    private float _positionHeight;
    private float _positionWidth;
    private int _playerHasBeenHere;
    private int _indexWidth;
    private int _indexHeight;
    private float _weight = 0;
    private bool _canSpawn = true;
    private float _prevWeight = 0f;
    private bool _weightUpdated = false;
    private bool _dataWasInserted = false;
    private int _heapIndex = -1;
    private bool _standingOnChunk = false;
    private int _enemyDiedCount = 0;
    private float _enemyDiedWeight = 0;
    private bool _wasSpawned = false;
    private int _wasntSpawnedCount = 0;
    private int _wasntSpawnedResetTime = 200;
    public Vector3 GetPosition()
    {
        return new(_positionWidth, _positionHeight, 0);
    }

    public Vector3 GetDimensions()
    {
        return new(_width,_height, 1f);
    }


    public void StandingOnChunk(bool standingOnChunk)
    {
        _standingOnChunk = standingOnChunk;
    }
    
    public bool IsStandingOnChunk()
    {
        return _standingOnChunk;
    }

    // public void UpdateWeight()
    // {
    //     
    //         if (_wasntSpawnedCount < _wasntSpawnedResetTime && _wasSpawned)
    //         {
    //             _wasntSpawnedCount++;
    //         }
    //         else
    //         {
    //             _wasntSpawnedCount = 0;
    //             _wasSpawned = false;
    //         }
    //
    //         _prevWeight = _weight;
    //         float weightPart1 = _deathTracker != null ? (float)_enemyDiedCount / _deathTracker.GetDeathCount() * 0.15f:0f;
    //         float weightPart2 = _enemyDiedWeight * 0.1f;
    //         float weightPart3 = _standingOnChunk ? 0.05f : 0;
    //         float weightPart4 = _playerTracking != null ? (float)_playerHasBeenHere / _playerTracking.PlayerPositionCount() * 0.2f : 0f;
    //         float weightPart5 = !_wasSpawned ? 0.6f : -0.4f;
    //         weightPart5 = _standingOnChunk && !_wasSpawned ? weightPart5 - 0.3f : weightPart5;
    //         // if (!_wasSpawned && !_standingOnChunk)
    //         // {
    //         //     if (weightPart1 == 0)
    //         //     {
    //         //         weightPart5 += 0.15f;
    //         //     }
    //         //     if (weightPart2 == 0)
    //         //     {
    //         //         weightPart5 += 0.1f;
    //         //     }
    //         // }
    //
    //         _weight = weightPart1 + weightPart2 + weightPart3 + weightPart4 + weightPart5;
    //         
    //         if (Mathf.Abs(_prevWeight-_weight) > 0.001f)
    //         {
    //             _weightUpdated = true;
    //         }
    //         else
    //         {
    //             _weightUpdated = false;
    //         }
    //         
    //     
    // }

    public bool WeightWasUpdated()
    {
        return _weightUpdated;
    }

    public void InsertData()
    {
        _dataWasInserted = true;
    }

    public bool DataWasInserted()
    {
        return _dataWasInserted;
    }

    public float GetWeight()
    {
        return _weight;
    }

    public void SetWeight(int weight)
    {
        _weight = weight;
    }

    public void SetWasSpawned()
    {
        _wasSpawned = true;
    }

    public bool WasSpawned()
    {
        return _wasSpawned;
    }

    public void EnemyDied(float enemyWeight)
    {
        _enemyDiedWeight = enemyWeight;
        _enemyDiedCount++;
    }

    public int GetGeneratedIndex()
    {
        return _indexWidth+(_indexHeight * 19);
    }

    public (int,int) GetIndexes()
    {
        return (_indexHeight, _indexWidth);
    }

    public void SetHeapIndex(int index)
    {
        _heapIndex = index;
    }

    public int GetHeapIndex()
    {
        return _heapIndex;
    }

}
