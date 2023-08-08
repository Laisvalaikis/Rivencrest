using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkData
{

    public ChunkData(int indexHeight, int indexWidth, float width, float height, float positionWidth, float positionHeight, bool tileIsLocked)
    {
        _indexHeight = indexHeight;
        _indexWidth = indexWidth;
        _width = width;
        _height = height;
        _positionWidth = positionWidth;
        _positionHeight = positionHeight;
        _tileIsLocked = tileIsLocked;
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
    private int _indexWidth;
    private int _indexHeight;
    private float _weight = 0;
    private bool _weightUpdated = false;
    private bool _dataWasInserted = false;
    private int _heapIndex = -1;
    private bool _standingOnChunk = false;
    private bool _canUseTile = false;
    private bool _tileIsLocked = false;
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

    public bool TileIsLocked()
    {
        return _tileIsLocked;
    }

    public void SetTileIsLocked(bool tileIsLocked)
    {
        _tileIsLocked = tileIsLocked;
    }

    public bool CanUseTile()
    {
        return _canUseTile;
    }

    public bool IsStandingOnChunk()
    {
        return _standingOnChunk;
    }

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
