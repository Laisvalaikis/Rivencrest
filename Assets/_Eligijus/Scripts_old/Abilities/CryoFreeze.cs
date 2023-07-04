using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoFreeze : BaseAction
{
    //private string actionStateName = "CryoFreeze";

    private bool isAbilityActive = false;
    //public int minAttackDamage = 2;
    //public int maxAttackDamage = 4;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "CryoFreeze";
        isAbilitySlow = true;
    }
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        GameObject AddableObject = GetSpecificGroundTile(middleTile, 0, 0, groundLayer); //kad galima butu pasirinkt tik save
        this.AvailableTiles[movementIndex].Add(AddableObject);
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
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        this.AvailableTiles.Add(new List<GameObject>());
        AddSurroundingsToList(transform.gameObject, 0);
        MergeIntoOneList();
    }
    /*
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
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
                tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
            }
        }
    }
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (isAbilityActive && (GetComponent<PlayerInformation>().health > 0))
        {
            StartCoroutine(ExecuteAfterTime(0.2f, () =>
            {
                transform.Find("CharacterModel").GetComponent<Animator>().SetBool("stasis", false);
                GetComponent<PlayerInformation>().Stasis = false;
            }));
            //
            StartCoroutine(ExecuteAfterTime(0.4f, () =>
            {
                var pushDirectionVectors = new List<(int, int)>
            {
                    (1, 0),
                    (0, 1),
                    (-1, 0),
                    (0, -1)
            };
                foreach (var x in pushDirectionVectors)
                {
                    if (CheckIfSpecificLayer(gameObject, x.Item1, x.Item2, groundLayer)) //animation on ground
                    {
                        GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
                    }
                    if (CheckIfSpecificTag(gameObject, x.Item1, x.Item2, blockingLayer, "Player"))
                    {
                        GameObject target = GetSpecificGroundTile(gameObject, x.Item1, x.Item2, blockingLayer);
                        if (isAllegianceSame(target))
                        {
                            DealRandomDamageToTarget(target, minAttackDamage / 2, maxAttackDamage / 2);
                        }
                        else
                        {
                            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                        }
                    }
                }
            }));
        }
        isAbilityActive = false;
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        base.ResolveAbility(clickedTile);
        isAbilityActive = true;
        transform.Find("CharacterModel").GetComponent<Animator>().SetBool("stasis", true);
        GetComponent<PlayerInformation>().Stasis = true;
        FinishAbility();
    }
    public override bool canTileBeClicked(GameObject tile)
    {
        return true;
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(2);
            foreach (GameObject character in characterList)
            {
                if (!isAllegianceSame(character))
                {
                    EnemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 1 && GetComponent<PlayerInformation>().BarrierProvider == null && GetComponent<PlayerInformation>().BlockingAlly == null)
        {
            return gameObject;
        }
        if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 60 && GetComponent<PlayerInformation>().BarrierProvider == null && GetComponent<PlayerInformation>().BlockingAlly == null)
        {
            return gameObject;
        }
        return null;
    }
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }
}
