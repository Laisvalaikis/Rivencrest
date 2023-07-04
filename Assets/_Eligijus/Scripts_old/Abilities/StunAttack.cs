using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StunAttack : BaseAction
{
    //private string actionStateName = "StunAttack";
    //public int spellDamage = 60;
    //public int minAttackDamage = 2;
    //public int maxAttackDamage = 3;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "StunAttack";
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
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
            }
        }
    }
    */
    public override void ResolveAbility(GameObject clickedTile)
    {
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            //bool crit = IsItCriticalStrike(ref spellDamage);
            //dodgeActivation(ref spellDamage, target);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");//bus atskira animacija
            if (DoesCharacterHaveBlessing("Stunning"))
            {
                target.GetComponent<PlayerInformation>().ApplyDebuff("Stun");
            }
            else 
            {
                target.GetComponent<PlayerInformation>().ApplyDebuff("CantMove");
            }
            if (DoesCharacterHaveBlessing("Toxic metals"))
            {
                target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
            }
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            clickedTile.transform.Find("mapTile").Find("VFXImpact").gameObject.GetComponent<Animator>().SetTrigger("burgundy1");
            //target.GetComponent<PlayerInformation>().DealDamage(spellDamage, crit);
            FinishAbility();
        }
    }
    /*
    public override bool canTileBeClicked(GameObject tile)
    {
        if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")) && !isAllegianceSame(tile, blockingLayer))
        {
            return true;
        }
        else return false;
    }
    */
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "STUN");
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            CreateGrid();
            foreach (List<GameObject> MovementTileList in this.AvailableTiles)
            {
                foreach (GameObject tile in MovementTileList)
                {
                    if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
                    {
                        GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                        if (!isAllegianceSame(character) && canTileBeClicked(tile))
                        {
                            EnemyCharacterList.Add(character);
                        }
                    }

                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
