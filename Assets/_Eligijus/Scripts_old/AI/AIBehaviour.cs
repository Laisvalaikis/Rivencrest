using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    //Grid and Raycasts
    public List<SpecialAbility> specialAbilities = new List<SpecialAbility>();
    public List<SpecialBlessing> specialBlessings = new List<SpecialBlessing>();
    public LayerMask groundLayer;
    public LayerMask blockingLayer;
    public LayerMask fogLayer;

    private RaycastHit2D raycast;

    private List<List<GameObject>> TileGrid = new List<List<GameObject>>();
    //private bool isCoroutineExecuting;

    //
    private List<GameObject> Destinations;
    private int currentDestinationIndex;
    public GameObject DestinationObject;
    private int AttackRange;
    private List<(int, int)> AttackRangeVectors;
    private float castAfter;
    public Data _data;
    /*void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            DoTurnActions();
        }
    }*/
    void Start()
    {
        Destinations = GameObject.Find("GameInformation").GetComponent<AIManager>().AIDestinations;
        currentDestinationIndex = 0;//Random.Range(0, Destinations.Count - 1);
        DestinationObject = Destinations[currentDestinationIndex++];
        AttackRange = GetComponent<ActionManager>().FindActionByName("Attack").AttackRange;
        if (AttackRange == 1)
        {
            AttackRangeVectors = new List<(int, int)>
            {
            };
        }
        else
        {
            AttackRangeVectors = new List<(int, int)>
            {
            (1, 1),
            (-1, 1),
            (1, -1),
            (-1, -1)
            };
        }
    }
    public void DoTurnActions()
    {
        castAfter = 0.5f;
        if (!GetComponent<PlayerInformation>().Debuffs.Contains("Stun") && !GameObject.Find("GameInformation").GetComponent<GameInformation>().isVictoryScreenEnabled)
        {
            GameObject AttackTarget;
            if (_data.townData.difficultyLevel == 0)
            {
                AttackTarget = ClosestVisibleEnemyCharacter(GetCharactersInGrid(3 + AttackRange));
            }
            else
            {
                AttackTarget = EnemyWithLeastHealth(GetCharactersInGrid(3 + AttackRange));
            }
            DestinationObject = Destinations[currentDestinationIndex - 1];
            //Direction to go to
            if (DestinationObject != null && CheckIfSpecificLayer(DestinationObject, 0, 0, blockingLayer) && GetSpecificGroundTile(DestinationObject, 0, 0, blockingLayer) == gameObject)
            {
                if (currentDestinationIndex >= Destinations.Count)
                {
                    currentDestinationIndex = 0;
                }
                DestinationObject = Destinations[currentDestinationIndex++];
            }
            //ACTIONS
            //BeforeMovement
            CastAbilitiesByCastOrder(0);
            //Moves and attacks if possible
            if (AttackTarget != null && PlaceToMoveToWhenAttacking(AttackTarget, AttackRange) != null)
            {

                StartCoroutine(ExecuteAfterTime(castAfter, () =>
                {
                    MoveCharacterToTile(PlaceToMoveToWhenAttacking(AttackTarget, AttackRange));
                }));
                castAfter += 0.4f;
                //BeforeAttack
                CastAbilitiesByCastOrder(1);
                //Basic attack
                StartCoroutine(ExecuteAfterTime(castAfter, () =>  //Padaro kazka po sekundes <3
                {
                    GetComponent<ActionManager>().FindActionByName("Attack").ResolveAbility(AttackTarget);
                    FlipCharacter(AttackTarget);
                    EndAIAction();
                }));
                castAfter += 0.6f;
                //Using special Ability
                CastAbilitiesByCastOrder(3);
            }
    
            //Moves to destination if possible
            else if (DestinationObject != null && ClosestMovementTileToDestination(DestinationObject) != null)
            {
                StartCoroutine(ExecuteAfterTime(castAfter, () =>
                {
                    MoveCharacterToTile(ClosestMovementTileToDestination(DestinationObject));
                }));
                castAfter += 0.4f;
                //Wall
                StartCoroutine(ExecuteAfterTime(castAfter, () =>
                {
                    int toAttackNearbyWallOrNot = Random.Range(0, 4);//jei tik siaip paeina
                    if (GetComponent<GridMovement>().AvailableMovementPoints > 0 || toAttackNearbyWallOrNot < 2)
                    {
                        GameObject WallTarget = ClosestTileFromListToDestination(DestinationObject, GetSurroundingWalls());
                        if (WallTarget != null)
                        {
                            /*if (AttackRange > 1 && PlaceToMoveToWhenAttacking(WallTarget, AttackRange) != null)
                            {
                                MoveCharacterToTile(PlaceToMoveToWhenAttacking(AttackTarget, AttackRange));
                            }*/
                            GetComponent<ActionManager>().FindActionByName("Attack").ResolveAbility(WallTarget);
                            FlipCharacter(WallTarget);
                            EndAIAction();
                        }
                    }
                }));
                castAfter += 0.6f;
                //Casts a spell if possible
                CastAbilitiesByCastOrder(3);
            }
        }
    }
    public float TimeRequiredForTurn()//Return max possible time spent in turn
    {
        float TimeRequired = 0.6f;
        if (!GetComponent<PlayerInformation>().Debuffs.Contains("Stun"))
        {
            if (areAllAbilitiesOnCooldown() || RandomEnemyCharacter(GetCharactersInGrid(7)) == null)
            {
                TimeRequired += 0.5f;
            }
            else
            {
                TimeRequired += 1f;
                TimeRequired += availableAbilitiesCount() * 0.6f;
            }
        }
        return TimeRequired;
    }
    private List<GameObject> GetSurroundingWalls()
    {
        List<GameObject> surroundingWallList = new List<GameObject>();
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        if (AttackRange > 1)
        {
            directionVectors = AttackRangeVectors;
        }

        foreach (var x in directionVectors)
        {
            bool isWall = CheckIfSpecificTag(gameObject, x.Item1, x.Item2, blockingLayer, "Wall");
            if (isWall)
            {
                GameObject addableTile = GetSpecificGroundTile(gameObject, x.Item1, x.Item2, blockingLayer);
                surroundingWallList.Add(addableTile);
            }
        }
        return surroundingWallList;
    }
    private GameObject ClosestVisibleEnemyCharacter(List<GameObject> characterList)
    {
        GameObject closestCharacter;
        List<GameObject> closestCharacterList = new List<GameObject>();
        float closestDistance = 9999;
        foreach (GameObject character in characterList)
        {
            if (character.CompareTag("Wall") || isCharacterOnDifferentAllegiance(character) && PlaceToMoveToWhenAttacking(character, AttackRange) != null)
            {
                float xDistance = Math.Abs(transform.position.x - character.transform.position.x);
                float yDistance = Math.Abs(transform.position.y - character.transform.position.y);
                if (xDistance + yDistance == closestDistance)
                {
                    closestCharacterList.Add(character);
                }
                else if (xDistance + yDistance < closestDistance)
                {
                    closestDistance = xDistance + yDistance;
                    closestCharacterList.Clear();
                    closestCharacterList.Add(character);
                }
            }
        }
        if (closestCharacterList.Count > 0)
        {
            closestCharacter = closestCharacterList[Random.Range(0, closestCharacterList.Count - 1)];
        }
        else
        {
            closestCharacter = null;
        }
        return closestCharacter;
    }
    private GameObject EnemyWithLeastHealth(List<GameObject> characterList)
    {
        GameObject lowestHealthCharacter;
        List<GameObject> lowestHealthCharacterList = new List<GameObject>();
        int lowestHealth = 999;
        //Tier1 CreatingField
        foreach (GameObject character in characterList)
        {
            if (character.CompareTag("Player") && isCharacterOnDifferentAllegiance(character) && PlaceToMoveToWhenAttacking(character, AttackRange) != null)
            {

                if (lowestHealth == character.GetComponent<PlayerInformation>().health && !character.GetComponent<PlayerInformation>().Stasis && (character.GetComponent<PlayerInformation>().IsCreatingWhiteField || character.GetComponent<PlayerInformation>().Marker != null))
                {
                    lowestHealthCharacterList.Add(character);
                }
                else if (character.GetComponent<PlayerInformation>().health < lowestHealth && character.GetComponent<PlayerInformation>().IsCreatingWhiteField && (!character.GetComponent<PlayerInformation>().Stasis || character.GetComponent<PlayerInformation>().health <= 4))
                {
                    lowestHealth = character.GetComponent<PlayerInformation>().health;
                    lowestHealthCharacterList.Clear();
                    lowestHealthCharacterList.Add(character);
                }
            }
        }
        //Tier2
        if (lowestHealthCharacterList.Count == 0)
        {
            foreach (GameObject character in characterList)
            {
                if (character.CompareTag("Player") && isCharacterOnDifferentAllegiance(character) && PlaceToMoveToWhenAttacking(character, AttackRange) != null)
                {

                    if (lowestHealth == character.GetComponent<PlayerInformation>().health && !character.GetComponent<PlayerInformation>().Stasis)
                    {
                        lowestHealthCharacterList.Add(character);
                    }
                    else if (character.GetComponent<PlayerInformation>().health < lowestHealth && (!character.GetComponent<PlayerInformation>().Stasis || character.GetComponent<PlayerInformation>().health <= 4))
                    {
                        lowestHealth = character.GetComponent<PlayerInformation>().health;
                        lowestHealthCharacterList.Clear();
                        lowestHealthCharacterList.Add(character);
                    }
                }
            }
        }
        if (lowestHealthCharacterList.Count > 0)
        {
            lowestHealthCharacter = lowestHealthCharacterList[Random.Range(0, lowestHealthCharacterList.Count - 1)];
        }
        else
        {
            lowestHealthCharacter = null;
        }
        return lowestHealthCharacter;
    }
    private GameObject RandomEnemyCharacter(List<GameObject> characterList)
    {
        List<GameObject> enemyCharacterList = new List<GameObject>();

        foreach (GameObject character in characterList)
        {
            if (isCharacterOnDifferentAllegiance(character))
            {
                enemyCharacterList.Add(character);
            }
        }
        if (enemyCharacterList.Count > 0)
        {
            return enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)];
        }
        return null;
    }
    private GameObject ClosestMovementTileToDestination(GameObject destination)
    {
        if (GetComponent<PlayerInformation>().CantMove && CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
        {
            return GetSpecificGroundTile(gameObject, 0, 0, groundLayer);
        }
        GetComponent<GridMovement>().CreateGrid();
        CreateGrid(GetComponent<GridMovement>().AvailableMovementPoints, "Movement");
        GameObject closestTile;
        List<GameObject> closestTileList = new List<GameObject>();
        float closestDistance = 9999;
        foreach (List<GameObject> tileList in TileGrid)
        {
            foreach (GameObject tile in tileList)
            {
                bool isBlockingLayer = CheckIfSpecificLayer(tile, 0, 0, blockingLayer);
                bool isThisPlayerOnTop = (CheckIfSpecificLayer(tile, 0, 0, blockingLayer) && GetSpecificGroundTile(tile, 0, 0, blockingLayer) == gameObject);

                if ((!isBlockingLayer || (isThisPlayerOnTop)) && !ShouldCharacterBeAfraidToGoOnTile(tile))
                {
                    float xDistance = Math.Abs(destination.transform.position.x - tile.transform.position.x);
                    float yDistance = Math.Abs(destination.transform.position.y - tile.transform.position.y);
                    if (xDistance + yDistance == closestDistance)
                    {
                        closestTileList.Add(tile);
                    }
                    else if (xDistance + yDistance < closestDistance)
                    {
                        closestDistance = xDistance + yDistance;
                        closestTileList.Clear();
                        closestTileList.Add(tile);
                    }
                }
            }
        }
        if (closestTileList.Count > 0)
        {
            closestTile = closestTileList[Random.Range(0, closestTileList.Count - 1)];
        }
        else
        {
            closestTile = null;
        }
        return closestTile;
    }
    private GameObject ClosestTileFromListToDestination(GameObject destination, List<GameObject> tileList)
    {
        GameObject closestTile;
        List<GameObject> closestTileList = new List<GameObject>();
        float closestDistance = 9999;
        foreach (GameObject tile in tileList)
        {
            if (!ShouldCharacterBeAfraidToGoOnTile(tile))
            {
                float xDistance = Math.Abs(destination.transform.position.x - tile.transform.position.x);
                float yDistance = Math.Abs(destination.transform.position.y - tile.transform.position.y);
                if (xDistance + yDistance == closestDistance)
                {
                    closestTileList.Add(tile);
                }
                else if (xDistance + yDistance < closestDistance)
                {
                    closestDistance = xDistance + yDistance;
                    closestTileList.Clear();
                    closestTileList.Add(tile);
                }
            }
        }
        if (closestTileList.Count > 0)
        {
            closestTile = closestTileList[Random.Range(0, closestTileList.Count - 1)];
        }
        else
        {
            closestTile = null;
        }
        return closestTile;
    }
    private void MoveCharacterToTile(GameObject newPosition)
    {
        GetComponent<GridMovement>().CreateGrid();
        GetComponent<GridMovement>().CreateWayList(newPosition);
        FlipCharacter(newPosition);
        /* transform.position = newPosition.transform.position + new Vector3(0f, 0f, -1f);
         if (!(CheckIfSpecificLayer(newPosition, 0, 0, blockingLayer) && GetSpecificGroundTile(newPosition, 0, 0, blockingLayer) == gameObject))
         {
             GetComponent<GridMovement>().RemoveAvailableMovementPoints(newPosition);
         }*/
        GameObject.Find("GameInformation").GetComponent<GameInformation>().MoveCharacter(newPosition, gameObject);
        if (CheckIfSpecificLayer(newPosition, 0, 0, groundLayer) &&
            !GetSpecificGroundTile(newPosition, 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf)//focus camera when AI steps on visible tile
        {
            GameObject.Find("GameInformation").GetComponent<GameInformation>().FocusSelectedCharacter(gameObject);
        }
    }
    private GameObject PlaceToMoveToWhenAttacking(GameObject attackTarget, int attackRange)
    {
        List<GameObject> possibleMovePositions = GetSurroundingTiles(attackTarget, attackRange);
        List<GameObject> placesToMove = new List<GameObject>();
        GetComponent<GridMovement>().CreateGrid();
        if (GetComponent<PlayerInformation>().CantMove)
        {
            GetComponent<GridMovement>().AvailableMovementTiles.Clear();
        }
        if (GetComponent<GridMovement>().AvailableMovementTiles.Count > 0)
        {
            GetComponent<GridMovement>().AvailableMovementTiles[0].Add(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
        }
        else
        {
            GetComponent<GridMovement>().AvailableMovementTiles.Add(new List<GameObject>());
            GetComponent<GridMovement>().AvailableMovementTiles[0].Add(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
        }
        CreateGrid(GetComponent<GridMovement>().AvailableMovementPoints, "Movement");
        foreach (List<GameObject> tilesInEachGridIndex in GetComponent<GridMovement>().AvailableMovementTiles) // buvo this.TileGrid
        {
            foreach (GameObject tile in tilesInEachGridIndex)
            {
                foreach (GameObject possibleMovePosition in possibleMovePositions)
                {
                    //gal random sita checkint
                    bool isItCantAttackTile = CheckIfSpecificLayer(possibleMovePosition, 0, 0, fogLayer) || CheckIfSpecificTag(possibleMovePosition, 0, 0, groundLayer, "Water");
                    if (possibleMovePosition == tile && !isItCantAttackTile && !ShouldCharacterBeAfraidToGoOnTile(possibleMovePosition))
                    {
                        placesToMove.Add(tile);
                    }
                }
            }
        }
        if (placesToMove.Count > 0)
        {
            return placesToMove[Random.Range(0, placesToMove.Count - 1)];
        }
        return null;
    }
    private List<GameObject> GetSurroundingTiles(GameObject middleTile, int surroundingRange)//tik tos, ant kuriu galima atsistot
    {
        List<GameObject> surroundingTileList = new List<GameObject>();
        var directionVectors = new List<(int, int)>
        {
            (surroundingRange, 0),
            (0, surroundingRange),
            (-surroundingRange, 0),
            (0, -surroundingRange)
        };
        var directionVectors2 = AttackRangeVectors;
        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isThisPlayerOnTop = (CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer) && GetSpecificGroundTile(middleTile, x.Item1, x.Item2, blockingLayer) == gameObject);
            bool isThereSomethingInBetween;
            if (surroundingRange > 1)
            {
                isThereSomethingInBetween = CheckIfSpecificLayer(middleTile, x.Item1 / 2, x.Item2 / 2, blockingLayer);
            }
            else
            {
                isThereSomethingInBetween = false;
            }
            if (isGround && (!isBlockingLayer || isThisPlayerOnTop) && !isThereSomethingInBetween)
            {
                GameObject addableTile = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                surroundingTileList.Add(addableTile);
            }
        }
        //istrizi langeliai
        foreach (var x in directionVectors2)
        {
            bool isGround = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isThisPlayerOnTop = (CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer) && GetSpecificGroundTile(middleTile, x.Item1, x.Item2, blockingLayer) == gameObject);
            if (isGround && (!isBlockingLayer || isThisPlayerOnTop))
            {
                GameObject addableTile = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                surroundingTileList.Add(addableTile);
            }
        }
        return surroundingTileList;
    }
    public List<GameObject> GetCharactersInGrid(int GridRange)
    {
        CreateGrid(GridRange, "Vision");
        List<GameObject> charactersInGrid = new List<GameObject>();
        foreach (List<GameObject> tilesInEachGridIndex in this.TileGrid)
        {
            foreach (GameObject tile in tilesInEachGridIndex)
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !GetSpecificGroundTile(tile, 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf) //kad nepultu to ko net nemato;
                {
                    charactersInGrid.Add(GetSpecificGroundTile(tile, 0, 0, blockingLayer));
                }
            }
        }
        return charactersInGrid;
    }

    //RayCast Functions
    protected GameObject GetSpecificGroundTile(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        return raycast.transform.gameObject;
    }
    protected bool CheckIfSpecificLayer(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        return true;
    }
    protected bool CheckIfSpecificTag(GameObject tile, int x, int y, LayerMask chosenLayer, string tagName)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        else if (raycast.transform.CompareTag(tagName))
        {
            return true;
        }
        return false;
    }
    //
    //TileGrid
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex, string gridType)
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
            bool isPlayer;
            bool isWall;
            if (gridType == "Movement")
            {
                isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player") && !isCharacterOnDifferentAllegiance(GetSpecificGroundTile(middleTile, x.Item1, x.Item2, blockingLayer));
                isWall = false;
            }
            else
            {
                isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
                isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            }
            // bool isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            bool isMiddleTileWall = CheckIfSpecificTag(middleTile, 0, 0, blockingLayer, "Wall");
            if (isGroundLayer && (!isBlockingLayer || (isPlayer) || isWall) && !isMiddleTileWall) //&& isCharacterOnDifferentAllegiance(middleTile)
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.TileGrid[movementIndex].Add(AddableObject);
            }
        }
    }
    private void CreateGrid(int tileGridRange, string gridType)
    {
        if (!GetComponent<PlayerInformation>().Debuffs.Contains("Stun") && !(gridType == "Movement" && GetComponent<PlayerInformation>().CantMove))
        {
            this.TileGrid.Clear();
            if (tileGridRange > 0)
            {
                this.TileGrid.Add(new List<GameObject>());
                TileGrid[0].Add(GetSpecificGroundTile(gameObject, 0, 0, groundLayer)); //optional
                AddSurroundingsToList(GetSpecificGroundTile(gameObject, 0, 0, groundLayer), 0, gridType);
            }

            for (int i = 1; i <= tileGridRange - 1; i++)
            {
                this.TileGrid.Add(new List<GameObject>());

                foreach (var tileInPreviousList in this.TileGrid[i - 1])
                {
                    AddSurroundingsToList(tileInPreviousList, i, gridType);
                }
                //TileGrid[i] = ShuffleGameObjectList(TileGrid[i]);
            }
        }
    }
    private bool isCharacterOnDifferentAllegiance(GameObject charactersTile)
    {
        if (CheckIfSpecificLayer(charactersTile, 0, 0, blockingLayer))
        {
            GameObject character = GetSpecificGroundTile(charactersTile, 0, 0, blockingLayer);
            return GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(character.GetComponent<PlayerInformation>().CharactersTeam) !=
                    GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(GetComponent<PlayerInformation>().CharactersTeam);
        }
        return false;
    }
    private List<GameObject> ShuffleGameObjectList(List<GameObject> originalList)
    {
        List<GameObject> shuffledList = new List<GameObject>();
        for (int i = 0; i < originalList.Count; i++)
        {
            int n = Random.Range(0, originalList.Count - 1);
            shuffledList.Add(originalList[n]);
            originalList.RemoveAt(n);
        }
        return shuffledList;
    }
    private void FlipCharacter(GameObject tile)
    {
        if (tile.transform.position.x > transform.position.x)
        {
            GetComponent<PlayerMovement>().Flip(true);
        }
        else if (tile.transform.position.x < transform.position.x)
        {
            GetComponent<PlayerMovement>().Flip(false);
        }
    }
    private bool ShouldCharacterBeAfraidToGoOnTile(GameObject tile)
    {
        bool isItDangerZone = (CheckIfSpecificLayer(tile, 0, 0, groundLayer) && ((GetSpecificGroundTile(tile, 0, 0, groundLayer).transform.Find("DangerUI").gameObject.activeSelf) 
            || (GetSpecificGroundTile(tile, 0, 0, groundLayer).transform.Find("mapTile").Find("PinkZone").gameObject.activeSelf)
            || (GetSpecificGroundTile(tile, 0, 0, groundLayer).transform.Find("mapTile").Find("GreenZone").gameObject.activeSelf)
            ));
        //not afraid when
        bool ShouldCharacterGo = !isItDangerZone || (isItDangerZone && (GetComponent<PlayerInformation>().BarrierProvider != null || GetComponent<GridMovement>().AvailableMovementPoints < 3));
        return !ShouldCharacterGo;
    }
    private void EndAIAction()
    {
        // GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().activateOnce = true;
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
        GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FakeUpdate();
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
    //
    [System.Serializable]
    public class SpecialAbility
    {
        public string abilityName;
        public int castOrder; //0 - before moving/attack, 1 - only before attack, 2 - attack, 3 - after attack 
        public int difficultyLevel;
    }
    private List<SpecialAbility> FindAllSpecialAbilitiesByCastOrder(int castOrder)
    {
        List<SpecialAbility> validAbilities = new List<SpecialAbility>();
        foreach (SpecialAbility x in specialAbilities)
        {
            if (x.castOrder == castOrder)
            {
                validAbilities.Add(x);
            }
        }
        return validAbilities;
    }
    private void CastAbilitiesByCastOrder(int castOrder)
    {
        if (FindAllSpecialAbilitiesByCastOrder(castOrder).Count > 0)
        {
            foreach (SpecialAbility x in FindAllSpecialAbilitiesByCastOrder(castOrder))
            {
                if (GetComponent<ActionManager>().FindActionByName(x.abilityName).canGridBeEnabled() 
                    && x.difficultyLevel <= _data.townData.selectedEncounter.encounterLevel)
                {
                    StartCoroutine(ExecuteAfterTime(castAfter, () =>
                    {
                        GetComponent<ActionManager>().FindActionByName(x.abilityName).PrepareForAIAction();
                        GameObject AttackTarget = GetComponent<ActionManager>().FindActionByName(x.abilityName).PossibleAIActionTile();
                        if (AttackTarget != null)
                        {
                            GetComponent<ActionManager>().FindActionByName(x.abilityName).ResolveAbility(AttackTarget);
                            FlipCharacter(AttackTarget);
                            EndAIAction();
                        }
                    }));
                    castAfter += 0.6f;
                }
            }
        }
    }
    private bool areAllAbilitiesOnCooldown()
    {
        foreach (SpecialAbility x in specialAbilities)
        {
            if (GetComponent<ActionManager>().FindActionByName(x.abilityName).canGridBeEnabled())
            {
                return false;
            }
        }
        return true;
    }
    private int availableAbilitiesCount()
    {
        List<string> availableAbilityNames = new List<string>();
        foreach (SpecialAbility x in specialAbilities)
        {
            if (GetComponent<ActionManager>().FindActionByName(x.abilityName).canGridBeEnabled() && !availableAbilityNames.Contains(x.abilityName))
            {
                availableAbilityNames.Add(x.abilityName);
            }
        }
        return availableAbilityNames.Count;
    }
    [System.Serializable]
    public class SpecialBlessing
    {
        public string blessingName;
        public int difficultyLevel;//Encounter
    }
}
