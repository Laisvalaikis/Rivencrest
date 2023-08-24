using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IceQuake : BaseAction
{
    //private string actionStateName = "IceQuake";
    //public int minAttackDamage = 3;
    //public int maxAttackDamage = 5;
    public int rootDamage = 5;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "IceQuake";
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
        else
        {
            transform.gameObject.GetComponent<PlayerInformation>().currentState = "Movement";
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
        if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
        {
            MergedTileList.Remove(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
        }
    }
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
    /*
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
        GetSpecificGroundTile(transform.gameObject, 0, 0, groundLayer).GetComponent<HighlightTile>().SetHighlightBool(false);
    }
    */
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            GameObject target = GetSpecificGroundTile(position);
            int bonusDamage = 0;
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");
            if (target.GetComponent<PlayerInformation>().Slow1 || target.GetComponent<PlayerInformation>().Slow2 || target.GetComponent<PlayerInformation>().Slow3)
            {
                target.GetComponent<GridMovement>().RemoveDebuff("Slows");
                target.GetComponent<PlayerInformation>().ApplyDebuff("CantMove");
                bonusDamage += rootDamage;
            }
            else
            {
                target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
            }
            //clickedTile.transform.Find("mapTile").Find("VFX9x9Below").gameObject.GetComponent<Animator>().SetTrigger("iceQuake");
            DealRandomDamageToTarget(target, minAttackDamage+bonusDamage, maxAttackDamage+ bonusDamage);
            FinishAbility();
        }
    }
    /*
    public override bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")
            && !isAllegianceSame(tile, blockingLayer)))
        {
            return true;
        }
        else return false;
    }
    */
    public override void OnTileHover(GameObject tile)
    {
        int showMinDamage = minAttackDamage;
        int shownMaxDamage = maxAttackDamage;
        if (isTargetSlowed(tile))
        {
            showMinDamage += rootDamage;
            shownMaxDamage += rootDamage;
        }
        EnableDamagePreview(tile, showMinDamage, shownMaxDamage);
    }
    private bool isTargetSlowed(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")) {
            GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
            if (target.GetComponent<PlayerInformation>().Slow1 || target.GetComponent<PlayerInformation>().Slow2 || target.GetComponent<PlayerInformation>().Slow3)
            {
                return true;
            }
        }
        return false;
   }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> enemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();

            foreach (GameObject tile in MergedTileList)
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    if (!isAllegianceSame(character) && CanTileBeClicked(tile.transform.position))
                    {
                        enemyCharacterList.Add(character);
                    }
                }
            }
        }

        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (enemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
