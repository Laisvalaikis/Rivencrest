using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttack : BaseAction
{
    public string impactVFXName = "";
    //public int minAttackDamage = 6;
    //public int maxAttackDamage = 12;

    void Start()
    {
        actionStateName = "Attack";
        AttackAbility = true;
    }

    protected void AddSurroundingsToList(GameObject middleTile, int movementIndex, bool canWallsBeTargeted)
    {
        //base.AddSurroundingsToList(middleTile, movementIndex, true);
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        ChunkData chunkData = GameTileMap.Tilemap.GetChunk(position);
        DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
        Debug.LogError("FIX FINISH ABILITY");
        FinishAbility();
    }

    // public override void ResolveAbility(GameObject clickedTile)
    // {
    //     
    //     if (canTileBeClicked(clickedTile))
    //     {
    //         base.ResolveAbility(clickedTile);
    //         GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
    //         transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
    //         if (impactVFXName != "")
    //         {
    //             GetSpecificGroundTile(clickedTile, 0, 0, groundLayer).transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger(impactVFXName);
    //         }
    //         DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);    
    //         //Blessing
    //         if (DoesCharacterHaveBlessing("Freezing touch"))
    //         {
    //             target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
    //         }
    //         if (DoesCharacterHaveBlessing("Venomous touch"))
    //         {
    //             target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 1, 2));
    //         }
    //         if (DoesCharacterHaveBlessing("Molten touch"))
    //         {
    //             target.GetComponent<PlayerInformation>().Aflame = gameObject;
    //         }
    //         if (DoesCharacterHaveBlessing("Vampiric touch"))
    //         {
    //             GetComponent<PlayerInformation>().Heal(3, false);
    //         }
    //         //Finish
    //         FinishAbility();
    //     }
    // }

    public bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
            && !isAllegianceSame(tile.transform.position) && !GetComponent<PlayerInformation>().CantAttackCondition)
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }

    //// NOT RELATED AFLAME EXPLODE
    //public override void SpecificAbilityAction(GameObject aflameCharacter) //Explode
    //{
    //    if (aflameCharacter != null && aflameCharacter.GetComponent<PlayerInformation>().Aflame != null && aflameCharacter.GetComponent<PlayerInformation>().health > 0)
    //    {
    //        var directionVectors = new List<(int, int)>
    //    {
    //        (1, 0),
    //        (0, 1),
    //        (-1, 0),
    //        (0, -1)
    //    };
    //        aflameCharacter.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetTrigger("explode");
    //        aflameCharacter.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetBool("aflame", false);
    //        foreach (var x in directionVectors)
    //        {
    //            bool isPlayer = CheckIfSpecificTag(aflameCharacter, x.Item1, x.Item2, blockingLayer, "Player");
    //            if (isPlayer)
    //            {
    //                GameObject target = GetSpecificGroundTile(aflameCharacter, x.Item1, x.Item2, blockingLayer);
    //                if (isAllegianceSame(aflameCharacter, target, blockingLayer))
    //                {
    //                    DealRandomDamageToTarget(target, 2, 2);
    //                }
    //            }
    //        }
    //        DealRandomDamageToTarget(aflameCharacter, 2, 2);
    //        aflameCharacter.GetComponent<PlayerInformation>().Aflame = null;
    //    }
    //}
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Sharp blade"))
        {
            minAttackDamage += 2;
            maxAttackDamage += 2;
        }
        isAbilitySlow = !DoesCharacterHaveBlessing("Agility");
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        PlayerAttack ability = new PlayerAttack();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Sharp blade") != null)
        {
            ability.minAttackDamage += 2;
            ability.maxAttackDamage += 2;
        }

        ability.isAbilitySlow = blessings.Find(x => x.blessingName == "Agility") == null;

        return ability;
    }
}
