using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallSmash : BaseAction
{
    //private string actionStateName = "WallSmash";
    public int damageToWall = 1;
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 6;
    [HideInInspector] GameObject tileForAnimation;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "WallSmash";
        isAbilitySlow = false;
    }
    protected void AddSurroundingsToList(GameObject middleTile, int movementIndex, bool canWallsBeTargeted)
    {
        //base.AddSurroundingsToList(middleTile, movementIndex, true);
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
            bool isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            bool isMiddleTileWall = CheckIfSpecificTag(middleTile, 0, 0, blockingLayer, "Wall");
            if (isGroundLayer && (!isBlockingLayer || isPlayer || isWall) && !isMiddleTileWall)
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    */
    public override void EnableGrid()
    {
        if (CanGridBeEnabled())
        {
            CreateGrid();
            HighlightOuter();
        }
    }
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
    /*
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
        if (canTileBeClicked(position))
        {
            base.ResolveAbility(position);
            if(CheckIfSpecificTag(position, 0, 0, blockingLayer, "Wall"))
            {
               // tileForAnimation = position;
                WallSmashAnimationEnd();
            }
            else if(CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player"))
            {
                DealRandomDamageToTarget(GetSpecificGroundTile(position), minAttackDamage - 2, maxAttackDamage - 2);
            }
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");

            FinishAbility();
        }
    }
    public bool canTileBeClicked(Vector3 position)
    {
        if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Wall") || (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") && !isAllegianceSame(position)))
        {
            return true;
        }
        else return false;
    }
    /*
    public override void OnTileHover(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
            && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().health - damageToWall <= 0)
        {
            tile.transform.Find("mapTile").Find("Death").gameObject.SetActive(true);
            tile.transform.Find("mapTile").Find("DamageText").position = tile.transform.position + new Vector3(0f, 0.65f, 0f);
        }
        tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
        tile.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = "-" + damageToWall.ToString();

        //
        var pushDirectionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };
        foreach (var x in pushDirectionVectors)
        {
            if (CheckIfSpecificLayer(gameObject, x.Item1, x.Item2, groundLayer) && GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer) == tile)
            {
                if (CheckIfSpecificTag(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer, "Player"))
                {
                    if (!GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf) //No text in fog of war
                    {
                        if (GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer).GetComponent<PlayerInformation>().health - damageToPlayer <= 0)
                        {
                            GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("mapTile").Find("Death").gameObject.SetActive(true);
                            GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("mapTile").Find("DamageText").position = GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer).transform.position + new Vector3(0f, 0.65f, 0f);
                        }
                        GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
                        GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = "-" + damageToPlayer.ToString();
                    }
                }
            }
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
            && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().health - damageToWall <= 0)
        {
            tile.transform.Find("mapTile").Find("Death").gameObject.SetActive(false);
            tile.transform.Find("mapTile").Find("DamageText").position = tile.transform.position + new Vector3(0f, 0.50f, 0f);
        }
        tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(false);
        //
        var pushDirectionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };
        foreach (var x in pushDirectionVectors)
        {
            if (CheckIfSpecificLayer(gameObject, x.Item1, x.Item2, groundLayer) && GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer) == tile)
            {
                if (CheckIfSpecificTag(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer, "Player"))
                {
                    if (GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer).GetComponent<PlayerInformation>().health - damageToPlayer <= 0)
                    {
                        GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("mapTile").Find("Death").gameObject.SetActive(false);
                        GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("mapTile").Find("DamageText").position = GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer).transform.position + new Vector3(0f, 0.5f, 0f);
                    }
                    GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("mapTile").Find("DamageText").gameObject.SetActive(false);
                }
            }
        }
    }*/
    public void WallSmashAnimationEnd()
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
            if (CheckIfSpecificLayer(gameObject, x.Item1, x.Item2, groundLayer) && GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer) == tileForAnimation)
            {
                if (CheckIfSpecificTag(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer, "Player"))
                {
                    GameObject target = GetSpecificGroundTile(gameObject, x.Item1 * 2, x.Item2 * 2, blockingLayer);
                    if (!isAllegianceSame(target))
                    {
                        DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                        target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                        target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                        if (DoesCharacterHaveBlessing("Concussion"))
                        {
                            target.GetComponent<PlayerInformation>().Silenced = true;
                        }
                    }
                }
            }
        }
        GetSpecificGroundTile(tileForAnimation, 0, 0, blockingLayer).GetComponent<PlayerInformation>().DealDamage(damageToWall, false, gameObject);
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FakeUpdate();
    }

    public override void OnTileHover(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
        {
            EnableDamagePreview(tile, damageToWall);
            Vector3 direction = tile.transform.position - transform.position;
            if (CheckIfSpecificTag(gameObject, (int)direction.x * 2, (int)direction.y * 2, blockingLayer, "Player") && !isAllegianceSame(GetSpecificGroundTile(gameObject, (int)direction.x * 2, (int)direction.y * 2, blockingLayer)))
            {
                EnableDamagePreview(GetSpecificGroundTile(gameObject, (int)direction.x * 2, (int)direction.y * 2, groundLayer), minAttackDamage, maxAttackDamage);
            }
        }
        else if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(tile))
        {
            EnableDamagePreview(tile, minAttackDamage - 2, maxAttackDamage - 2);
        }
    }

    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile);
        Vector3 direction = tile.transform.position - transform.position;
        if(CheckIfSpecificLayer(gameObject, (int)direction.x * 2, (int)direction.y * 2, groundLayer))
        {
            DisablePreview(GetSpecificGroundTile(gameObject, (int)direction.x * 2, (int)direction.y * 2, groundLayer));
        }
    }
}