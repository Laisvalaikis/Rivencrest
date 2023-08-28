using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Purify : BaseAction
{
    //private string actionStateName = "Purify";
    private GameObject target;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "Purify";
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
        //Merging into one list
        MergedTileList.Clear();
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!MergedTileList.Contains(tile))
                {
                    MergedTileList.Add(tile);
                }
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
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = false;
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
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(true);
    }
    */
    protected override void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(true);
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            target = GetSpecificGroundTile(position);
            if (DoesCharacterHaveBlessing("Enlighten"))
            {
                int randomHeal = Random.Range(3, 5);
                bool crit = IsItCriticalStrike(ref randomHeal);
                target.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
            }
            target.GetComponent<PlayerInformation>().Marker = null;
            if (target.GetComponent<PlayerInformation>().Debuffs.Contains("Stun"))
            {
                target.GetComponent<PlayerInformation>().Debuffs.Remove("Stun"); //RemoveAll
                target.transform.Find("VFX").Find("VFXStun").gameObject.SetActive(false);
            }
            target.GetComponent<GridMovement>().RemoveDebuff("Slows");
            target.GetComponent<PlayerInformation>().Disarmed = false;
            target.GetComponent<PlayerInformation>().CantMove = false;
            target.GetComponent<PlayerInformation>().Silenced = false;
            target.GetComponent<PlayerInformation>().Aflame = null;
            target.GetComponent<PlayerInformation>().Poisons.Clear();
            target.transform.Find("VFX").Find("VFX1x1").GetComponent<Animator>().SetTrigger("purify");

            FinishAbility();
        }
    }
    public override bool CanTileBeClicked(Vector3 position)
    {
        if ((CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player")) && isAllegianceSame(position))
        {
            GameObject tileTarget = GetSpecificGroundTile(position);
            bool poisoned = tileTarget.GetComponent<PlayerInformation>().Poisons.Count > 0;
            bool stunned = tileTarget.GetComponent<PlayerInformation>().Debuffs.Contains("Stun");
            bool slowed1 = tileTarget.GetComponent<PlayerInformation>().Slow1;
            bool slowed2 = tileTarget.GetComponent<PlayerInformation>().Slow2;
            bool slowed3 = tileTarget.GetComponent<PlayerInformation>().Slow3;
            bool cantMove = tileTarget.GetComponent<PlayerInformation>().CantMove;
            bool silenced = tileTarget.GetComponent<PlayerInformation>().Silenced;
            bool aflame = tileTarget.GetComponent<PlayerInformation>().Aflame != null;
            bool disarmed = tileTarget.GetComponent<PlayerInformation>().Disarmed;
            if (DoesCharacterHaveBlessing("Enlighten") || (poisoned || stunned || slowed1 || slowed2 || slowed3 || cantMove || silenced || aflame || disarmed))
            {
                return true;
            }
        }
        return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "PURIFY");
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> enemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile.transform.position))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    enemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (enemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Distant remedy"))
        {
            AttackRange++;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        Purify ability = target.AddComponent<Purify>();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Distant remedy") != null)
        {
            ability.AttackRange++;
        }

        return ability;
    }
}
