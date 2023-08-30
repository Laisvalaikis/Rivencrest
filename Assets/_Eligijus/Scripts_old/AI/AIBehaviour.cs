using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    //Grid and Raycasts
    public List<SpecialAbility> specialAbilities;
    public List<SpecialBlessing> specialBlessings;
    public LayerMask groundLayer;
    public LayerMask blockingLayer;
    public LayerMask fogLayer;
    [SerializeField]
    private PlayerInformation playerInformation;
    [SerializeField]
    PlayerMovement playerMovement;
    [SerializeField]
    // ActionManager actionManager;
    private RaycastHit2D raycast;

    private List<List<GameObject>> _tileGrid;
    // private List<Vector3> _destinations;
    private int _currentDestinationIndex;
    // public Vector3 DestinationObject;
    private int _attackRange;
    private List<(int, int)> _attackRangeVectors;
    private float _castAfter;
    public Data _data;

    void Start()
    {
        if (_data == null)
        {
            _data = Data.Instance;
        }

        // _destinations = GameObject.Find("GameInformation").GetComponent<AIManager>().AIDestinations;
        _currentDestinationIndex = 0;
        // _attackRange = actionManager.FindActionByName("Attack").AttackRange;
        if (_attackRange == 1)
        {
            _attackRangeVectors = new List<(int, int)>();
        }
        else
        {
            _attackRangeVectors = new List<(int, int)>
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
        _castAfter = 0.5f;
        if (!playerInformation.Debuffs.Contains("Stun") && !GameObject.Find("GameInformation").GetComponent<GameInformation>().isVictoryScreenEnabled)
        {
            GameObject attackTarget;
            if (_data.townData.difficultyLevel == 0)
            {
                attackTarget = ClosestVisibleEnemyCharacter(GetCharactersInGrid(3 + _attackRange));
            }
            else
            {
                attackTarget = EnemyWithLeastHealth(GetCharactersInGrid(3 + _attackRange));
            }
            // DestinationObject = _destinations[_currentDestinationIndex - 1];
            //Direction to go to
            // if (CheckIfSpecificLayer(DestinationObject, 0, 0, blockingLayer) && GetSpecificGroundTile(DestinationObject, 0, 0, blockingLayer) == gameObject)
            // {
                // if (_currentDestinationIndex >= _destinations.Count)
                // {
                    // _currentDestinationIndex = 0;
                // }
                // DestinationObject = _destinations[_currentDestinationIndex++];
            // }
            //ACTIONS
            //BeforeMovement
            CastAbilitiesByCastOrder(0);
            //Moves and attacks if possible
            if (attackTarget != null && PlaceToMoveToWhenAttacking(attackTarget, _attackRange) != null)
            {

                StartCoroutine(ExecuteAfterTime(_castAfter, () =>
                {
                    MoveCharacterToTile(PlaceToMoveToWhenAttacking(attackTarget, _attackRange));
                }));
                _castAfter += 0.4f;
                //BeforeAttack
                CastAbilitiesByCastOrder(1);
                //Basic attack
                StartCoroutine(ExecuteAfterTime(_castAfter, () =>  //Padaro kazka po sekundes <3
                {
                    // actionManager.FindActionByName("Attack").ResolveAbility(attackTarget);
                    FlipCharacter(attackTarget);
                    EndAIAction();
                }));
                _castAfter += 0.6f;
                //Using special Ability
                CastAbilitiesByCastOrder(3);
            }
    
            //Moves to destination if possible
            // else if (ClosestMovementTileToDestination(DestinationObject) != null)
            // {
            //     StartCoroutine(ExecuteAfterTime(_castAfter, () =>
            //     {
            //         MoveCharacterToTile(ClosestMovementTileToDestination(DestinationObject));
            //     }));
                // _castAfter += 0.4f;
                //Wall
                // StartCoroutine(ExecuteAfterTime(_castAfter, () =>
                // {
                    // int toAttackNearbyWallOrNot = Random.Range(0, 4);//jei tik siaip paeina
                    // if (gridMovement.AvailableMovementPoints > 0 || toAttackNearbyWallOrNot < 2)
                    // {
                    //     GameObject wallTarget = ClosestTileFromListToDestination(DestinationObject, GetSurroundingWalls());
                    //     if (wallTarget != null)
                    //     {
                            /*if (AttackRange > 1 && PlaceToMoveToWhenAttacking(WallTarget, AttackRange) != null)
                            {
                                MoveCharacterToTile(PlaceToMoveToWhenAttacking(AttackTarget, AttackRange));
                            }*/
                            // actionManager.FindActionByName("Attack").ResolveAbility(wallTarget);
                    //         FlipCharacter(wallTarget);
                    //         EndAIAction();
                    //     }
                    // }
                // }));
                // _castAfter += 0.6f;
                //Casts a spell if possible
                // CastAbilitiesByCastOrder(3);
            // }
        }
    }
    public float TimeRequiredForTurn()//Return max possible time spent in turn
    {
        float timeRequired = 0.6f;
        if (!playerInformation.Debuffs.Contains("Stun"))
        {
            if (areAllAbilitiesOnCooldown() || RandomEnemyCharacter(GetCharactersInGrid(7)) == null)
            {
                timeRequired += 0.5f;
            }
            else
            {
                timeRequired += 1f;
                timeRequired += availableAbilitiesCount() * 0.6f;
            }
        }
        return timeRequired;
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
        if (_attackRange > 1)
        {
            directionVectors = _attackRangeVectors;
        }

        foreach (var x in directionVectors)
        {
            bool isWall = CheckIfSpecificTag(gameObject.transform.position, x.Item1, x.Item2, blockingLayer, "Wall");
            if (isWall)
            {
                GameObject addableTile = GetSpecificGroundTile(gameObject.transform.position, x.Item1, x.Item2, blockingLayer);
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
            if (character.CompareTag("Wall") || isCharacterOnDifferentAllegiance(character) && PlaceToMoveToWhenAttacking(character, _attackRange) != null)
            {
                Vector3 characterPosition = character.transform.position;
                Vector3 transformPosition = transform.position;
                float xDistance = Math.Abs(transformPosition.x - characterPosition.x);
                float yDistance = Math.Abs(transformPosition.y - characterPosition.y);
                if (Math.Abs(xDistance + yDistance - closestDistance) < 0.001f)
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
            if (character.CompareTag("Player") && isCharacterOnDifferentAllegiance(character) && PlaceToMoveToWhenAttacking(character, _attackRange) != null)
            {
                PlayerInformation characterInformation = character.GetComponent<PlayerInformation>();
                if (lowestHealth == characterInformation.health && !characterInformation.Stasis && (characterInformation.IsCreatingWhiteField || characterInformation.Marker != null))
                {
                    lowestHealthCharacterList.Add(character);
                }
                else if (characterInformation.health < lowestHealth && characterInformation.IsCreatingWhiteField && (!characterInformation.Stasis || characterInformation.health <= 4))
                {
                    lowestHealth = characterInformation.health;
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
                if (character.CompareTag("Player") && isCharacterOnDifferentAllegiance(character) && PlaceToMoveToWhenAttacking(character, _attackRange) != null)
                {
                    PlayerInformation characterInformation = character.GetComponent<PlayerInformation>();
                    if (lowestHealth == characterInformation.health && !characterInformation.Stasis)
                    {
                        lowestHealthCharacterList.Add(character);
                    }
                    else if (characterInformation.health < lowestHealth && (!characterInformation.Stasis || characterInformation.health <= 4))
                    {
                        lowestHealth = characterInformation.health;
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
    private GameObject ClosestMovementTileToDestination(Vector3 destination)
    {
        if (playerInformation.CantMove && CheckIfSpecificLayer(gameObject.transform.position, 0, 0, groundLayer))
        {
            return GetSpecificGroundTile(gameObject.transform.position, 0, 0, groundLayer);
        }
        // gridMovement.CreateGrid();
        // CreateGrid(gridMovement.AvailableMovementPoints, "Movement");
        GameObject closestTile;
        List<GameObject> closestTileList = new List<GameObject>();
        float closestDistance = 9999;
        foreach (List<GameObject> tileList in _tileGrid)
        {
            foreach (GameObject tile in tileList)
            {
                Vector3 tilePosition = tile.transform.position;
                bool isBlockingLayer = CheckIfSpecificLayer(tilePosition, 0, 0, blockingLayer);
                bool isThisPlayerOnTop = (CheckIfSpecificLayer(tilePosition, 0, 0, blockingLayer) && GetSpecificGroundTile(tilePosition, 0, 0, blockingLayer) == gameObject);

                if ((!isBlockingLayer || (isThisPlayerOnTop)) && !ShouldCharacterBeAfraidToGoOnTile(tile))
                {
                    float xDistance = Math.Abs(destination.x - tilePosition.x);
                    float yDistance = Math.Abs(destination.y - tilePosition.y);
                    if (Math.Abs(xDistance + yDistance - closestDistance) < 0.001f)
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
    private GameObject ClosestTileFromListToDestination(Vector3 destination, List<GameObject> tileList)
    {
        GameObject closestTile;
        List<GameObject> closestTileList = new List<GameObject>();
        float closestDistance = 9999;
        foreach (GameObject tile in tileList)
        {
            if (!ShouldCharacterBeAfraidToGoOnTile(tile))
            {
                Vector3 tilePosition = tile.transform.position;
                float xDistance = Math.Abs(destination.x - tilePosition.x);
                float yDistance = Math.Abs(destination.y - tilePosition.y);
                if (Math.Abs(xDistance + yDistance - closestDistance) < 0.001f)
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
        // gridMovement.CreateGrid();
        // gridMovement.CreateWayList(newPosition);
        FlipCharacter(newPosition);
        GameObject.Find("GameInformation").GetComponent<GameInformation>().MoveCharacter(newPosition, gameObject);
        if (CheckIfSpecificLayer(newPosition.transform.position, 0, 0, groundLayer) &&
            !GetSpecificGroundTile(newPosition.transform.position, 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf)//focus camera when AI steps on visible tile
        {
            GameObject.Find("GameInformation").GetComponent<GameInformation>().FocusSelectedCharacter(gameObject);
        }
    }
    private GameObject PlaceToMoveToWhenAttacking(GameObject attackTarget, int attackRange)
    {
        List<GameObject> possibleMovePositions = GetSurroundingTiles(attackTarget, attackRange);
        List<GameObject> placesToMove = new List<GameObject>();
        // gridMovement.CreateGrid();
        // if (playerInformation.CantMove)
        // {
        //     gridMovement.AvailableMovementTiles.Clear();
        // }
        // if (gridMovement.AvailableMovementTiles.Count > 0)
        // {
        //     gridMovement.AvailableMovementTiles[0].Add(GetSpecificGroundTile(gameObject.transform.position, 0, 0, groundLayer));
        // }
        // else
        // {
        //     gridMovement.AvailableMovementTiles.Add(new List<GameObject>());
        //     gridMovement.AvailableMovementTiles[0].Add(GetSpecificGroundTile(gameObject.transform.position, 0, 0, groundLayer));
        // }
        // CreateGrid(gridMovement.AvailableMovementPoints, "Movement");
        // foreach (List<GameObject> tilesInEachGridIndex in gridMovement.AvailableMovementTiles) // buvo this.TileGrid
        // {
        //     foreach (GameObject tile in tilesInEachGridIndex)
        //     {
        //         foreach (GameObject possibleMovePosition in possibleMovePositions)
        //         {
        //             //gal random sita checkint
        //             Vector3 possibleMove = possibleMovePosition.transform.position;
        //             bool isItCantAttackTile = CheckIfSpecificLayer(possibleMove, 0, 0, fogLayer) || CheckIfSpecificTag(possibleMove, 0, 0, groundLayer, "Water");
        //             if (possibleMovePosition == tile && !isItCantAttackTile && !ShouldCharacterBeAfraidToGoOnTile(possibleMovePosition))
        //             {
        //                 placesToMove.Add(tile);
        //             }
        //         }
        //     }
        // }
        if (placesToMove.Count > 0)
        {
            return placesToMove[Random.Range(0, placesToMove.Count - 1)];
        }
        return null;
    }
    private List<GameObject> GetSurroundingTiles(GameObject middleTile, int surroundingRange)//tik tos, ant kuriu galima atsistot
    {
        List<GameObject> surroundingTileList = new List<GameObject>();
        Vector3 middleTilePosition = middleTile.transform.position;
        var directionVectors = new List<(int, int)>
        {
            (surroundingRange, 0),
            (0, surroundingRange),
            (-surroundingRange, 0),
            (0, -surroundingRange)
        };
        var directionVectors2 = _attackRangeVectors;
        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, blockingLayer);
            bool isThisPlayerOnTop = (CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, blockingLayer) && GetSpecificGroundTile(middleTile.transform.position, x.Item1, x.Item2, blockingLayer) == gameObject);
            bool isThereSomethingInBetween;
            if (surroundingRange > 1)
            {
                isThereSomethingInBetween = CheckIfSpecificLayer(middleTilePosition, x.Item1 / 2, x.Item2 / 2, blockingLayer);
            }
            else
            {
                isThereSomethingInBetween = false;
            }
            if (isGround && (!isBlockingLayer || isThisPlayerOnTop) && !isThereSomethingInBetween)
            {
                GameObject addableTile = GetSpecificGroundTile(middleTilePosition, x.Item1, x.Item2, groundLayer);
                surroundingTileList.Add(addableTile);
            }
        }
        //istrizi langeliai
        foreach (var x in directionVectors2)
        {
            bool isGround = CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, blockingLayer);
            bool isThisPlayerOnTop = (CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, blockingLayer) && GetSpecificGroundTile(middleTilePosition, x.Item1, x.Item2, blockingLayer) == gameObject);
            if (isGround && (!isBlockingLayer || isThisPlayerOnTop))
            {
                GameObject addableTile = GetSpecificGroundTile(middleTilePosition, x.Item1, x.Item2, groundLayer);
                surroundingTileList.Add(addableTile);
            }
        }
        return surroundingTileList;
    }
    public List<GameObject> GetCharactersInGrid(int gridRange)
    {
        CreateGrid(gridRange, "Vision");
        List<GameObject> charactersInGrid = new List<GameObject>();
        foreach (List<GameObject> tilesInEachGridIndex in this._tileGrid)
        {
            foreach (GameObject tile in tilesInEachGridIndex)
            {
                Vector3 tilePosition = tile.transform.position;
                if (CheckIfSpecificTag(tilePosition, 0, 0, blockingLayer, "Player") &&
                    !GetSpecificGroundTile(tilePosition, 0, 0, groundLayer).GetComponent<HighlightTile>().FogOfWarTile
                        .activeSelf) //kad nepultu to ko net nemato
                {
                    charactersInGrid.Add(GetSpecificGroundTile(tilePosition, 0, 0, blockingLayer));
                }
            }
        }
        return charactersInGrid;
    }

    //RayCast Functions
    private static RaycastHit2D GetRaycastHit(Vector3 tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        return Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
    }
    private GameObject GetSpecificGroundTile(Vector3 tile, int x, int y, LayerMask chosenLayer)
    {
    raycast = GetRaycastHit(tile, x, y, chosenLayer);
    return raycast.transform.gameObject;
    }

    private bool CheckIfSpecificLayer(Vector3 tile, int x, int y, LayerMask chosenLayer)
    {
        raycast = GetRaycastHit(tile, x, y, chosenLayer);
        return ReferenceEquals(raycast.transform, null);
    }
    private bool CheckIfSpecificTag(Vector3 tile, int x, int y, LayerMask chosenLayer, string tagName)
    {
        raycast = GetRaycastHit(tile, x, y, chosenLayer);
        if (ReferenceEquals(raycast.transform, null))
        {
            return false;
        }
        return raycast.transform.CompareTag(tagName);
    }
    //
    //TileGrid
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex, string gridType)
    {
        Vector3 middleTilePosition = middleTile.transform.position;
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        foreach (var x in directionVectors)
        {
            bool isGroundLayer = CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTilePosition, x.Item1, x.Item2, blockingLayer);
            bool isPlayer;
            bool isWall;
            if (gridType == "Movement")
            {
                isPlayer = CheckIfSpecificTag(middleTilePosition, x.Item1, x.Item2, blockingLayer, "Player") && !isCharacterOnDifferentAllegiance(GetSpecificGroundTile(middleTile.transform.position, x.Item1, x.Item2, blockingLayer));
                isWall = false;
            }
            else
            {
                isPlayer = CheckIfSpecificTag(middleTilePosition, x.Item1, x.Item2, blockingLayer, "Player");
                isWall = CheckIfSpecificTag(middleTilePosition, x.Item1, x.Item2, blockingLayer, "Wall");
            }
            bool isMiddleTileWall = CheckIfSpecificTag(middleTilePosition, 0, 0, blockingLayer, "Wall");
            if (isGroundLayer && (!isBlockingLayer || (isPlayer) || isWall) && !isMiddleTileWall) //&& isCharacterOnDifferentAllegiance(middleTile)
            {
                GameObject addableObject = GetSpecificGroundTile(middleTile.transform.position, x.Item1, x.Item2, groundLayer);
                _tileGrid[movementIndex].Add(addableObject);
            }
        }
    }
    private void CreateGrid(int tileGridRange, string gridType)
    {
        if (!playerInformation.Debuffs.Contains("Stun") && !(gridType == "Movement" && playerInformation.CantMove))
        {
            _tileGrid.Clear();
            if (tileGridRange > 0)
            {
                Vector3 gameObjectPosition = gameObject.transform.position;
                _tileGrid.Add(new List<GameObject>());
                _tileGrid[0].Add(GetSpecificGroundTile(gameObjectPosition, 0, 0, groundLayer)); //optional
                AddSurroundingsToList(GetSpecificGroundTile(gameObjectPosition, 0, 0, groundLayer), 0, gridType);
            }
            for (int i = 1; i <= tileGridRange - 1; i++)
            {
                _tileGrid.Add(new List<GameObject>());

                foreach (var tileInPreviousList in this._tileGrid[i - 1])
                {
                    AddSurroundingsToList(tileInPreviousList, i, gridType);
                }
                //TileGrid[i] = ShuffleGameObjectList(TileGrid[i]);
            }
        }
    }
    private bool isCharacterOnDifferentAllegiance(GameObject charactersTile)
    {
        if (CheckIfSpecificLayer(charactersTile.transform.position, 0, 0, blockingLayer))
        {
            GameObject character = GetSpecificGroundTile(charactersTile.transform.position, 0, 0, blockingLayer);
            return GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(character.GetComponent<PlayerInformation>().CharactersTeam) !=
                    GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(playerInformation.CharactersTeam);
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
            playerMovement.Flip(true);
        }
        else if (tile.transform.position.x < transform.position.x)
        {
            playerMovement.Flip(false);
        }
    }
    private bool ShouldCharacterBeAfraidToGoOnTile(GameObject tile)
    {
        Vector3 tilePosition = tile.transform.position;
        bool isItDangerZone = (CheckIfSpecificLayer(tilePosition, 0, 0, groundLayer) && ((GetSpecificGroundTile(tilePosition, 0, 0, groundLayer).transform.Find("DangerUI").gameObject.activeSelf) 
            || (GetSpecificGroundTile(tilePosition, 0, 0, groundLayer).transform.Find("mapTile").Find("PinkZone").gameObject.activeSelf)
            || (GetSpecificGroundTile(tilePosition, 0, 0, groundLayer).transform.Find("mapTile").Find("GreenZone").gameObject.activeSelf)
            ));
        //not afraid when
        bool shouldCharacterGo = !isItDangerZone || (playerInformation.BarrierProvider != null);
        return !shouldCharacterGo;
    }
    private void EndAIAction()
    {
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
                // if (actionManager.FindActionByName(x.abilityName).CanGridBeEnabled() 
                //     && x.difficultyLevel <= _data.townData.selectedEncounter.encounterLevel)
                // {
                //     StartCoroutine(ExecuteAfterTime(_castAfter, () =>
                //     {
                //         actionManager.FindActionByName(x.abilityName).PrepareForAIAction();
                //         GameObject attackTarget = actionManager.FindActionByName(x.abilityName).PossibleAIActionTile();
                //         if (attackTarget != null)
                //         {
                //             actionManager.FindActionByName(x.abilityName).ResolveAbility(attackTarget);
                //             FlipCharacter(attackTarget);
                //             EndAIAction();
                //         }
                //     }));
                //     _castAfter += 0.6f;
                // }
            }
        }
    }
    private bool areAllAbilitiesOnCooldown()
    {
        // foreach (SpecialAbility x in specialAbilities)
        // {
        //     // if (actionManager.FindActionByName(x.abilityName).CanGridBeEnabled())
        //     // {
        //     //     return false;
        //     // }
        // }
        return true;
    }
    private int availableAbilitiesCount()
    {
        // List<string> availableAbilityNames = new List<string>();
        // foreach (SpecialAbility x in specialAbilities)
        // {
        //     if (actionManager.FindActionByName(x.abilityName).CanGridBeEnabled() && !availableAbilityNames.Contains(x.abilityName))
        //     {
        //         availableAbilityNames.Add(x.abilityName);
        //     }
        // }
        return 0;
    }
    [System.Serializable]
    public class SpecialBlessing
    {
        public string blessingName;
        public int difficultyLevel;//Encounter
    }
}
