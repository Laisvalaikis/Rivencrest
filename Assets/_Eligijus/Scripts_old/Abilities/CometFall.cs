using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometFall : BaseAction
{
    //private string actionStateName = "CometFall";
    //public int spellDamage = 50;
    //public int minAttackDamage = 8;
    //public int maxAttackDamage = 10;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    private List<GameObject> DamageTiles = new List<GameObject>();

    //
    private List<List<GameObject>> AIGridTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "CometFall";
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
        if (DamageTiles.Count > 0)
        {
            foreach (GameObject tile in DamageTiles)
            {
                tile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("cometFall");
                //Enemy
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer)))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                }
                //Ally
                else if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer)))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage/3, maxAttackDamage/3);
                }
                tile.transform.Find("mapTile").Find("CometZone").gameObject.SetActive(false);
            }
            DamageTiles.Clear();
        }
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        base.ResolveAbility(clickedTile);
        transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1"); //CometFallStart animation
        DamageTiles.Clear();
        if (CheckIfSpecificLayer(clickedTile, 0, 0, groundLayer))
        {
            DamageTiles.Add(GetSpecificGroundTile(clickedTile, 0, 0, groundLayer));
            GetSpecificGroundTile(clickedTile, 0, 0, groundLayer).transform.Find("mapTile").Find("CometZone").gameObject.SetActive(true);
        }

        FinishAbility();
    }
    public override bool canTileBeClicked(GameObject tile)
    {
        return CheckIfSpecificLayer(tile, 0, 0, groundLayer);
    }
    public override GameObject PossibleAIActionTile()
    {
        if (canGridBeEnabled())
        {

            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(2);

            List<GameObject> enemyCharacterList = new List<GameObject>();
            List<GameObject> TilesNearEnemyList = new List<GameObject>();

            foreach (GameObject character in characterList)
            {
                if (!isAllegianceSame(character) && CheckIfSpecificLayer(character, 0, 0, groundLayer)
                    && !GetSpecificGroundTile(character, 0, 0, groundLayer).transform.Find("mapTile").Find("CometZone").gameObject.activeSelf)
                {
                    enemyCharacterList.Add(character);
                }
            }
            if (enemyCharacterList.Count > 0)
            {
                return enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)];
            }
            //Not directly on enemy
            CreateAIGrid();
            var directionVectors = new List<(int, int)>
                {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
                };


            foreach (List<GameObject> MovementTileList in this.AIGridTiles)
            {
                foreach (GameObject tile in MovementTileList)
                {
                    foreach (var x in directionVectors)
                    {
                        if (CheckIfSpecificTag(tile, x.Item1, x.Item2, blockingLayer, "Player") && !isAllegianceSame(GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer))
                            && !tile.transform.Find("mapTile").Find("CometZone").gameObject.activeSelf && !(CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer))))
                        {
                            TilesNearEnemyList.Add(tile);
                        }
                    }
                }
            }
            if (TilesNearEnemyList.Count > 0)
            {
                return TilesNearEnemyList[Random.Range(0, TilesNearEnemyList.Count - 1)];
            }
        }
            return null;
        }
        //Grid(for AI) creation
        private void AddSurroundingsToGridList(GameObject middleTile, int movementIndex)
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
                    this.AIGridTiles[movementIndex].Add(AddableObject);
                }
            }
        }
        private void CreateAIGrid()
        {
            this.AIGridTiles.Clear();
            if (AttackRange > 0)
            {
                this.AIGridTiles.Add(new List<GameObject>());
                AddSurroundingsToGridList(transform.gameObject, 0);
            }

            for (int i = 1; i <= AttackRange - 1; i++)
            {
                this.AIGridTiles.Add(new List<GameObject>());

                foreach (var tileInPreviousList in this.AIGridTiles[i - 1])
                {
                    AddSurroundingsToGridList(tileInPreviousList, i);
                }
            }
        }
        //

    }
