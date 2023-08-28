using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Execute : BaseAction
{
    //private string actionStateName = "Execute";
    public int minimumDamage = 4;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "Execute";
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
            //bool isWall = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Wall");
            //bool isMiddleTileWall = CheckIfSpecificTag(middleTile, 0, 0, blockingLayer, "Wall");
            if (isGroundLayer && (!isBlockingLayer || isPlayer))// && !isMiddleTileWall)
            {
                GameObject AddableObject = GetSpecificGroundTile(middleTile, x.Item1, x.Item2, groundLayer);
                this.AvailableTiles[movementIndex].Add(AddableObject);
            }
        }
    }
    */
    public override void EnableGrid()
    {
        if (CanGridBeEnabled())
        {
            CreateGrid();
            HighlightOuter();
        }
        else
        {
            // transform.gameObject.GetComponent<PlayerInformation>().currentState = "Movement";
            Debug.Log("Current state is not changing anymore in baseAction exapmle: BindingRitual");
        }

    }
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
    */
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
    /*
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
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            GameObject target = GetSpecificGroundTile(position).GetCurrentCharacter();
           // int damage = ExecuteDamage(target);
            //dodgeActivation(ref damage, target);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
           // target.GetComponent<PlayerInformation>().DealDamage(damage, false, gameObject);
            if(target.GetComponent<PlayerInformation>().health <= 0)
            {
                transform.position = position + new Vector3(0f, 0f, -1f);
                GetComponent<PlayerInformation>().Heal(5, false);
                if (DoesCharacterHaveBlessing("Feast"))
                {
                    minimumDamage+=2;
                }
            }
            else 
            {
                //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").
                    gameObject.GetComponent<Animator>().SetTrigger("red2");
            }
            FinishAbility();
        }
    }
   // private int ExecuteDamage(GameObject target) 
   // {
       // int damage = minimumDamage + Mathf.FloorToInt(float.Parse((
           // (target.GetComponent<PlayerInformation>().MaxHealth) * 0.15
          //  ).ToString()));

      //  return damage;
  //  }
    public bool CanTileBeClicked(Vector3 position)
    {
        if ((CheckIfSpecificTag(position, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(position, 0, 0, blockingLayer, "Wall"))
            && !isAllegianceSame(position))// && !GetComponent<PlayerInformation>().CantAttackCondition)
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
       // EnableDamagePreview(tile, ExecuteDamage(GetSpecificGroundTile(tile, 0, 0, blockingLayer)));
    }
}
