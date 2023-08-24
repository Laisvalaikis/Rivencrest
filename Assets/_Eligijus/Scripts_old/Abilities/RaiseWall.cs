using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseWall : BaseAction
{
    //private string actionStateName = "RaiseWall";

    public GameObject WallPrefab;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "RaiseWall";
    }
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        var directionVectors = new List<(int, int)>
        {
            (AttackRange, 0),
            (0, AttackRange),
            (-AttackRange, 0),
            (0, -AttackRange)
        };

        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            if (isGround && (!isBlockingLayer || isPlayer))
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
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, 0);
        MergeIntoOneList();
    }
    /*
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
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
            }
        }
    }
    */
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        FinishAbility();
        GameObject spawnedWall = Instantiate(WallPrefab, position + new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
        if (transform.position.x == position.x)
        {
            IntantiateNeighborWalls(position, new List<(int, int)> { (1, 0), (-1, 0) }); //jei paspausto langelio ir characterio x lygus, tai sonines sienos bus i sonus
        }
        else
        {
            IntantiateNeighborWalls(position, new List<(int, int)> { (0, 1), (0, -1) }); //o jei y lygus, tai i virsu ir i apacia
        }
    }

    private void IntantiateNeighborWalls(Vector3 position, List<(int, int)> directionVectorsX)
    {
        foreach (var x in directionVectorsX)
        {
            if (!CheckIfSpecificLayer(position, x.Item1, x.Item2, blockingLayer))
            {
                Instantiate(WallPrefab, position + new Vector3(x.Item1, x.Item2, 1f), Quaternion.identity);
            }
        }
    }
}
