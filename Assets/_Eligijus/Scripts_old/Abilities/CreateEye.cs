using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEye : BaseAction
{
    //private string actionStateName = "CreateEye";
    

    public GameObject EyePrefab;
    private bool isEyeActive = false;

    private GameObject createdEye;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "CreateEye";
        isAbilitySlow = false;
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
                GameObject addableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(addableObject);
            }
        }
    }
    /*
    public override void EnableGrid()
    {
        if (AvailableAttacks != 0 && AbilityPoints >= AbilityCooldown)
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
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
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
    }
    */
    public override void OnTurnEnd()
    {
        RefillActionPoints();
        if (isEyeActive)
        {
            createdEye.GetComponent<CharacterVision>().DisableGrid();
            Destroy(createdEye);
            GetComponent<PlayerInformation>().VisionGameObject = null;
            isEyeActive = false;
        }
    }

    public override void ResolveAbility(Vector3 position)
    {
            base.ResolveAbility(position);
            createdEye = Instantiate(EyePrefab, position + new Vector3(0f, 0f, 1f), Quaternion.identity);
            createdEye.GetComponent<CharacterVision>().EnableGrid();
            GetComponent<PlayerInformation>().VisionGameObject = createdEye;
            isEyeActive = true;


            FinishAbility();

    }
    public override void OnTileHover(GameObject tile)
    {
        tile.transform.Find("mapTile").Find("Object").gameObject.SetActive(true);
        tile.transform.Find("mapTile").Find("Object").gameObject.GetComponent<SpriteRenderer>().sprite = EyePrefab.GetComponent<SpriteRenderer>().sprite;
    }
}
