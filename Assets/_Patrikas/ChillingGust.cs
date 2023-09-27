using System.Collections.Generic;
using UnityEngine;

public class ChillingGust : BaseAction
{
    private const int MinAttackDamage = 3;
    private const int MaxAttackDamage = 5;
    private const int BonusBlessingDamage = 2;
    private List<ChunkData> _additionalDamageTiles=new List<ChunkData>();
    private GameObject _protectedAlly;

    void Start()
    {
        isAbilitySlow = false;
        AttackHighlight = new Color32(123,156, 178,255);
        AttackHighlightHover = new Color32(103, 136, 158, 255);
        CharacterOnGrid = new Color32(146, 212, 255, 255);
    }

    public override void OnTurnStart()
    {
        if (_protectedAlly != null)
        {
            _protectedAlly.GetComponent<PlayerInformation>().Protected = false;
            //protectedAlly.transform.Find("VFX").Find("Protected").gameObject.SetActive(false);
            _protectedAlly.GetComponent<PlayerInformation>().Debuffs.Remove("Protected");
            _protectedAlly = null;
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
            base.ResolveAbility(position);
            ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
            GameObject target = chunk.GetCurrentCharacter();
            PlayerInformation clickedPlayerInformation = target.GetComponent<PlayerInformation>();
            
            if (IsAllegianceSame(chunk))
            {
                clickedPlayerInformation.Protected = true;
                //target.transform.Find("VFX").Find("Protected").gameObject.SetActive(true);
                if (clickedPlayerInformation.Debuffs.Contains("Protected")) //Dealing with WhiteField
                {
                    clickedPlayerInformation.Debuffs.Remove("Protected");
                }
                clickedPlayerInformation.Debuffs.Add(new Debuff("Protected", gameObject));
                _protectedAlly = target;
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
                    bonusDamage = BonusBlessingDamage;
                }
                DealRandomDamageToTarget(chunk, MinAttackDamage + bonusDamage, MaxAttackDamage + bonusDamage);
                clickedPlayerInformation.ApplyDebuff("IceSlow");
                //target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
                //clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("crowAttack");
                //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("white2");
                //GetSpecificGroundTile(target, 0, 0, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
                if (DoesCharacterHaveBlessing("Tempest"))
                {
                    CreateDamageTileList(chunk);
                    foreach (ChunkData c in _additionalDamageTiles)
                    {
                        if (CanTileBeClicked(c))
                        {
                            DealRandomDamageToTarget(c, minAttackDamage, maxAttackDamage);
                            c.GetCurrentCharacter().GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                        }
                        //tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
                    }
                }
            }
            FinishAbility();
    }
    
    private void CreateDamageTileList(ChunkData chunk)
    {
        _additionalDamageTiles.Clear();
        var spellDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        foreach (var x in spellDirectionVectors)
        {
            if (CheckIfSpecificInformationType(chunk, InformationType.Player))
            {
                _additionalDamageTiles.Add(chunk);
            }
        }
    }
    public void OnTileHover(Vector3 position)
    {
        // if (IsAllegianceSame(position))
        // {
        //     //EnableTextPreview(position, "PROTECT");
        // }
        // else
        // {
        //     int bonusDamage = 0;
        //     if (DoesCharacterHaveBlessing("Harsh winds"))
        //     {
        //         bonusDamage = BonusBlessingDamage;
        //     }
        //     //EnableDamagePreview(tile, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        // }
    }
    
}
