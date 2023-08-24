using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfernoOld : BaseAction
{
    //private string actionStateName = "Inferno";
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;
    private bool isAbilityActive = false;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();
    private GameObject characterTile;

    void Start()
    {
        actionStateName = "InfernoOld";
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
    */
    public override void CreateGrid()
    {
        /*
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
        */
        /*
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
            characterTile = GetSpecificGroundTile(gameObject, 0, 0, groundLayer);
        }
        */
        base.CreateGrid();
        if(CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
        {
            characterTile = GetSpecificGroundTile(gameObject, 0, 0, groundLayer);
        }
    }
    public override void DisableGrid()
    {
        if (characterTile != null)
        {
            characterTile.GetComponent<HighlightTile>().canAbilityTargetYourself = false;
            characterTile.GetComponent<HighlightTile>().SetHighlightBool(false);
        }
    }
    public override void HighlightAll()
    {
        if (characterTile != null)
        {
            characterTile.GetComponent<HighlightTile>().SetHighlightBool(true);
            characterTile.GetComponent<HighlightTile>().activeState = actionStateName;
            characterTile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
            characterTile.GetComponent<HighlightTile>().ChangeBaseColor();
        }
    }
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (isAbilityActive && GetComponent<PlayerInformation>().health > 0)
        {
            StartCoroutine(ExecuteAfterTime(0.5f, () =>
            {
                transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
                //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
                CreateGrid();
                foreach (GameObject tile in MergedTileList)
                {
                    if (canTileBeDamaged(tile))
                    {
                        GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                        int bonusDamage = 0;
                        DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
                    }
                    tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("orange1");
                    tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("burgundy3");
                }
                transform.Find("VFX").Find("WindBoost").gameObject.SetActive(false);
            }));
        }
        isAbilityActive = false;
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            isAbilityActive = true;
            transform.Find("VFX").Find("WindBoost").gameObject.SetActive(true);
            //  
            FinishAbility();
        }
    }
    public bool canTileBeClicked(GameObject tile)
    {
        return true;
    }
    private bool canTileBeDamaged(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && !isAllegianceSame(GetSpecificGroundTile(tile, 0, 0, blockingLayer)))
        {
            return true;
        }

        return false;
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 1 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[UnityEngine.Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        else if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 40)
        {
            return GetSpecificGroundTile(EnemyCharacterList[UnityEngine.Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
}
