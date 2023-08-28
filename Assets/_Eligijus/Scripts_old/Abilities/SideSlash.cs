using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SideSlash : BaseAction
{
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 5;


    void Start()
    {
        actionStateName = "SideSlash";
    }

    private void AddSurroundingsToList(GameObject middleTile)
    {
        int directionIndex = 0;
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        foreach (var x in directionVectors)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            bool isGroundLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isTargetGroundLayer = CheckIfSpecificLayer(middleTile, x.Item1 * 2, x.Item2 * 2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            bool isTargetBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1 * 2, x.Item2 * 2, blockingLayer);
            bool isTargetPlayer = CheckIfSpecificTag(middleTile, x.Item1 * 2, x.Item2 * 2, blockingLayer, "Player");
            if (isGroundLayer && (!isBlockingLayer || isPlayer) && isTargetGroundLayer && (!isTargetBlockingLayer || isTargetPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1 * 2, x.Item2 * 2, groundLayer);
                bool isDirectionFromPlayerHorizontal = x.Item2 == 0;
                this.AvailableTiles[directionIndex].Add(AddableObject);
                AddAdjacentTiles(AddableObject, isDirectionFromPlayerHorizontal, directionIndex);
            }
            directionIndex++;
        }
    }

    private void AddAdjacentTiles(GameObject centerTile, bool isDirectionFromPlayerHorizontal, int directionIndex)
    {
        List<(int, int)> directionVectors;
        if (isDirectionFromPlayerHorizontal)
        {
            directionVectors = new List<(int, int)> { (0, 1), (0, -1) };
        }
        else
        {
            directionVectors = new List<(int, int)> { (1, 0), (-1, 0) };
        }
        foreach (var x in directionVectors)
        {
            bool isGroundLayer = CheckIfSpecificLayer(centerTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(centerTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(centerTile, x.Item1, x.Item2, blockingLayer, "Player");
            if (isGroundLayer && (!isBlockingLayer || isPlayer))
            {
                GameObject AddableObject = GetSpecificGroundTile(centerTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[directionIndex].Add(AddableObject);
            }
        }
    }

    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        AddSurroundingsToList(transform.gameObject);
        //MergeIntoOneList();
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
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && (!isAllegianceSame(tile) || friendlyFire))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);

                }
                tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("undead6");
                tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("undead5");
            }
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            FinishAbility();
        }
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
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(tile))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
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
