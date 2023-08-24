using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePortal : BaseAction
{
    //private string actionStateName = "CreatePortal";
    

    public GameObject PortalPrefab;
    private bool isPortalActive = false;

    private GameObject portalExit;
    private GameObject portalEntrance;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "CreatePortal";
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
        if (isPortalActive)
        {
            Destroy(portalExit);
            Destroy(portalEntrance);
            isPortalActive = false;
        }
    }

    public override void ResolveAbility(GameObject clickedTile)
    {
        if (!CheckIfSpecificLayer(gameObject, 0, 0, portalLayer) && !CheckIfSpecificLayer(clickedTile, 0, 0, portalLayer))
        {
            base.ResolveAbility(clickedTile);
            portalExit = Instantiate(PortalPrefab, clickedTile.transform.position + new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
            portalEntrance = Instantiate(PortalPrefab, transform.position + new Vector3(0f, 0f, 2f), Quaternion.identity) as GameObject;
            portalExit.GetComponent<Portal>().OtherPortalExit = portalEntrance;
            portalEntrance.GetComponent<Portal>().OtherPortalExit = portalExit;
            isPortalActive = true;


            FinishAbility();

        }

    }
    public override void OnTileHover(GameObject tile)
    {
        tile.transform.Find("mapTile").Find("Object").gameObject.SetActive(true);
        tile.transform.Find("mapTile").Find("Object").gameObject.GetComponent<SpriteRenderer>().sprite = PortalPrefab.GetComponent<SpriteRenderer>().sprite;
        GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("Object").gameObject.SetActive(true);
        GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("Object").gameObject.GetComponent<SpriteRenderer>().sprite = PortalPrefab.GetComponent<SpriteRenderer>().sprite;
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile);
        GetSpecificGroundTile(gameObject, 0, 0, groundLayer).transform.Find("mapTile").Find("Object").gameObject.SetActive(false);
    }
}
