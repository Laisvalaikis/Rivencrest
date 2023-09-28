using System.Collections.Generic;
using UnityEngine;

public class PowerShot : BaseAction
{
    //private string actionStateName = "PowerShot";
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 7;

    void Start()
    {
        laserGrid = true;
    }
    
    protected override void GeneratePlusPattern(ChunkData centerChunk, int length)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        bool[] canExtend = { true, true, true, true };

        for (int i = 1; i <= length; i++)
        {
            List<(int, int, int)> positions = new List<(int, int, int)> 
            {
                (centerX, centerY + i, 0),  // Up
                (centerX, centerY - i, 1),  // Down
                (centerX + i, centerY, 2),  // Right
                (centerX - i, centerY, 3)   // Left
            };
            foreach (var (x, y, direction) in positions)
            {
                if (!canExtend[direction])
                {
                    continue;
                }
                if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
                {
                    ChunkData chunk = chunksArray[x, y];
                    if (chunk != null && !chunk.TileIsLocked())
                    {
                        if (chunk.GetInformationType() == InformationType.Object)
                        {
                            canExtend[direction] = false;
                            continue;
                        }
                        _chunkList.Add(chunk);
                        if (chunk.GetCurrentCharacter() != null)
                        {
                            canExtend[direction] = false;
                        }
                    }
                }
            }
        }
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
            base.ResolveAbility(chunk);
            PlayerInformation playerInformationLocal = chunk.GetCurrentPlayerInformation();
            int bonusDamage = 0;
            if (DoesCharacterHaveBlessing("Release toxins") /*&& playerInformation.Poisons.Count > 0*/)
            {
                bonusDamage = 3;
            }
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
           
            playerInformationLocal.ApplyDebuff("IceSlow");
            if (DoesCharacterHaveBlessing("Chilling shot"))
            {
                playerInformationLocal.ApplyDebuff("IceSlow");
            }
                //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");
            
            DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            FinishAbility();
    }
    public void OnTileHover(Vector3 position)
    {
        // if (IsAllegianceSame(position)) //jei priesas, tada jam ikirs
        // {
        //     var bonusDamage = (DoesCharacterHaveBlessing("Release toxins")/* && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().Poisons.Count > 0*/) ? 3 : 0;
        //     //EnableDamagePreview(position, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        // }
    }
}