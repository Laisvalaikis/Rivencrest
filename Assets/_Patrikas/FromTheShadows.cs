using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class FromTheShadows : BaseAction
{
    public string ImpactName = "red1";
    //public int minAttackDamage = 3;
    //public int maxAttackDamage = 4;
    


    void Start()
    {
        actionStateName = "FromTheShadows";
        //isAbilitySlow = false;
    }
    public override void ResolveAbility(Vector3 position)
    {
            base.ResolveAbility(position);
            ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            if (!GameTileMap.Tilemap.CharacterIsOnTile(position))
            {
                GameTileMap.Tilemap.MoveSelectedCharacter(position, new Vector3(0, 0.5f, 1));
            }
            DamageAdjacent(chunk);
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger(ImpactName);        
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
