using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Avalanche : BaseAction
{
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;

    void Start()
    {
        actionStateName = "Avalanche";
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                    target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                    tile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("avalanche");
                }

            }
            FinishAbility();
        }
    }
    public override bool canTileBeClicked(GameObject tile)
    {
        //foreach (GameObject tileInList in MergedTileList)

            if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer)) && isCharacterAffectedByCrowdControl(tile))
            {
                return true;
            }

        return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
    private bool isCharacterAffectedByCrowdControl(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
        {
            GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
            if (target.GetComponent<PlayerInformation>().Slow1 
                || target.GetComponent<PlayerInformation>().Slow2 
                || target.GetComponent<PlayerInformation>().Slow3
                || target.GetComponent<PlayerInformation>().Debuffs.Contains("Stun")
                || target.GetComponent<PlayerInformation>().Silenced
                || target.GetComponent<PlayerInformation>().CantMove)
            {
                return true;
            }
        }
        return false;
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 1 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        else if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 50)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
