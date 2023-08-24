using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridMovement : BaseAction
{
    public int MovementPoints = 3;
    [HideInInspector] public int AvailableMovementPoints;

    private RaycastHit2D raycast;
    [HideInInspector] public bool IceSlow = false;
    [HideInInspector] public bool OilSlow = false;
    [HideInInspector] public bool Stun = false;
    [HideInInspector] public int currentlyRemovedMovementPoints;
    [HideInInspector] public int officiallyRemovedMovementPoints;

    //private bool isItMyTurn;

    [HideInInspector] public List<List<GameObject>> AvailableMovementTiles = new List<List<GameObject>>();
    [HideInInspector] public List<GameObject> WayTilesList = new List<GameObject>(); //kelias kuri nueina veikejas
    [HideInInspector] public GameObject PreferredWayTile = null;
    void Start()
    {
        AvailableMovementPoints = MovementPoints;
        if (DoesCharacterHaveBlessing("Head start"))
        {
            AvailableMovementPoints += 2;

        }
    }
    /*void Update()
    {
        FakeUpdate();
    }*/

    public void FakeUpdate()
    {
        if (CheckIfSpecificLayer(transform.gameObject, 0, 0, consumablesLayer))
        {
            GetSpecificGroundTile(transform.gameObject, 0, 0, consumablesLayer).GetComponent<ConsumableManager>().ConsumableScript.PickUp(transform.gameObject);
        }
        /*if (IceSlow)
        {
            if (AvailableMovementPoints > 0)
            {
                AvailableMovementPoints--;
                currentlyRemovedMovementPoints++;
            }
            gameObject.GetComponent<PlayerInformation>().Slow1 = true;
            transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().SetBool("iceFreeze", true);
            IceSlow = false;
        }
        if (OilSlow)
        {
            if (AvailableMovementPoints - 2 <= 0)
            {
                currentlyRemovedMovementPoints += AvailableMovementPoints;
                AvailableMovementPoints = 0;
            }
            else if (AvailableMovementPoints > 0)
            {
                AvailableMovementPoints -= 2;
                currentlyRemovedMovementPoints += 2;
            }
            gameObject.GetComponent<PlayerInformation>().Slow2 = true;
            transform.Find("VFX").Find("VFXOilSlow").GetComponent<Animator>().SetBool("oilSlow", true);
            OilSlow = false;
        }
        if (Stun) //cia pakeist dar kad neitu jam gaut movement pointsu
        {
            isDisabled = true;
            gameObject.GetComponent<PlayerInformation>().Stun = true;
            transform.Find("VFX").Find("VFXStun").gameObject.SetActive(true);
            Stun = false;
            if (GetComponent<PlayerInformation>().IsCreatingWhiteField)
            {
                GetComponent<CreateWhiteField>().OnTurnStart();//sunaikina white field
            }
        }
        //
        gameObject.GetComponent<PlayerInformation>().Slow1 = false;
        gameObject.GetComponent<PlayerInformation>().Slow2 = false;
        gameObject.GetComponent<PlayerInformation>().Slow3 = false;
        if (currentlyRemovedMovementPoints == 1)
        {
            gameObject.GetComponent<PlayerInformation>().Slow1 = true;

        }
        if (currentlyRemovedMovementPoints == 2)
        {
            gameObject.GetComponent<PlayerInformation>().Slow2 = true;

        }
        if (currentlyRemovedMovementPoints == 3)
        {
            gameObject.GetComponent<PlayerInformation>().Slow3 = true;
        }*/
    }
    public void ApplyDebuff(string debuff, GameObject DebuffApplier = null)
    {

        if (debuff == "IceSlow")
        {
            if (AvailableMovementPoints > 0)
            {
                AvailableMovementPoints--;
                currentlyRemovedMovementPoints++;
            }
            officiallyRemovedMovementPoints++;
            //gameObject.GetComponent<PlayerInformation>().Slow1 = true;
            transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().SetBool("iceFreeze", true);
            IceSlow = false;
        }
        if (debuff == "OilSlow")
        {
            if (AvailableMovementPoints - 2 <= 0)
            {
                currentlyRemovedMovementPoints += AvailableMovementPoints;
                AvailableMovementPoints = 0;
            }
            else if (AvailableMovementPoints > 0)
            {
                AvailableMovementPoints -= 2;
                currentlyRemovedMovementPoints += 2;
            }
            officiallyRemovedMovementPoints += 2;
            //gameObject.GetComponent<PlayerInformation>().Slow2 = true;
            transform.Find("VFX").Find("VFXOilSlow").GetComponent<Animator>().SetBool("oilSlow", true);
            OilSlow = false;
        }
        if (debuff == "Stun")
        {
            isDisabled = true;
            gameObject.GetComponent<PlayerInformation>().Debuffs.Add(new Debuff("Stun", DebuffApplier));
            transform.Find("VFX").Find("VFXStun").gameObject.SetActive(true);
            Stun = false;
            if (GetComponent<PlayerInformation>().IsCreatingWhiteField)
            {
                GetComponent<CreateWhiteField>().OnTurnStart();//sunaikina white field
            }
        }
        gameObject.GetComponent<PlayerInformation>().Slow1 = false;
        gameObject.GetComponent<PlayerInformation>().Slow2 = false;
        gameObject.GetComponent<PlayerInformation>().Slow3 = false;
        if (officiallyRemovedMovementPoints == 1)
        {
            gameObject.GetComponent<PlayerInformation>().Slow1 = true;

        }
        if (officiallyRemovedMovementPoints == 2)
        {
            gameObject.GetComponent<PlayerInformation>().Slow2 = true;

        }
        if (officiallyRemovedMovementPoints == 3)
        {
            gameObject.GetComponent<PlayerInformation>().Slow3 = true;
        }
        if (GetComponent<PlayerInformation>().Blocker && isDisabled)
        {
            GetComponent<ActionManager>().FindActionByName("Block").OnTurnStart();
        }
    }
    public void RemoveDebuff(string debuff)
    {
        if (debuff == "Slows")
        {
            if (!GetComponent<ActionManager>().hasSlowAbilityBeenCast)
            {
                AvailableMovementPoints += currentlyRemovedMovementPoints;
            }
            currentlyRemovedMovementPoints=0;
            officiallyRemovedMovementPoints=0;
            transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().SetBool("iceFreeze", false);
            transform.Find("VFX").Find("VFXOilSlow").GetComponent<Animator>().SetBool("oilSlow", false);
            gameObject.GetComponent<PlayerInformation>().Slow1 = false;
            gameObject.GetComponent<PlayerInformation>().Slow2 = false;
            gameObject.GetComponent<PlayerInformation>().Slow3 = false;
        }
    }
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
            bool isGround = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfPlayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isTileInFogOfWar;
            if (isGround && GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer).GetComponent<HighlightTile>().FogOfWarTile.activeSelf)
            {
                isTileInFogOfWar = true;
            }
            else
            {
                isTileInFogOfWar = false;
            }
            //bool isTheSameTeam = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, blockingLayer).GetComponent<PlayerInformation>().CharactersTeam == GetComponent<PlayerInformation>().CharactersTeam;
            /* if (v2)
             { //adds player to list
                 GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, blockingLayer);
                 this.AvailableMovementTiles[movementIndex].Add(AddableObject);
             }
             else*/
            if (!isTileInFogOfWar && isGround && (!isBlockingLayer || (isPlayer && (isAllegianceSame(GetSpecificGroundTile(middleTile, x.Item1, x.Item2, blockingLayer)) || DoesCharacterHaveBlessing("Spectral")))))
                //GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(GetSpecificGroundTile(middleTile, x.Item1, x.Item2, blockingLayer).GetComponent<PlayerInformation>().CharactersTeam) ==
                    //GameObject.Find("GameInformation").GetComponent<PlayerTeams>().FindTeamAllegiance(GetComponent<PlayerInformation>().CharactersTeam))))
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableMovementTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    private bool CheckIfPlayer(GameObject tile, int x, int y, LayerMask chosenLayer)
    {
        Vector3 firstPosition = tile.transform.position + new Vector3(0f, 0.5f, 0f) + new Vector3(x, y, 0f);
        Vector3 secondPosition = firstPosition + new Vector3(0.1f, 0f, 0f);
        raycast = Physics2D.Linecast(firstPosition, secondPosition, chosenLayer);
        if (raycast.transform == null)
        {
            return false;
        }
        else if (raycast.transform.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    public override bool CanGridBeEnabled()
    {
        if (!isDisabled && AvailableMovementPoints > 0)
        {
            return true;
        }
        else return false;
    }
    public override void EnableGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = "Movement";
        Debug.Log("disable changing state");
        if (CanGridBeEnabled())
        {
            CreateGrid();
            HighlightMovement();
        }
    }
    public override void CreateGrid()
    {
        if (!isDisabled)
        {
            this.AvailableMovementTiles.Clear();
            if (AvailableMovementPoints > 0)
            {
                this.AvailableMovementTiles.Add(new List<GameObject>());
                AddSurroundingsToList(transform.gameObject, 0);
            }

            for (int i = 1; i <= AvailableMovementPoints - 1; i++)
            {
                this.AvailableMovementTiles.Add(new List<GameObject>());

                foreach (var tileInPreviousList in this.AvailableMovementTiles[i - 1])
                {
                    AddSurroundingsToList(tileInPreviousList, i);
                }
            }
        }
    }
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableMovementTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!CheckIfPlayer(tile, 0, 0, blockingLayer))
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(false);
                }
            }
        }
    }
    public void RemoveAvailableMovementPoints(GameObject tile)
    {
        for (int i = 0; i < this.AvailableMovementTiles.Count; i++)
        {
            if (this.AvailableMovementTiles[i].Find(x => x == tile) != null)
            {
                AvailableMovementPoints -= (i + 1);
                break;
            }
        }
    }
    public int MovementPointsToReachDestination(GameObject tile)
    {
        for (int i = 0; i < this.AvailableMovementTiles.Count; i++)
        {
            if (this.AvailableMovementTiles[i].Find(x => x == tile) != null)
            {
                return (i + 1);
            }
        }
        return 0;
    }
    public void HighlightOuter()
    {
        if (AvailableMovementPoints != 0)
        {
            foreach (GameObject tile in this.AvailableMovementTiles[this.AvailableMovementTiles.Count - 1])
            {
                if (!CheckIfPlayer(tile, 0, 0, blockingLayer))//(!tile.CompareTag("Player"))
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                    tile.GetComponent<HighlightTile>().activeState = "Movement";
                    tile.GetComponent<HighlightTile>().ChangeBaseColor();
                }
            }
            for (int i = this.AvailableMovementTiles.Count - 1; i > 0; i--)
            {
                foreach (GameObject tile in this.AvailableMovementTiles[i - 1])
                {
                    if (!CheckIfPlayer(tile, 0, 0, blockingLayer))
                    {
                        tile.GetComponent<HighlightTile>().SetHighlightBool(false);
                    }
                }
            }
        }
    }
    public void HighlightMovement()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableMovementTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!CheckIfPlayer(tile, 0, 0, blockingLayer))
                {
                    tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                    tile.GetComponent<HighlightTile>().activeState = "Movement";
                    tile.GetComponent<HighlightTile>().ChangeBaseColor();
                }
            }
        }
    }
    //Slows and stuns
    public void RefillMovementPoints()
    {
        AvailableMovementPoints = MovementPoints;
    }

    public override void OnTurnEnd()
    {
        RefillMovementPoints();
        currentlyRemovedMovementPoints = 0;
        officiallyRemovedMovementPoints = 0;
        gameObject.GetComponent<PlayerInformation>().Slow1 = false;
        gameObject.GetComponent<PlayerInformation>().Slow2 = false;
        gameObject.GetComponent<PlayerInformation>().Slow3 = false;
        gameObject.GetComponent<PlayerInformation>().Debuffs.Remove("Stun");
        gameObject.GetComponent<PlayerInformation>().CantMove = false;
        //
        gameObject.GetComponent<PlayerInformation>().Disarmed = false;
        //
        if (transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().isActiveAndEnabled)
        {
            transform.Find("VFX").Find("VFXIceFreeze").GetComponent<Animator>().SetBool("iceFreeze", false);
            transform.Find("VFX").Find("VFXOilSlow").GetComponent<Animator>().SetBool("oilSlow", false);
            transform.Find("VFX").Find("VFXStun").gameObject.SetActive(false);

        }
        isDisabled = false;
        //isItMyTurn = false;
    }
    //Arrows
    public void MakeWayList(GameObject finalTile)
    {
        CreateWayList(finalTile);
        HighlightWayTiles();
    }
    public void CreateWayList(GameObject finalTile)
    {
        if (AreTheseTilesNeighbours(finalTile, PreferredWayTile) && FindIndexOfTile(finalTile) - 1 == FindIndexOfTile(PreferredWayTile))
        {
            WayTilesList.Add(finalTile); //jei vienu i prieki
        }
        else if (WayTilesList.Count > 1 && AreTheseTilesNeighbours(finalTile, PreferredWayTile) && finalTile == WayTilesList[1])
        {
            WayTilesList.Remove(PreferredWayTile); //jei vienu atgal
        }
        else
        {
            WayTilesList.Clear();
            WayTilesList.Add(finalTile);
            GameObject currentTile = finalTile;
            for (int i = FindIndexOfTile(finalTile); i >= 0; i--)
            {
                var directionVectors = new List<(int, int)>
            {
                (1, 0),
                (0, 1),
                (-1, 0),
                (0, -1)
            };
                {
                    foreach (var x in directionVectors)
                    {
                        if (CheckIfSpecificLayer(currentTile, x.Item1, x.Item2, groundLayer) && IsTileInThisIndex(GetSpecificGroundTile(currentTile, x.Item1, x.Item2, groundLayer), i - 1))
                        {
                            currentTile = GetSpecificGroundTile(currentTile, x.Item1, x.Item2, groundLayer);
                            WayTilesList.Add(currentTile);
                            break;
                        }
                    }
                }
            }
        }
    }
        private int FindIndexOfTile(GameObject tile) //kuriame ejime yra sis tile
    {
        int index = -1;
        for (int i = 0; i < AvailableMovementTiles.Count; i++)
        {
            for (int j = 0; j < AvailableMovementTiles[i].Count; j++)
            {
                if (AvailableMovementTiles[i][j] == tile)
                {
                    return i;
                }
            }
        }
        return index;
    }
    private bool IsTileInThisIndex(GameObject tile, int index)
    {
        if (index >= 0)
        {
            for (int j = 0; j < AvailableMovementTiles[index].Count; j++)
            {
                if (AvailableMovementTiles[index][j] == tile)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private int MovementIndexOfWayTile(GameObject tile)
    {
        for (int i = 0; i < AvailableMovementTiles.Count; i++)
        {
            for (int j = 0; j < AvailableMovementTiles[i].Count; j++)
            {
                if (AvailableMovementTiles[i][j] == tile)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    private void HighlightWayTiles()
    {
        SortWayTilesList();
        foreach (GameObject tile in WayTilesList)
        {
            tile.GetComponent<HighlightTile>().ArrowTile.SetActive(true);
        }
        SetWayTileAnimations();
        //atjungia visas rodykles jei netycia ijunge ten kur nereikia
        foreach (List<GameObject> MovementTileList in this.AvailableMovementTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!IsTileInWayTilesList(tile))
                {
                    tile.GetComponent<HighlightTile>().ArrowTile.SetActive(false);
                }
            }
        }
    }
    private bool IsTileInWayTilesList(GameObject givenTile)
    {
        foreach (GameObject tile in WayTilesList)
        {
            if (tile == givenTile)
            {
                return true;
            }
        }
        return false;
    }
    public void DisableWayTiles()
    {
        foreach (GameObject tile in WayTilesList)
        {
            tile.GetComponent<HighlightTile>().ArrowTile.SetActive(false);
        }
    }
    private bool AreTheseTilesNeighbours(GameObject tile1, GameObject tile2)
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
            if (CheckIfSpecificLayer(tile1, x.Item1, x.Item2, groundLayer) && GetSpecificGroundTile(tile1, x.Item1, x.Item2, groundLayer) == tile2)
            {
                return true;
            }
        }
        return false;
    }
    private void SetWayTileAnimations()
    {
        if (WayTilesList.Count != 0)
        {
            WayTilesList.Insert(0, GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer));
            for (int i = 0; i < WayTilesList.Count; i++)
            {
                if (i == WayTilesList.Count - 1)
                {
                    WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("End", true);
                    WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("Corner", false);
                    WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetFloat("Direction", GetEndTileDirection(WayTilesList[i - 1], WayTilesList[i]));
                }
                if (i > 0 && i < WayTilesList.Count - 1)
                {
                    int transitionDirection = GetTransitionTileDirection(WayTilesList[i - 1], WayTilesList[i], WayTilesList[i + 1]);
                    if (transitionDirection == -2)
                    {
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("End", false);
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("Corner", false);
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetFloat("Direction", 1);
                    }
                    else if (transitionDirection == -1)
                    {
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("End", false);
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("Corner", false);
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetFloat("Direction", 2);
                    }
                    else
                    {
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("End", false);
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetBool("Corner", true);
                        WayTilesList[i].GetComponent<HighlightTile>().ArrowTile.GetComponent<Animator>().SetFloat("Direction", transitionDirection);
                    }
                }
            }
            WayTilesList.Remove(GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer));
        }
    }
    private int GetEndTileDirection(GameObject fromWhere, GameObject toWhere)
    {
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
            i++;
            if (CheckIfSpecificLayer(fromWhere, x.Item1, x.Item2, groundLayer) && GetSpecificGroundTile(fromWhere, x.Item1, x.Item2, groundLayer) == toWhere)
            {
                return i;
            }
        }
        return 1;
    }
    private int GetTransitionTileDirection(GameObject previousTile, GameObject middleTile, GameObject NextTile)
    {
        int index1 = GetEndTileDirection(middleTile, previousTile);
        int index2 = GetEndTileDirection(middleTile, NextTile);
        var horizontalVerticalVectors = new List<(int, int)>
            {
                (1, 3),
                (2, 4),
            };
        int i = 0;
        foreach (var x in horizontalVerticalVectors)
        {
            i++;
            if (((index1 == x.Item1 && index2 == x.Item2) || (index1 == x.Item2 && index2 == x.Item1)) && i == 1)
            {
                return -2; //horizontal
            }
            if (((index1 == x.Item1 && index2 == x.Item2) || (index1 == x.Item2 && index2 == x.Item1)) && i == 2)
            {
                return -1; //vertical
            }
        }
        var cornerVectors = new List<(int, int)>
            {
                (1, 2),
                (2, 3),
                (3, 4),
                (4, 1),
            };
        int j = 0;
        foreach (var x in cornerVectors)
        {
            j++;
            if ((index1 == x.Item1 && index2 == x.Item2) || (index1 == x.Item2 && index2 == x.Item1))
            {
                return j; //cornerIndex
            }
        }
        return 1;
    }
    private void SortWayTilesList()
    {
        List<GameObject> SortedWayTilesList = new List<GameObject>();
        for (int i = 0; i < WayTilesList.Count; i++)
        {
            foreach (GameObject tile in WayTilesList)
            {
                if (MovementIndexOfWayTile(tile) == i)
                {
                    SortedWayTilesList.Add(tile);
                    break;
                }
            }
        }
        WayTilesList = SortedWayTilesList;
    }
    public void ClearWayList()
    {
        WayTilesList.Clear();
    }
    public GameObject PickUpWayTiles()
    {
        foreach (GameObject tile in WayTilesList)
        {
            List<GameObject> PickedConsumables = new List<GameObject>();

            while (CheckIfSpecificLayer(tile, 0, 0, consumablesLayer) &&
                (!CheckIfSpecificLayer(tile, 0, 0, blockingLayer))) //||
                                                                    //GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().CharactersTeam == GetComponent<PlayerInformation>().CharactersTeam)) //jei tai saviskis, vis tiek paimtu
            {
                GameObject ConsumableTile = GetSpecificGroundTile(tile, 0, 0, consumablesLayer);
                if (PickedConsumables.Contains(ConsumableTile))
                {
                    ConsumableTile.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    PickedConsumables.Add(ConsumableTile);
                }
                //Debug.Log(ConsumableTile);
                ConsumableTile.GetComponent<ConsumableManager>().ConsumableScript.PickUp(transform.gameObject);
                if (ConsumableTile.GetComponent<ConsumableManager>().ConsumableScript.Stop && CheckIfSpecificLayer(ConsumableTile, 0, 0, groundLayer))
                {
                    //transform.position = ConsumableTile.transform.position;
                    GameObject TileToStop = GetSpecificGroundTile(ConsumableTile, 0, 0, groundLayer);
                    return TileToStop;
                }
            }
            foreach(GameObject consumable in PickedConsumables)
            {
                if (consumable != null)
                {
                    consumable.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
        return null;
    }
    public bool AreThereConsumablesOnTheWay()
    {
        foreach (GameObject tile in WayTilesList)
        {
            if (CheckIfSpecificLayer(tile, 0, 0, consumablesLayer) &&
                (!CheckIfSpecificLayer(tile, 0, 0, blockingLayer)))
            {
                return true;
            }
        }
        return false;
    }
}
