using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchPlaces : BaseAction
{
    //private string actionStateName = "SwitchPlaces";


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "SwitchPlaces";
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
        //Merging into one list
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
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(true);
    }
    */
    public override void ResolveAbility(Vector3 position)
    {
        var currentPosition = transform.position;
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            GameObject target = GetSpecificGroundTile(position);
            if (!isAllegianceSame(target))
            {
                if (DoesCharacterHaveBlessing("Stay there"))
                {
                    target.GetComponent<PlayerInformation>().ApplyDebuff("CantMove");
                }
                else
                {
                    target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                }
            }
            transform.position = target.transform.position;
            target.transform.position = currentPosition;
            GameObject.Find("GameInformation").GetComponent<GameInformation>().FocusSelectedCharacter(gameObject);
            FinishAbility();
        }
    }
    private new bool CanTileBeClicked(Vector3 position)
    {
        if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player"))
        {
            return true;
        }
        return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "SWITCH");
    }
}
