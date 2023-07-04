using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerShot : BaseAction
{
    //private string actionStateName = "PowerShot";
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 7;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    void Start()
    {
        laserGrid = true;
        actionStateName = "PowerShot";
    }
    protected override void AddSurroundingsToList(GameObject middleTile, int movementIndex, int x, int y)
    {
        for (int i = 1; i <= AttackRange; i++)
        {
            //cia visur x ir y dauginama kad pasiektu tuos langelius kurie in range yra 
            bool isGround = CheckIfSpecificLayer(middleTile, x * i, y * i, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x * i, y * i, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Wall");

            bool isMiddleTileBlocked = false;
            if (i > 1)
            {
                isMiddleTileBlocked = CheckIfSpecificLayer(middleTile, x * (i - 1), y * (i - 1), blockingLayer);
            }
            if (isGround && (!isBlockingLayer || isPlayer) && (!isMiddleTileBlocked))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x * i, y * i, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
            else
            {
                break; //kad neitu kiaurai sienas
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
    /*
    public override void CreateGrid()
    {
        transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        this.AvailableTiles.Add(new List<GameObject>());
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        int i = 0;
        foreach (var x in directionVectors)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, i, x.Item1, x.Item2);
            i++;
        }
    }
    */
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

    public override void HighlightAll()
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
    }
    */
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
    public override bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")) && !isAllegianceSame(tile))
        {
                return true;
        }
        return false;
    }

    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            int bonusDamage = 0;
            if (DoesCharacterHaveBlessing("Release toxins") &&
                target.GetComponent<PlayerInformation>().Poisons.Count > 0)
            {
                bonusDamage = 3;
            }
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
           
            target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
            if (DoesCharacterHaveBlessing("Chilling shot"))
            {
                target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
            }
                clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("green1");
            //
            DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            FinishAbility();
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        if (!isAllegianceSame(tile)) //jei priesas, tada jam ikirs
        {
            var bonusDamage = (DoesCharacterHaveBlessing("Release toxins") && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().Poisons.Count > 0) ? 3 : 0;
            EnableDamagePreview(tile, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        }
    }

    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            CreateGrid();
            foreach (List<GameObject> MovementTileList in this.AvailableTiles)
            {
                foreach (GameObject tile in MovementTileList)
                {
                        if (canTileBeClicked(tile))
                        {
                            GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                            EnemyCharacterList.Add(character);
                        }
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