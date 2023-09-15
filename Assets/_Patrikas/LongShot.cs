using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LongShot : BaseAction
{
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;

    void Start()
    {
        actionStateName = "LongShot";
        isAbilitySlow = false;
    }

    public override void ResolveAbility(Vector3 position)
    {
            base.ResolveAbility(position);
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            ChunkData target = GameTileMap.Tilemap.GetChunk(position);
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            //if (DoesCharacterHaveBlessing("Poisonous shot"))
            //{
           //     target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 1));
           // }
           // if (DoesCharacterHaveBlessing("Caught one"))
           // {
           //     target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
           //     target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
           // }
           // clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("rainOfArrows");
           // clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");

            FinishAbility();
        
    }

    public override void CreateGrid(ChunkData centerChunk, int radius)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == radius)
                {
                    int targetX = centerX + x;
                    int targetY = centerY + y;

                    // Ensuring we don't go out of array bounds.
                    if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        if (chunk != null && !chunk.TileIsLocked())
                        {
                            _chunkList.Add(chunk);
                            HighlightGridTile(chunk);
                            //chunk.EnableTileRenderingGameObject();
                            //chunk.EnableTileRendering();
                        }
                    }
                }
            }
        }
    }

    public override void CreateGrid()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
        CreateGrid(startChunk, AttackRange);
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        
    }
    
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }

}