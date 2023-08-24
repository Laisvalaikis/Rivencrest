using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateBearTrap : BaseAction
{
    //private string actionStateName = "CreateBearTrap";
    public GameObject BearTrapPrefab;
    private int i = 0;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "CreateBearTrap";
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
    */
    /*
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                //bool isBlockingLayer = CheckIfSpecificLayer(tile, 0, 0, blockingLayer);
                //bool isPlayer = CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player");
                //if (!isBlockingLayer) //|| isPlayer)
                //{
                    tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                    tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                    tile.GetComponent<HighlightTile>().activeState = actionStateName;
                    tile.GetComponent<HighlightTile>().ChangeBaseColor();
               // }
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
        if (spawnedCharacter != null)
        {
            i++;
            if (i >= 2)
            {
                Destroy(spawnedCharacter);
                i = 0;
            }
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            FinishAbility();

            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("createFog");
            spawnedCharacter = Instantiate(BearTrapPrefab, position, Quaternion.identity) as GameObject;
            spawnedCharacter.transform.Find("BearTrapCollider").GetComponent<BearTrap>().creator = gameObject;
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
            i = 0;
        }
    }

    public bool CanTileBeClicked(GameObject tile)
    {
        bool isBlockingLayer = CheckIfSpecificLayer(tile, 0, 0, blockingLayer);
        bool isConsumablesLayer = CheckIfSpecificLayer(tile, 0, 0, consumablesLayer);
        if (!isBlockingLayer && !isConsumablesLayer)
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        tile.transform.Find("mapTile").Find("CharacterAlpha").gameObject.SetActive(true);
        tile.transform.Find("mapTile").Find("CharacterAlpha").gameObject.GetComponent<SpriteRenderer>().sprite = BearTrapPrefab.GetComponent<SpriteRenderer>().sprite;
    }
}
