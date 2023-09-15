using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerShot : BaseAction
{
    //private string actionStateName = "PowerShot";
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 7;

    void Start()
    {
        laserGrid = true;
        actionStateName = "PowerShot";
    }
    public override void ResolveAbility(Vector3 position)
    {
            base.ResolveAbility(position);
            ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
            PlayerInformation playerInformation = chunk.GetCurrentPlayerInformation();
            int bonusDamage = 0;
            if (DoesCharacterHaveBlessing("Release toxins") /*&& playerInformation.Poisons.Count > 0*/)
            {
                bonusDamage = 3;
            }
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
           
            playerInformation.ApplyDebuff("IceSlow");
            if (DoesCharacterHaveBlessing("Chilling shot"))
            {
                playerInformation.ApplyDebuff("IceSlow");
            }
                //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");
            
            DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            FinishAbility();
    }
    public override void OnTileHover(GameObject tile)
    {
        if (!isAllegianceSame(tile)) //jei priesas, tada jam ikirs
        {
            var bonusDamage = (DoesCharacterHaveBlessing("Release toxins")/* && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().Poisons.Count > 0*/) ? 3 : 0;
            EnableDamagePreview(tile, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        }
    }
}