using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpearPulse : BaseAction
{
    //private string actionStateName = "SpearPulse";
    //public int attackDamage = 60;
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();
    private GameObject startingPoint;

    void Start()
    {
        actionStateName = "SpearPulse";
        //AttackAbility = true;
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
            bool isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            if (isGroundLayer && (!isBlockingLayer || isPlayer || isWall))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
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
    */
    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        startingPoint = gameObject;
        if (GetComponent<ActionManager>().FindActionByName("ThrowSpear") != null && GetComponent<ActionManager>().FindActionByName("ThrowSpear").spawnedCharacter != null)
        {
            startingPoint = GetComponent<ActionManager>().FindActionByName("ThrowSpear").spawnedCharacter;
        }
        if (AttackRange > 0)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            
            if (CheckIfSpecificLayer(startingPoint, 0, 0, groundLayer))
            {
                AvailableTiles[0].Add(GetSpecificGroundTile(startingPoint, 0, 0, groundLayer));
            }
            AddSurroundingsToList(startingPoint, 0);
        }

        for (int i = 1; i <= AttackRange - 1; i++)
        {
            this.AvailableTiles.Add(new List<GameObject>());

            foreach (var tileInPreviousList in this.AvailableTiles[i - 1])
            {
                AddSurroundingsToList(tileInPreviousList, i);
            }
        }
        /*
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
        if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
        {
            MergedTileList.Remove(GetSpecificGroundTile(startingPoint, 0, 0, groundLayer));
        }
        */
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
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = false;
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
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
    }
    */
    public override void ResolveAbility(GameObject clickedTile)
    {
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            // transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            foreach (GameObject tile in MergedTileList)
            {
                if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(tile)) || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                }
                tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("purple1");
            }
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
            FinishAbility();
        }
    }
    public bool canTileBeClicked(GameObject tile)
    {
        return true;
        /* foreach (GameObject tileInList in MergedTileList)
         {
             if (CheckIfSpecificTag(tileInList, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tileInList, 0, 0, blockingLayer, "Wall"))
             {
                 return true;
             }
         }

        return false;*/
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
        if (CheckIfSpecificLayer(startingPoint, 0, 0, groundLayer))
        {
            GetSpecificGroundTile(startingPoint, 0, 0, groundLayer).GetComponent<HighlightTile>().HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = 
            GetSpecificGroundTile(startingPoint, 0, 0, groundLayer).GetComponent<HighlightTile>().NotHoveredColor;
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile))
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
