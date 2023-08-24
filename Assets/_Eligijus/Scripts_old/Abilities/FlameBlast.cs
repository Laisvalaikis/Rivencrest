using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlameBlast : BaseAction
{
    //private string actionStateName = "FlameBlast";
    //public int minAttackDamage = 2;
    //public int maxAttackDamage = 4;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    private List<GameObject> DamageTiles = new List<GameObject>();
    private List<GameObject> CometTiles = new List<GameObject>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "FlameBlast";
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
    */
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
        //
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
            MergedTileList.Remove(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
        }
    }
    */
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
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
    }
    */
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (CometTiles.Count > 0)
        {
            foreach (GameObject tile in CometTiles)
            {
                tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("orange2");
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && (!isAllegianceSame(tile) || friendlyFire))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage+3, maxAttackDamage+3);
                }
                tile.transform.Find("mapTile").Find("OrangeZone").gameObject.SetActive(false);
            }
            CometTiles.Clear();
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
        CometTiles.Clear();
        CreateDamageTileList(position);
        foreach (GameObject tile in DamageTiles)
        {
            if (tile == DamageTiles[0])
            {
                tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("orange3");
                CometTiles.Add(tile);
                tile.transform.Find("mapTile").Find("OrangeZone").gameObject.SetActive(true);
            }
            if (tile != DamageTiles[0])
            {
                tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("orange1");
                if(DoesCharacterHaveBlessing("Meteor rain"))
                {
                    CometTiles.Add(tile);
                    tile.transform.Find("mapTile").Find("OrangeZone").gameObject.SetActive(true);
                }
            }
            if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && (!isAllegianceSame(tile) || friendlyFire))
            {
                GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            }
        }
        FinishAbility();
    }

    public void CreateDamageTileList(Vector3 position)
    {
        DamageTiles.Clear();
        var spellDirectionVectors = new List<(int, int)>
        {
            (0, 0),
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        foreach (var x in spellDirectionVectors)
        {
            if (CheckIfSpecificLayer(position, x.Item1, x.Item2, groundLayer) && (!CheckIfSpecificLayer(position, x.Item1, x.Item2, blockingLayer) || CheckIfSpecificTag(position, x.Item1, x.Item2, blockingLayer, "Player")))
            {
                DamageTiles.Add(GetSpecificGroundTile(position));
            }
        }

    }
    public void OnTileHover(Vector3 position) //pakeisti veliau i public override void
    {
        print("on tile hover");
        CreateDamageTileList(position);
        foreach (GameObject tileInList in DamageTiles)
        {
            if (CheckIfSpecificTag(tileInList, 0, 0, blockingLayer, "Player") && (!isAllegianceSame(tileInList) || friendlyFire))
            {
                if (!tileInList.GetComponent<HighlightTile>().FogOfWarTile.activeSelf)//No text preview in fog of war
                {
                    if (GetSpecificGroundTile(tileInList, 0, 0, blockingLayer).GetComponent<PlayerInformation>().health - minAttackDamage <= 0)
                    {
                        tileInList.transform.Find("mapTile").Find("Death").gameObject.SetActive(true);
                        tileInList.transform.Find("mapTile").Find("DamageText").position = tileInList.transform.position + new Vector3(0f, 0.65f, 0f);
                    }
                    tileInList.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
                    tileInList.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = minAttackDamage.ToString() + "-" + maxAttackDamage.ToString();
                }
            }
            if (tileInList == DamageTiles[0])
            {
                tileInList.GetComponent<HighlightTile>().HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = GameObject.Find("GameInformation").GetComponent<ColorManager>().MovementHighlightHover;//tileInList.GetComponent<HighlightTile>().HoverHighlightColor;
                //tile.transform.Find("mapTile").Find("Direction").gameObject.SetActive(true);
            }
            if (tileInList != DamageTiles[0])
                tileInList.transform.Find("mapTile").Find("Highlight").gameObject.SetActive(true);
            //tile.transform.Find("mapTile").Find("Object").gameObject.GetComponent<SpriteRenderer>().sprite = EyePrefab.GetComponent<SpriteRenderer>().sprite;
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        print("off tile hover");
        tile.transform.Find("mapTile").Find("Direction").gameObject.SetActive(false);
        foreach (GameObject tileInList in DamageTiles)
        {
            DisablePreview(tileInList);
        }
    }
    public override GameObject PossibleAIActionTile()
    {
        if (CanGridBeEnabled())
        {
            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(AttackRange);

            List<GameObject> enemyCharacterList = new List<GameObject>();

            foreach (GameObject character in characterList)
            {
                if (!isAllegianceSame(character))
                {
                    enemyCharacterList.Add(character);
                }
            }
            if (enemyCharacterList.Count > 0)
            {
                return enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)];
            }
        }

        return null;
    }
}
