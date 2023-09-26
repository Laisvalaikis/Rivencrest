using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    private ChunkData[,] chunkArray;
    [SerializeField] private ActionManagerNew actionManager;
    private List<Ability> _abilities;
    public void Start()
    {
        chunkArray = GameTileMap.Tilemap.GetChunksArray();
        _abilities = actionManager.GetAbilities();
    }
    private bool ShouldCharacterFearChunk(ChunkData chunk)
    {
        return false;
    }


    private bool CanAttackPlayer()
    {
        foreach (var ability in _abilities)
        {
            if (ability.enabled)
            {
                
            }
        }

        return false;
    }
    
}
