using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IsolatedStrike : BaseAction
{
    //private string actionStateName = "IsolatedStrike";
    //public int attackDamage = 60;
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 8;
    public int isolationDamage = 7;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "IsolatedStrike";
        // AttackAbility = true;
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
            HighlightOuter();
        }
        else
        {
            transform.gameObject.GetComponent<PlayerInformation>().currentState = "Movement";
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
    }
    */
    /*
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
            GameObject target = GetSpecificGroundTile(position).GetCurrentCharacter();
            int bonusDamage = 0;
            //Isolation
            if (isTargetIsolated(position))
            { 
                bonusDamage += isolationDamage;
            }

            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            FinishAbility();
            
        }
    }
    public bool canTileBeClicked(Vector3 position)
    {
        if ((CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(position, 0, 0, blockingLayer, "Wall"))
            && !isAllegianceSame(position) && !GetComponent<PlayerInformation>().CantAttackCondition)
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        int showMinDamage = minAttackDamage;
        int shownMaxDamage = maxAttackDamage;
        if (isTargetIsolated(tile.transform.position))
        {
            showMinDamage += isolationDamage;
            shownMaxDamage += isolationDamage;
        }
        EnableDamagePreview(tile, showMinDamage, shownMaxDamage);
    }
    bool isTargetIsolated(Vector3 position)
    {
        int isolationNumber = 0;
        var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        foreach (var x in directionVectors)
        {
            if (CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") && CheckIfSpecificTag(position, x.Item1, x.Item2, blockingLayer, "Player") &&
                isAllegianceSame(GetSpecificGroundTile(position).GetCurrentCharacter(), GetSpecificGroundTile(position).GetCurrentCharacter(), blockingLayer))
            {
                isolationNumber++;
            }
        }
        if (isolationNumber == 0)
        {
            return true;
        }
        return false;
    }
}
