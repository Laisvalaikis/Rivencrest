using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PinkBarrier : BaseAction
{
    //private string actionStateName = "PinkBarrier";
   // private GameObject characterWithBarrier = null;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "PinkBarrier";
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
    */
    public override void HighlightAll()
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
    /*public override void OnTurnStart()//pradzioj ejimo
    {
        StartCoroutine(ExecuteAfterTime(0.5f, () =>
        {
            if (characterWithBarrier != null && characterWithBarrier.GetComponent<PlayerInformation>().DoesItHaveBarrier)
            {
                characterWithBarrier.GetComponent<PlayerInformation>().DoesItHaveBarrier = false;
                characterWithBarrier.transform.Find("VFX").Find("VFXBool").GetComponent<Animator>().SetTrigger("shieldEnd");
            }
            characterWithBarrier = null;
        }));
    }*/
    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().BarrierProvider = gameObject;
            //characterWithBarrier = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
            GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer).transform.Find("VFX").Find("VFXBool").GetComponent<Animator>().SetTrigger("shieldStart");
            GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer).GetComponent<GridMovement>().AvailableMovementPoints++;
            FinishAbility();

        }
    }
    public bool canTileBeClicked(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") 
            && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().BarrierProvider == null
            && isAllegianceSame(tile))
        {
            return true;
        }
        else return false;
    }
    public override GameObject PossibleAIActionTile()
    {
        bool isEnemyNearby = false;
        List<GameObject> AllyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            List<GameObject> enemyList = GetComponent<AIBehaviour>().GetCharactersInGrid(3);
            foreach (GameObject character in enemyList)
            {
                if (!isAllegianceSame(character))
                {
                    isEnemyNearby = true;
                    break;
                }
            }
            List<GameObject> AllyList = GetComponent<AIBehaviour>().GetCharactersInGrid(AttackRange);
            foreach (GameObject character in AllyList)
            {
                if (isAllegianceSame(character) && character != gameObject)
                {
                    AllyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (isEnemyNearby && AllyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return AllyCharacterList[Random.Range(0, AllyCharacterList.Count - 1)];
        }
        else if(isEnemyNearby && actionChanceNumber <= 100)
        {
            return gameObject;
        }
        return null;
    }
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Distant defence"))
        {
            AttackRange++;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        PinkBarrier ability = new PinkBarrier();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Distant defence") != null)
        {
            ability.AttackRange++;
        }

        return ability;
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
}
