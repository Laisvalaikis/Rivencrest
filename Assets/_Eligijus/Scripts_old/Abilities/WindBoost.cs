using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBoost : BaseAction
{
    //private string actionStateName = "WindBoost";

    private bool isAbilityActive = false;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "WindBoost";
        isAbilitySlow = false;
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
        if (isAbilityActive)
        {
            StartCoroutine(ExecuteAfterTime(0.5f, () =>
            {
                //
                // transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
                //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
                var pushDirectionVectors = new List<(int, int)>
                {
                    (0, 0),
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
                            target.GetComponent<GridMovement>().AvailableMovementPoints++;
                            target.transform.Find("VFX").Find("VFX1x1").GetComponent<Animator>().SetTrigger("speedBoost");
                            int bonusHealing = 0;
                            if (DoesCharacterHaveBlessing("Fresh air"))//Blessing
                            {
                                bonusHealing = 3;
                            }
                            int randomHeal = UnityEngine.Random.Range(2 + bonusHealing, 4 + bonusHealing);
                            bool crit = IsItCriticalStrike(ref randomHeal);
                            target.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
                        }
                    }
                }
                transform.Find("VFX").Find("WindBoost").gameObject.SetActive(false);
                //
            }));

        }
        isAbilityActive = false;
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        base.ResolveAbility(clickedTile);
        isAbilityActive = true;
        transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
        transform.Find("VFX").Find("WindBoost").gameObject.SetActive(true);
        // transform.Find("VFX").Find("Protected").gameObject.SetActive(true);
        FinishAbility();
    }

    public override GameObject PossibleAIActionTile()
    {
        bool isActionPossible = false;
        if (CanGridBeEnabled())
        {
            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(2);
            foreach (GameObject character in characterList)
            {
                if (!isAllegianceSame(character))
                {
                    isActionPossible = true;

                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (isActionPossible && actionChanceNumber <= 80)
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
