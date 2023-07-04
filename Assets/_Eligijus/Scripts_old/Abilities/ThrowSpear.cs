using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowSpear : BaseAction
{
    //private string actionStateName = "ThrowSpear";
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 8;
    public GameObject SpearPrefab;
    //SpearOnBoard;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    void Start()
    {
        laserGrid = true;
        actionStateName = "ThrowSpear";
    }
    /*
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex, int x, int y)
    {
        for (int i = 1; i <= AttackRange; i++)
        {
            //cia visur x ir y dauginama kad pasiektu tuos langelius kurie in range yra 
            bool isGround = CheckIfSpecificLayer(middleTile, x * i, y * i, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x * i, y * i, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Wall");
            if (isGround && (!isBlockingLayer || isPlayer || isWall))
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


    public override void ResolveAbility(GameObject clickedTile)
    {
        if (FindIndexOfTile(clickedTile) != -1)
        {
            base.ResolveAbility(clickedTile);
            int bonusBlessingDamage = 0;
            foreach (GameObject tile in AvailableTiles[FindIndexOfTile(clickedTile)])
            {
               
                if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && (!isAllegianceSame(tile) || friendlyFire)) || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                    if (DoesCharacterHaveBlessing("Fetch"))
                    {
                        bonusBlessingDamage++;
                    }
                }
                tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("purple1");
            }
            //Fetch
            minAttackDamage += bonusBlessingDamage;
            maxAttackDamage += bonusBlessingDamage;
            //
            GameObject LastTile = AvailableTiles[FindIndexOfTile(clickedTile)][AvailableTiles[FindIndexOfTile(clickedTile)].Count - 1];
            spawnedCharacter = Instantiate(SpearPrefab, LastTile.transform.position, Quaternion.identity) as GameObject;
            //isSpearInHand = false;
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
    public override void RefillActionPoints()//pradzioj ejimo
    {
        AvailableAttacks = 1;
        AbilityPoints++;
        if(AbilityPoints >= AbilityCooldown && spawnedCharacter != null)
        {
            Destroy(spawnedCharacter);
            //isSpearInHand = true;
            spawnedCharacter = null;
        }
    }
    public override void SpecificAbilityAction(GameObject character = null) //PickUpSpear
    {
        //isSpearInHand = true;
        AbilityPoints++;
        spawnedCharacter = null;
    }

}