using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileMapData", menuName = "ScriptableObjects/TileMapData", order = 1)]
public class TileMapData : ScriptableObject
{


    public List<CollumBoundries> _mapBoundries;
    public Vector2 _initialPosition;
    public float _chunkSize = 3f;
    public float _mapHeight = 10f;
    public float _mapWidth = 10f;
}

[System.Serializable]
public class CollumBoundries
{
    public List<Vector2> boundries;
}