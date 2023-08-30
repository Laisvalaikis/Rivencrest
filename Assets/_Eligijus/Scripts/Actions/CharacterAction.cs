using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction : MonoBehaviour
{
    public virtual void OnTileClick(Vector3 mousePosition)
    {
        
    }

    public virtual void CreateGrid()
    {
        
    }

    public virtual void ClearGrid()
    {
        
    }
    
    public virtual void DealRandomDamageToTarget(ChunkData chunkData, int minDamage, int maxDamage)
    {
        
    }

    protected virtual List<ChunkData> GeneratePattern(ChunkData centerChunk, ChunkData[,] chunksArray, int length)
    {
        return null;
    }

    public virtual void ResolveAbility(Vector3 position)
    {
    }
}//visos visu abilities funkcijos
