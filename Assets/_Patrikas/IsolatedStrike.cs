using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IsolatedStrike : BaseAction
{
    //private string actionStateName = "IsolatedStrike";
    //public int attackDamage = 60;
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 8;
    public int isolationDamage = 7;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "IsolatedStrike";
        // AttackAbility = true;
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData target = GameTileMap.Tilemap.GetChunk(position);
        int bonusDamage = 0;
        //Isolation
        if (isTargetIsolated(target))
        {
            bonusDamage += isolationDamage;
        }

        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
        DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        FinishAbility();
    }

    public override void OnTileHover(GameObject tile)
    {
        int showMinDamage = minAttackDamage;
        int shownMaxDamage = maxAttackDamage;
        /*if (isTargetIsolated())
        {
            showMinDamage += isolationDamage;
            shownMaxDamage += isolationDamage;
        }*/

        EnableDamagePreview(tile, showMinDamage, shownMaxDamage);
    }

    bool isTargetIsolated(ChunkData target)
    {
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int y, int x) indexes = target.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && chunks[nx, ny]?.GetCurrentCharacter() != null /*patikrinti ar skirtingos komandos*/)
            {
                return false;
            }
            
        }
        return true;
    }
}
