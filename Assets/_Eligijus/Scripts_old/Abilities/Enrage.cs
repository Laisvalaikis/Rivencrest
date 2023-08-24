using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enrage : BaseAction
{
    //private string actionStateName = "Enrage";


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "Enrage";
        isAbilitySlow = false;
    }
    /*
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        foreach (var x in directionVectors)
        {
            bool isGroundLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            if (isGroundLayer && (!isBlockingLayer || isPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    */
    /*
    public override void EnableGrid()
    {
        if (canGridBeEnabled())
        {
            CreateGrid();
            HighlightAll();
        }
    }
    public override void CreateGrid()
    {
        transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        if (AttackRange > 0)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, 0);
        }

        for (int i = 1; i <= AttackRange - 1; i++)
        {
            this.AvailableTiles.Add(new List<GameObject>());

            foreach (var tileInPreviousList in this.AvailableTiles[i - 1])
            {
                AddSurroundingsToList(tileInPreviousList, i);
            }
        }
    }
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
                //tile.GetComponent<HighlightTile>().canAbilityTargetYourself = false;
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
            }
        }
    }
    */
    /*
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
    }
    */
    public override void ResolveAbility(Vector3 position)
    {
       
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            GameObject target = GetSpecificGroundTile(position);
            target.GetComponent<GridMovement>().AvailableMovementPoints++;
            GetComponent<GridMovement>().AvailableMovementPoints++;
            GetSpecificGroundTile(target, 0, 0, groundLayer).transform.
                Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("red1");
            GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.
                Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("red1");
            //target.transform.Find("VFX").Find("VFX1x1").GetComponent<Animator>().SetTrigger("heal");
            //transform.Find("VFX").Find("VFX1x1").GetComponent<Animator>().SetTrigger("heal");
            FinishAbility();
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().EnableMovementAction();
        }
    }
    public bool CanTileBeClicked(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && isAllegianceSame(tile))
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "+1 MP");
        EnableTextPreview(GetSpecificGroundTile(gameObject, 0, 0, groundLayer), "+1 MP");
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile);
        DisablePreview(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
    }
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Hostile"))
        {
            AbilityCooldown--;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        Enrage ability = new Enrage();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Hostile") != null)
        {
            ability.AbilityCooldown--;
        }

        return ability;
    }
}
