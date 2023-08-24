using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FreezeAbility : BaseAction
{
    //private string actionStateName = "IceFreeze";

    //public int spellDamage = 40;
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 8;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    void Start()
    {
        actionStateName = "IceFreeze";
        //AttackAbility = true;
    }
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        var directionVectors = new List<(int, int)> //cube around player
        {
            (1, 0),
            (1, 1),
            (0, 1),
            (-1, 1),
            (-1, 0),
            (-1, -1),
            (0, -1),
            (1, -1)
        };

        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            if (isGround && (!isBlockingLayer || isPlayer || isWall))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    /*
    public override void EnableGrid()
    {
        if (canGridBeEnabled())
        {
            CreateGrid();
            HighlightAll();
        }
    }
    */
    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        if (AttackRange > 0)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, 0);
        }
        MergeIntoOneList();
    }
    /*
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
            }
        }
    }
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true; //kad galetu paspaust ir ant saviskio ir vis tiek pasileis
            }
        }
    }
    */
    public override void ResolveAbility(GameObject clickedTile)
    {
        base.ResolveAbility(clickedTile);
        transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
        transform.Find("VFX").Find("VFX9x9").gameObject.GetComponent<Animator>().SetTrigger("iceFreeze");
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") ||
                    CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall")) //visiem stovintiems ant kvadrato tilesu, turinciu player arba wall tagga ikerta
                {
                    if (!isAllegianceSame(tile) || friendlyFire)
                    {
                        GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                        DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && target.GetComponent<PlayerInformation>().health > 0)
                        {
                            target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                            if (DoesCharacterHaveBlessing("Blizzard") && !isAllegianceSame(tile))//Blessing
                            {
                                target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                            }
                        }
                    }

                }
            }
        }
        FinishAbility();
        if (DoesCharacterHaveBlessing("Ice skates"))
        {
            GetComponent<GridMovement>().AvailableMovementPoints++;
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            EnableDamagePreview(tile, MovementTileList, minAttackDamage, maxAttackDamage);
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            DisablePreview(tile, MovementTileList);
        }
    }
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Frostbite"))
        {
            minAttackDamage += 2;
            maxAttackDamage += 2;
        }
        if(DoesCharacterHaveBlessing("Friendly frost"))
        {
            friendlyFire = false;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        FreezeAbility ability = new FreezeAbility();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Frostbite") != null)
        {
            ability.minAttackDamage += 2;
            ability.maxAttackDamage += 2;
        }
        if (blessings.Find(x => x.blessingName == "Friendly frost") != null)
        {
            ability.friendlyFire = false;
        }

        return ability;
    }
}
