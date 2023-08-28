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

    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            GameObject target = GetSpecificGroundTile(position).GetCurrentCharacter();
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
            //clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("rainOfArrows");
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");

            FinishAbility();
        }
    }

    public override bool CanTileBeClicked(Vector3 position)
    {
        if ((CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(position, 0, 0, blockingLayer, "Wall"))
            && !isAllegianceSame(position))
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
        List<GameObject> enemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile.transform.position) && !CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    enemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (enemyCharacterList.Count > 1 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        else if (enemyCharacterList.Count > 0 && actionChanceNumber <= 40)
        {
            return GetSpecificGroundTile(enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
