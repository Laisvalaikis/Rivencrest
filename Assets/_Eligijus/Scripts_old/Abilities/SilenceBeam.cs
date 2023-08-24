using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SilenceBeam : BaseAction
{
    //private string actionStateName = "SilenceBeam";
    //public int attackDamage = 25;
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 8;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    private List<GameObject> DamageTiles = new List<GameObject>();
    void Start()
    {
        laserGrid = true;
        actionStateName = "SilenceBeam";
        isAbilitySlow = false;
    }
    /*
    protected override void AddSurroundingsToList(GameObject middleTile, int movementIndex, int x, int y)
    {
        for (int i = 1; i <= AttackRange; i++)
        {
            //cia visur x ir y dauginama kad pasiektu tuos langelius kurie in range yra 
            bool isGround = CheckIfSpecificLayer(middleTile, x * i, y * i, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x * i, y * i, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Player");
            bool isWall = CheckIfSpecificTag(middleTile, x * i, y * i, blockingLayer, "Wall");
            if (isGround && (!isBlockingLayer || isPlayer || isWall))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x * i, y * i, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
            else
            {
                break; //kad neitu kiaurai sienas
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
    public override void CreateGrid()
    {
        transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        this.AvailableTiles.Add(new List<GameObject>());
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        int i = 0;
        foreach (var x in directionVectors)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            AddSurroundingsToList(transform.gameObject, i, x.Item1, x.Item2);
            i++;
        }
    }
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
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
    }
    */
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
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (DamageTiles.Count > 0)
        {
            if (GetComponent<PlayerInformation>().health > 0)
            {
                transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            }
            foreach (GameObject tile in DamageTiles)
            {
                tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("pink1");
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
                {
                    if (!isAllegianceSame(tile) || friendlyFire)
                    {
                        GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                        DealRandomDamageToTarget(target, minAttackDamage+2, maxAttackDamage+2);
                        target.GetComponent<PlayerInformation>().Silenced = true;
                        if (DoesCharacterHaveBlessing("Silent plague"))
                        {
                            target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 1));
                        }
                    }
                }
                tile.transform.Find("mapTile").Find("PinkZone").gameObject.SetActive(false);
            }
            DamageTiles.Clear();
        }
    }

    public override void ResolveAbility(GameObject clickedTile)
    {
        if (FindIndexOfTile(clickedTile) != -1)
        {
            base.ResolveAbility(clickedTile);
            DamageTiles.Clear();
            foreach (GameObject tile in AvailableTiles[FindIndexOfTile(clickedTile)])
            {
                GameObject tileToAdd = GetSpecificGroundTile(tile, 0, 0, groundLayer);
                DamageTiles.Add(tileToAdd);
                tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("pink1");
                if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall")) && (!isAllegianceSame(tile) || friendlyFire))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                    if (DoesCharacterHaveBlessing("Silent plague"))
                    {
                        target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 1));
                    }
                }
                tileToAdd.transform.Find("mapTile").Find("PinkZone").gameObject.SetActive(true);
            }

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
}