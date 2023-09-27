using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityToUse
{
    public BaseAction _action;
    public ChunkData chunkToCallOn;
    private bool isOffensive = true;
    public float coefficient = 0.5f;
}

public class AIBehaviour : MonoBehaviour
{
    private ChunkData[,] chunkArray;
    [SerializeField] private ActionManagerNew actionManager;
    private List<Ability> _abilities;
    private float confidence = 0.0f;
    
    public void Start()
    {
        chunkArray = GameTileMap.Tilemap.GetChunksArray();
        _abilities = actionManager.GetAbilities();
    }
    private bool ShouldCharacterFearChunk(ChunkData chunk)
    {
        return false;
    }
    
    private bool AbilityToBeUsed(BaseAction action)
    {
        List<ChunkData> chunksForAttack = new List<ChunkData>();
        action.CreateAvailableChunkList(action.AttackRange);
        chunksForAttack = action.GetChunkList();
        foreach (var chunk in chunksForAttack)
        {
            if (chunk.GetCurrentCharacter() != null && action.CanTileBeClicked(chunk))
                return true;
        }
            
        return false;
    }
    
}
