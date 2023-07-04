using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrontSlash : BaseAction
{
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 5;

    void Start()
    {
        laserGrid = true;
        actionStateName = "FrontSlash";
    }

    private int FindIndexOfTile(GameObject tileBeingSearched)
    { //indeksas platesniame liste

        for (int j = 0; j < AvailableTiles.Count; j++)
        {
            for (int i = 0; i < AvailableTiles[j].Count; i++)
            {
                if (AvailableTiles[j][i] == tileBeingSearched)
                {
                    return j;
                }
            }
        }
        return -1;
    }


    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (FindIndexOfTile(clickedTile) != -1)
        {
            base.ResolveAbility(clickedTile);
            foreach (GameObject tile in AvailableTiles[FindIndexOfTile(clickedTile)])
            {

                if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && (!isAllegianceSame(tile) || friendlyFire)) || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
                { 
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                }
                tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("undead4");
                tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("undead6");
            }
           // GameObject LastTile = AvailableTiles[FindIndexOfTile(clickedTile)][AvailableTiles[FindIndexOfTile(clickedTile)].Count - 1];
           
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            FinishAbility();
        }
    }
    public override bool canTileBeClicked(GameObject tile)
    {
        return true;
    }
    public override void OnTileHover(GameObject tile)
    {
        if (FindIndexOfTile(tile) != -1)
        {
            EnableDamagePreview(tile, AvailableTiles[FindIndexOfTile(tile)], minAttackDamage, maxAttackDamage);
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        if (FindIndexOfTile(tile) != -1)
        {
            DisablePreview(tile, AvailableTiles[FindIndexOfTile(tile)]);
        }
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(tile))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}