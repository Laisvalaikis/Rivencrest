using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaserBeam : BaseAction
{
    //public int minAttackDamage = 2;
    //public int maxAttackDamage = 4;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    void Start()
    {
        actionStateName = "LaserBeam";
        laserGrid = true;
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
                GetSpecificGroundTile(tile, 0, 0, groundLayer).transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("orange2");
                GetSpecificGroundTile(tile, 0, 0, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("orange4");
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    target.GetComponent<PlayerInformation>().Aflame = gameObject;
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                }
            }
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            FinishAbility();
        }
    }
    public bool canTileBeClicked(GameObject tile)
    {
        return true;//CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall");
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

    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Rapid fire"))
        {
            isAbilitySlow = false;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        LaserBeam ability = new LaserBeam();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Rapid fire") != null)
        {
            ability.isAbilitySlow = false;
        }

        return ability;
    }
}