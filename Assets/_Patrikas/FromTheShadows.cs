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
        (int y, int x) indexes = centerChunk.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(ny, nx) && chunks[ny, nx]?.GetCurrentCharacter() != null)
            {
                DealRandomDamageToTarget(chunks[nx, ny], minAttackDamage, maxAttackDamage);
            }
        }
    }

}
