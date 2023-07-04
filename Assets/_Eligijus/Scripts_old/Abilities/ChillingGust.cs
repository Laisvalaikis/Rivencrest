using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChillingGust : BaseAction
{
    //public int minAttackDamage = 3;
    //public int maxAttackDamage = 5;
    public int bonusBlessingDamage = 2;


    private List<GameObject> AdditionalDamageTiles = new List<GameObject>();
    private GameObject protectedAlly;

    void Start()
    {
        actionStateName = "ChillingGust";
        isAbilitySlow = false;
    }

    public override void OnTurnStart()
    {
        if (protectedAlly != null)
        {
            protectedAlly.GetComponent<PlayerInformation>().Protected = false;
            protectedAlly.transform.Find("VFX").Find("Protected").gameObject.SetActive(false);
            protectedAlly.GetComponent<PlayerInformation>().Debuffs.Remove("Protected");
            protectedAlly = null;
        }
    }
    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {
            base.ResolveAbility(clickedTile);
            if (isAllegianceSame(clickedTile))
            {
                GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
                target.GetComponent<PlayerInformation>().Protected = true;
                target.transform.Find("VFX").Find("Protected").gameObject.SetActive(true);
                if (target.GetComponent<PlayerInformation>().Debuffs.Contains("Protected")) //Dealing with WhiteField
                {
                    target.GetComponent<PlayerInformation>().Debuffs.Remove("Protected");
                }
                target.GetComponent<PlayerInformation>().Debuffs.Add(new Debuff("Protected", gameObject));
                protectedAlly = target;
                if (DoesCharacterHaveBlessing("Healing winds"))
                {
                    int randomHeal = Random.Range(3, 5);
                    bool crit = IsItCriticalStrike(ref randomHeal);
                    target.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
                }
            }
            else
            {
                transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
                //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");

                GameObject target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
                int bonusDamage = 0;
                if (DoesCharacterHaveBlessing("Harsh winds"))
                {
                    bonusDamage = bonusBlessingDamage;
                }
                DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
                target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                //target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
                //clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("crowAttack");
                clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("white2");
                GetSpecificGroundTile(target, 0, 0, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
                if (DoesCharacterHaveBlessing("Tempest"))
                {
                    CreateDamageTileList(clickedTile);
                    foreach (GameObject tile in AdditionalDamageTiles)
                    {
                        if (canTileBeClicked(tile))
                        {
                            target = GetSpecificGroundTile(clickedTile, 0, 0, blockingLayer);
                            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                            target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                        }
                        tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
                    }
                }
            }
            FinishAbility();
        }
    }
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
    public override bool canTileBeClicked(GameObject tile)
    {
        return CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player");
    }
    public void CreateDamageTileList(GameObject clickedTile)
    {
        AdditionalDamageTiles.Clear();
        var spellDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        foreach (var x in spellDirectionVectors)
        {
            if (CheckIfSpecificLayer(clickedTile, x.Item1, x.Item2, groundLayer) && (!CheckIfSpecificLayer(clickedTile, x.Item1, x.Item2, blockingLayer) || CheckIfSpecificTag(clickedTile, x.Item1, x.Item2, blockingLayer, "Player")))
            {
                AdditionalDamageTiles.Add(GetSpecificGroundTile(clickedTile, x.Item1, x.Item2, groundLayer));
            }
        }

    }
    public override void OnTileHover(GameObject tile)
    {
        if (isAllegianceSame(tile))
        {
            EnableTextPreview(tile, "PROTECT");
        }
        else
        {
            int bonusDamage = 0;
            if (DoesCharacterHaveBlessing("Harsh winds"))
            {
                bonusDamage = bonusBlessingDamage;
            }
            EnableDamagePreview(tile, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        }
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
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
        int actionChanceNumber = Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 1 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        else if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 40)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
