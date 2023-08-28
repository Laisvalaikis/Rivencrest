using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlam : BaseAction
{
    //private string actionStateName = "GroundSlam";

    //public int minAttackDamage = 3;
    //public int maxAttackDamage = 4;
    private bool isAbilityActive = false;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "GroundSlam";
        isAbilitySlow = false;
    }
    //private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    //{
    //    GameObject AddableObject = GetSpecificGroundTile(middleTile, 0, 0, groundLayer); //kad galima butu pasirinkt tik save
    //    this.AvailableTiles[movementIndex].Add(AddableObject);
    //}
    
    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        if (AttackRange > 0)
        {
            this.AvailableTiles.Add(new List<GameObject>());

            if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
            {
                AvailableTiles[0].Add(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
            }
            //AddSurroundingsToList(gameObject, 0);
        }

        for (int i = 1; i <= AttackRange - 1; i++)
        {
            this.AvailableTiles.Add(new List<GameObject>());

            foreach (var tileInPreviousList in this.AvailableTiles[i - 1])
            {
                //AddSurroundingsToList(tileInPreviousList, i);
            }
        }

        //MergeIntoOneList();
        MergedTileList.Add(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
    }

    protected override void HighlightAll()
    {
        foreach (GameObject tile in MergedTileList)
        {
            tile.GetComponent<HighlightTile>().SetHighlightBool(true);
            tile.GetComponent<HighlightTile>().activeState = actionStateName;
            tile.GetComponent<HighlightTile>().ChangeBaseColor();
            tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
            tile.GetComponent<HighlightTile>().canAbilityTargetYourself = true;
        }
    }
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (isAbilityActive && GetComponent<PlayerInformation>().health > 0)
        {
            StartCoroutine(ExecuteAfterTime(0.4f, () =>
            {
                //
                transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");
                DealDamageToAdjacent();
                transform.Find("VFX").Find("WindBoost").gameObject.SetActive(false);
                //Blessing
                if (DoesCharacterHaveBlessing("Battle rage"))
                {
                    int randomHeal = UnityEngine.Random.Range(3, 4);
                    bool crit = IsItCriticalStrike(ref randomHeal);
                    gameObject.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
                }

                //
            }));

        }
        isAbilityActive = false;
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");
        DealDamageToAdjacent();
        isAbilityActive = true;
        transform.Find("VFX").Find("WindBoost").gameObject.SetActive(true);
        FinishAbility();
    }
    private void DealDamageToAdjacent()
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
            //Ground impact vfx
            if (CheckIfSpecificLayer(gameObject, x.Item1, x.Item2, groundLayer))
            {
                GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("burgundy3");
            }
            //Deal damage
            if (CheckIfSpecificTag(gameObject, x.Item1, x.Item2, blockingLayer, "Player") && (!isAllegianceSame(GetSpecificGroundTile(gameObject, x.Item1, x.Item2, blockingLayer)) || friendlyFire))
            {
                GameObject target = GetSpecificGroundTile(gameObject, x.Item1, x.Item2, blockingLayer);
                DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            }
        }
    }

    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
    }
    public override void EnableDamagePreview(GameObject tile, List<GameObject> tileList, int minAttackDamage, int maxAttackDamage = -1)
    {
        foreach(GameObject tileInList in tileList)
        {
            if(tileInList != GetSpecificGroundTile(gameObject, 0, 0, groundLayer))
            {
                EnableDamagePreview(tileInList, minAttackDamage, maxAttackDamage);
            }
            else
            {
                EnableTextPreview(tileInList, "");
            }
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
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
        if (isActionPossible && actionChanceNumber <= 100)
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
