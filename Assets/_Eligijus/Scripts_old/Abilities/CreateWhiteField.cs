﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateWhiteField : BaseAction
{
    //private string actionStateName = "CreateWhiteField";
    public GameObject WhiteFieldPrefab;
    private bool _isWhiteFieldActive = false;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();//WhiteField list

    void Start()
    {
        actionStateName = "CreateWhiteField";
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

    */
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (_isWhiteFieldActive)
        {

            foreach (List<GameObject> movementTileList in this.AvailableTiles)
            {
                foreach (GameObject tile in movementTileList)
                {
                    if (CheckIfSpecificLayer(tile, 0, 0, whiteFieldLayer))
                    {
                        GetSpecificGroundTile(tile, 0, 0, whiteFieldLayer).GetComponent<WhiteField>().RemoveOwner(gameObject);
                    }
                }
            }
            _isWhiteFieldActive = false;
            GetComponent<PlayerInformation>().IsCreatingWhiteField = false;
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", false);
            AvailableTiles.Clear();
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            FinishAbility();
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spellToBool");
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", true);
            if (AvailableTiles.Count == 0)
            {
                CreateGrid();
            }
            foreach (List<GameObject> movementTileList in this.AvailableTiles)
            {
                foreach (GameObject tile in movementTileList)
                {
                    if (tile != GetSpecificGroundTile(gameObject, 0, 0, groundLayer))
                    {
                        if (CheckIfSpecificLayer(tile, 0, 0, whiteFieldLayer))
                        {
                            if (GetSpecificGroundTile(tile, 0, 0, whiteFieldLayer).GetComponent<WhiteField>().Owners.Find(x => x == gameObject) == null)
                            {
                                GetSpecificGroundTile(tile, 0, 0, whiteFieldLayer).GetComponent<WhiteField>().Owners.Add(gameObject);
                            }
                        }
                        else
                        {
                            GameObject newWhiteField = Instantiate(WhiteFieldPrefab, tile.transform.position + new Vector3(0f, 0f, 1f), Quaternion.identity);
                            newWhiteField.GetComponent<WhiteField>().Owners.Add(gameObject);
                        }
                    }
                }
            }
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().ChangeVisionTiles();
            _isWhiteFieldActive = true;
            GetComponent<PlayerInformation>().IsCreatingWhiteField = true;
            //Blessing
            if (DoesCharacterHaveBlessing("Breezy defence"))
            {
                gameObject.GetComponent<PlayerInformation>().BarrierProvider = gameObject;
                gameObject.transform.Find("VFX").Find("VFXBool").GetComponent<Animator>().SetTrigger("shieldStart");
            }
        }
    }
    public bool CanTileBeClicked(GameObject tile)
    {
        return CheckIfSpecificLayer(tile, 0, 0, groundLayer) || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player");
    }
    public override void OnTileHover(GameObject tile)
    {
        foreach (List<GameObject> movementTileList in this.AvailableTiles)
        {
            EnableTextPreview(tile, movementTileList, "");
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        foreach (List<GameObject> movementTileList in this.AvailableTiles)
        {
            DisablePreview(tile, movementTileList);
        }
    }
    public override GameObject PossibleAIActionTile()
    {
        bool isEnemyNearby = false;
        List<GameObject> allyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            List<GameObject> enemyList = GetComponent<AIBehaviour>().GetCharactersInGrid(4);
            foreach (GameObject character in enemyList)
            {
                if (!isAllegianceSame(character))
                {
                    isEnemyNearby = true;
                    break;
                }
            }
            List<GameObject> allyList = GetComponent<AIBehaviour>().GetCharactersInGrid(AttackRange);
            foreach (GameObject character in allyList)
            {
                if (isAllegianceSame(character) && character != gameObject)
                {
                    allyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = Random.Range(0, 100); //ar paleisti spella ar ne
        if (isEnemyNearby && allyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return allyCharacterList[Random.Range(0, allyCharacterList.Count - 1)];
        }
        return null;
    }
}
