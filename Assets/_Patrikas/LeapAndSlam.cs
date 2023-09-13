using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeapAndSlam : BaseAction
{
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 5;

    void Start()
    {
        actionStateName = "LeapAndSlam";
        isAbilitySlow = false;
    }
    
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
        if (!GameTileMap.Tilemap.CharacterIsOnTile(position))
        {
            GameTileMap.Tilemap.MoveSelectedCharacter(position, new Vector3(0, 0.5f, 1));
        }
        DamageAdjacent(chunk);
        FinishAbility();
        
    }
    private void DamageAdjacent(ChunkData centerChunk)
    {
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int y,int x) indexes = centerChunk.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;
        if (GameTileMap.Tilemap.CheckBounds(x,y+1) && chunks[x, y + 1].GetCurrentCharacter() != null)
        {
            DealRandomDamageToTarget(chunks[x, y + 1], minAttackDamage,maxAttackDamage);
        }
        if (GameTileMap.Tilemap.CheckBounds(x,y-1) && chunks[x, y + 1].GetCurrentCharacter() != null)
        {
            DealRandomDamageToTarget(chunks[x, y - 1], minAttackDamage,maxAttackDamage);
        }
        if (GameTileMap.Tilemap.CheckBounds(x+1,y) && chunks[x, y + 1].GetCurrentCharacter() != null)
        {
            DealRandomDamageToTarget(chunks[x+1, y], minAttackDamage,maxAttackDamage);
        }
        if (GameTileMap.Tilemap.CheckBounds(x-1,y) && chunks[x, y + 1].GetCurrentCharacter() != null)
        {
            DealRandomDamageToTarget(chunks[x-1, y], minAttackDamage,maxAttackDamage);
        }
    }
}
