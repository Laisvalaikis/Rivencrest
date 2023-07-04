using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateFog : BaseAction
{
    //private string actionStateName = "CreateFog";
    public GameObject FogPrefab;
    [HideInInspector] GameObject tileForAnimation;
    private int i = 0;
    private bool isFogActive = false;
    private GameObject spawnedFog;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "CreateFog";
    }
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        base.AddSurroundingsToList(middleTile, movementIndex, true);
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
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
            }
        }
    }
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                bool isBlockingLayer = CheckIfSpecificLayer(tile, 0, 0, blockingLayer);
                bool isPlayer = CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player");
                if (!isBlockingLayer || isPlayer)
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                    tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                    tile.GetComponent<HighlightTile>().activeState = actionStateName;
                    tile.GetComponent<HighlightTile>().ChangeBaseColor();
                }
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
    }
    */
    /*
    public void HighlightOuter()
    {
        if (AttackRange != 0)
        {
            foreach (GameObject tile in this.AvailableTiles[this.AvailableTiles.Count - 1])
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
            for (int i = this.AvailableTiles.Count - 1; i > 0; i--)
            {
                foreach (GameObject tile in this.AvailableTiles[i - 1])
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(false);
                }
            }
            GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
        }
    }
    */
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (isFogActive)
        {
            i++;
            if (i >= 2)
            {
                Destroy(spawnedFog);
                isFogActive = false;
                i = 0;
            }
        }
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            FinishAbility();
            tileForAnimation = clickedTile;
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("createFog");
            spawnedFog = Instantiate(FogPrefab, tileForAnimation.transform.position + new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
            isFogActive = true;
            i = 0;
        }
    }

    public override bool canTileBeClicked(GameObject tile)
    {
        bool isBlockingLayer = CheckIfSpecificLayer(tile, 0, 0, blockingLayer);
        bool isConsumablesLayer = CheckIfSpecificLayer(tile, 0, 0, consumablesLayer);
        bool isFogLayer = CheckIfSpecificLayer(tile, 0, 0, fogLayer);
        if (!isFogLayer)
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        tile.transform.Find("mapTile").Find("Object").gameObject.SetActive(true);
        tile.transform.Find("mapTile").Find("Object").gameObject.GetComponent<SpriteRenderer>().sprite = FogPrefab.GetComponent<SpriteRenderer>().sprite;
    }
}
