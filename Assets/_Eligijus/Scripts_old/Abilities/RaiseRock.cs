using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaiseRock : BaseAction
{
    //private string actionStateName = "RaiseRock";
    public GameObject WallPrefab;
    [HideInInspector] GameObject tileForAnimation;
    private bool canPreviewBeShown = true;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "RaiseRock";
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
            bool isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            bool isMiddleTileWall = CheckIfSpecificTag(middleTile, 0, 0, blockingLayer, "Wall");
            if (isGroundLayer && (!isBlockingLayer || isPlayer || isWall) && !isMiddleTileWall)
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    */
   /* public override void EnableGrid()
    {
        if (canGridBeEnabled())
        {
            CreateGrid();
            HighlightOuter();
        }
        else
        {
            transform.gameObject.GetComponent<PlayerInformation>().currentState = "Movement";
        }

    }*/
    /*
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
                if (!isBlockingLayer)
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                    tile.GetComponent<HighlightTile>().activeState = actionStateName;
                    tile.GetComponent<HighlightTile>().ChangeBaseColor();
                }
            }
        }
    }
    */
    public void HighlightOuter()
    {
        if (AttackRange != 0)
        {
            foreach (GameObject tile in this.AvailableTiles[this.AvailableTiles.Count - 1])
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
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
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        if (canTileBeClicked(position))
        {
            FinishAbility();
           // tileForAnimation = clickedTile;
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            GameObject spawnedWall = Instantiate(WallPrefab, tileForAnimation.transform.position + new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
            if (DoesCharacterHaveBlessing("Grand entrance"))
            {
                DealDamageToAdjacent(position);
            }
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
            spawnedWall.transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
            // transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("raiseWall");
        }
    }

    private void DealDamageToAdjacent(Vector3 position)
    {
        var pushDirectionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };
        foreach (var x in pushDirectionVectors)
        {
            if (CheckIfSpecificLayer(position, x.Item1, x.Item2, groundLayer)) //animation on ground
            {
                GetSpecificGroundTile(position).GetCurrentCharacter().transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("burgundy3");
            }
            if (CheckIfSpecificTag(position, x.Item1, x.Item2, blockingLayer, "Player"))
            {
                GameObject target = GetSpecificGroundTile(position).GetCurrentCharacter();

                if (!isAllegianceSame(target))
                {
                    DealRandomDamageToTarget(target, 2, 3);
                }
            }
        }
    }

    private void SetHighlightAdjacent(GameObject center, bool value)
    {
        var pushDirectionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };
        foreach (var x in pushDirectionVectors)
        {
            if (CheckIfSpecificTag(center, x.Item1, x.Item2, blockingLayer, "Player"))
            {
                GameObject target = GetSpecificGroundTile(center, x.Item1, x.Item2, groundLayer);

                if (!isAllegianceSame(target))
                {
                    target.transform.Find("mapTile").Find("Highlight").gameObject.SetActive(value);
                }
            }
        }
    }

    public bool canTileBeClicked(Vector3 position)
    {
        bool isBlockingLayer = CheckIfSpecificLayer(position, 0, 0, blockingLayer);
        bool isConsumablesLayer = CheckIfSpecificLayer(position, 0, 0, consumablesLayer);
        if (!isBlockingLayer && !isConsumablesLayer)
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        if (canPreviewBeShown)
        {
            tile.transform.Find("mapTile").Find("Object").gameObject.SetActive(true);
            tile.transform.Find("mapTile").Find("Object").gameObject.GetComponent<SpriteRenderer>().sprite = WallPrefab.transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sprite;
            if(DoesCharacterHaveBlessing("Grand entrance"))
            {
                SetHighlightAdjacent(tile, true);
            }
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile);
        SetHighlightAdjacent(tile, false);
        canPreviewBeShown = false;
        StartCoroutine(ExecuteAfterFrames(1, () =>
        {
            canPreviewBeShown = true;
        }));
    }
    public void RaiseRockAnimationEnd()
    {
        GameObject spawnedWall = Instantiate(WallPrefab, tileForAnimation.transform.position + new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
    }
}
