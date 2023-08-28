﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfActionBoost : BaseAction
{
    //private string actionStateName = "AttackTwice";


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "AttackTwice";
    }
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        GameObject addableObject = GetSpecificGroundTile(middleTile, 0, 0, groundLayer); //kad galima butu pasirinkt tik save
        this.AvailableTiles[movementIndex].Add(addableObject);
    }
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
    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = "AttackTwice";
        this.AvailableTiles.Clear();
        this.AvailableTiles.Add(new List<GameObject>());
        AddSurroundingsToList(transform.gameObject, 0);
    }
    public override void DisableGrid()
    {
        foreach (List<GameObject> movementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in movementTileList)
            {
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = false;
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
            }
        }
    }

    protected override void HighlightAll()
    {
        foreach (List<GameObject> movementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in movementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
            }
        }
    }
    public override void RefillActionPoints()//pradzioj ejimo
    {
        AvailableAttacks = 2;
        AbilityPoints++;
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        //GetComponent<GridMovement>().AvailableMovementPoints++; //debatable
        AbilityPoints = 0;
        AvailableAttacks--;
        GetComponent<ActionManager>().AddAvailableAttackToAll();
        DisableGrid();
    }
}
