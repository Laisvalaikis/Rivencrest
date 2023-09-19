using System.Collections.Generic;
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
        isAbilitySlow = false;
    }

    public override void OnTurnStart()
    {
        if (protectedAlly != null)
        {
            protectedAlly.GetComponent<PlayerInformation>().Protected = false;
            //protectedAlly.transform.Find("VFX").Find("Protected").gameObject.SetActive(false);
            protectedAlly.GetComponent<PlayerInformation>().Debuffs.Remove("Protected");
            protectedAlly = null;
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
            base.ResolveAbility(position);
            
            ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
            GameObject target = chunk.GetCurrentCharacter();
            PlayerInformation clickedPlayerInformation = target.GetComponent<PlayerInformation>();
            
            if (IsAllegianceSame(position))
            {
                clickedPlayerInformation.Protected = true;
                //target.transform.Find("VFX").Find("Protected").gameObject.SetActive(true);
                if (clickedPlayerInformation.Debuffs.Contains("Protected")) //Dealing with WhiteField
                {
                    clickedPlayerInformation.Debuffs.Remove("Protected");
                }
                clickedPlayerInformation.Debuffs.Add(new Debuff("Protected", gameObject));
                protectedAlly = target;
                if (DoesCharacterHaveBlessing("Healing winds"))
                {
                    int randomHeal = Random.Range(3, 5);
                    bool crit = IsItCriticalStrike(ref randomHeal);
                    clickedPlayerInformation.Heal(randomHeal, crit);
                }
            }
            else
            {
                //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
                //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
                int bonusDamage = 0;
                if (DoesCharacterHaveBlessing("Harsh winds"))
                {
                    bonusDamage = bonusBlessingDamage;
                }
                DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
                clickedPlayerInformation.ApplyDebuff("IceSlow");
                //target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
                //clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("crowAttack");
                //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("white2");
                //GetSpecificGroundTile(target, 0, 0, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
                /*if (DoesCharacterHaveBlessing("Tempest"))
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
                }*/
            }
            FinishAbility();
    }
    
    /*private void CreateDamageTileList(GameObject clickedTile)
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

    }*/
    public void OnTileHover(Vector3 position)
    {
        if (IsAllegianceSame(position))
        {
            //EnableTextPreview(position, "PROTECT");
        }
        else
        {
            int bonusDamage = 0;
            if (DoesCharacterHaveBlessing("Harsh winds"))
            {
                bonusDamage = bonusBlessingDamage;
            }
            //EnableDamagePreview(tile, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        }
    }
    
}
