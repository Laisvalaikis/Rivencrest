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

    public override void CreateGrid()
    {
        base.CreateGrid();
        for (int i = AvailableTiles.Count - 2; i >= 0; i--)
        {
            foreach (GameObject tile in AvailableTiles[i])
            {
                MergedTileList.Remove(tile);
            }
        }
    }

    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            if (DoesCharacterHaveBlessing("Poisonous shot"))
            {
                target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 1));
            }
            if (DoesCharacterHaveBlessing("Caught one"))
            {
                target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
            }
            clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("rainOfArrows");
            clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");

            FinishAbility();
        }
    }

    public bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
            && !isAllegianceSame(tile))
        {
            return true;
        }
        return false;
    }

    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }

    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile) && !CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
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
        else if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 40)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
