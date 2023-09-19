using System.Collections.Generic;
using UnityEngine;

public class PoisonDart : BaseAction
{
    public string ImpactName = "forest3";

    private List<GameObject> PoisonTiles = new List<GameObject>();
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;



    void Start()
    {
        isAbilitySlow = false;
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        //target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
        //clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("crowAttack");
        //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger(ImpactName);
        if (DoesCharacterHaveBlessing("Explosive dart"))
        {
            PoisonAdjacent(chunk);
        }
        FinishAbility();
    }

    private void PoisonAdjacent(ChunkData centerChunk)
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
                //target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));            }
            }
        }
    }
}

//public override void OnTileHover(GameObject tile)
    /*{
        print("on tile hover");
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
        if(DoesCharacterHaveBlessing("Explosive dart"))
        {
            CreatePoisonTileList(tile);
            foreach(GameObject tileInList in PoisonTiles)
            {
                tileInList.transform.Find("mapTile").Find("Highlight").gameObject.SetActive(true);
            }
        }*/
    //}
    /*public override void OffTileHover(GameObject tile)
    {
        print("off tile hover");
        DisablePreview(tile);
        if (DoesCharacterHaveBlessing("Explosive dart"))
        {
            //tile.transform.Find("mapTile").Find("Direction").gameObject.SetActive(false);
            foreach (GameObject tileInList in PoisonTiles)
            {
                DisablePreview(tileInList);
            }
        }
    }*/


