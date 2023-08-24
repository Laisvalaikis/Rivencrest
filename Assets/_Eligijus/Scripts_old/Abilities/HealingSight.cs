using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealingSight : BaseAction
{
    //private string actionStateName = "HealingSight";
    //public int healAmount = 40;
    public int minHealAmount = 3;
    public int maxHealAmount = 7;
    private bool canPreviewBeShown = true;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "HealingSight";
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
    public override void OnTurnEnd()
    {
        RefillActionPoints();
        GetComponent<ActionManager>().hasSlowAbilityBeenCast = false;
        GetComponent<CharacterVision>().VisionRange = 4;
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (CanTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("healingSight");
            int randomHeal = Random.Range(minHealAmount, maxHealAmount);
            bool crit = IsItCriticalStrike(ref randomHeal);
           // transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("heal");
            GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().Heal(randomHeal, crit);
            if (DoesCharacterHaveBlessing("Seeker"))
            {
                GetComponent<GridMovement>().AvailableMovementPoints ++;
            }
            if (DoesCharacterHaveBlessing("Solar explosion"))
            {
                TriggerSolarExplosion();
            }
            GetComponent<CharacterVision>().VisionRange = 6;
            FinishAbility();
        }
    }
    private void TriggerSolarExplosion() 
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
                GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("yellow1");
            }
            if (CheckIfSpecificTag(gameObject, x.Item1, x.Item2, blockingLayer, "Player"))
            {
                GameObject target = GetSpecificGroundTile(gameObject, x.Item1, x.Item2, blockingLayer);

                if (!isAllegianceSame(target))
                {
                    DealRandomDamageToTarget(target,2,3);
                }      
            }
        }
    }
    private void SetSolarExplosionHighlight(bool value)
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
            if (CheckIfSpecificTag(gameObject, x.Item1, x.Item2, blockingLayer, "Player"))
            {
                GameObject target = GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer);

                if (!isAllegianceSame(target))
                {
                    target.transform.Find("mapTile").Find("Highlight").gameObject.SetActive(value);
                }
            }
        }
    }
    public bool CanTileBeClicked(GameObject tile)
    {
        /*
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && isAllegianceSame(tile, blockingLayer))
        {
            return true;
        }
        else return false;
        */
        return true;
    }
    public override void OnTileHover(GameObject tile)
    {
        if (canPreviewBeShown)
        {
            EnableDamagePreview(tile, minHealAmount, maxHealAmount);
            if (DoesCharacterHaveBlessing("Solar explosion"))
            {
                SetSolarExplosionHighlight(true);
            }
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile);
        SetSolarExplosionHighlight(false);
        canPreviewBeShown = false;
        StartCoroutine(ExecuteAfterFrames(1, () =>
        {
            canPreviewBeShown = true;
        }));
    }
    public override GameObject PossibleAIActionTile()
    {
        if (CanGridBeEnabled())
        {
           // if(GetComponent<PlayerInformation>().health < GetComponent<PlayerInformation>().MaxHealth)
            {
                return GetSpecificGroundTile(gameObject, 0, 0, groundLayer);
            }
        }
        return null;
    }
}
