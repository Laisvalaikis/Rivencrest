using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class
    EnvironmentalHazard : MonoBehaviour
{
    public HazardAction action;

    void Start()
    {
        GameObject.Find("GameInformation").GetComponent<GameInformation>().environmentalHazards.Add(this);
    }

    public void ActivateAction(bool onTurnEnd)
    {
        if(BaseAction.CheckIfSpecificTag(gameObject, 0, 0, LayerMask.GetMask("BlockingLayer"), "Player"))
        {
            ChunkData target = GameTileMap.Tilemap.GetChunk(new Vector3(0, 0,0));
            if(GameObject.Find("GameInformation").GetComponent<GameInformation>().ActiveTeam == target.GetCurrentPlayerInformation().CharactersTeam)
            {
                if (onTurnEnd)
                {
                    switch (action)
                    {
                        case HazardAction.DealDamage:
                            //Debug.Log("Environmental hazard: DealDamage");
                            DealDamageToCharacter(target.GetCurrentCharacter());
                            break;
                        case HazardAction.Poison:
                            //Debug.Log("Environmental hazard: Poison");
                            PoisonCharacter(target.GetCurrentCharacter());
                            break;
                        case HazardAction.Aflame:
                            //Debug.Log("Environmental hazard: Aflame");
                            break;
                        case HazardAction.Heal:
                            //Debug.Log("Environmental hazard: Heal");
                            break;
                    }
                }
                else
                {
                    switch (action)
                    {
                        case HazardAction.Freeze:
                            //Debug.Log("Environmental hazard: Freeze");
                            FreezeCharacter(target.GetCurrentCharacter());
                            break;
                        case HazardAction.DamageBoost:
                            //Debug.Log("Environmental hazard: DamageBoost");
                            DamageBoostCharacter(target.GetCurrentCharacter());
                            break;
                        case HazardAction.Teleport:
                            //Debug.Log("Environmental hazard: Teleport");
                            break;
                    }
                }
            }
        }
    }

    private void DealDamageToCharacter(GameObject target)
    {
        target.GetComponent<PlayerInformation>().DealDamage(4, false, gameObject);
        transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("orange3");
    }

    private void PoisonCharacter(GameObject target)
    {
        target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
        transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("forest3");
    }

    private void AflameCharacter(GameObject target)
    {
        //
    }

    private void FreezeCharacter(GameObject target)
    {
        target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
        transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
    }

    private void DamageBoostCharacter(GameObject target)
    {
        target.GetComponent<PlayerInformation>().Debuffs.Add(new Debuff("EnvHazardDamageBoost", (GameObject)null));
        target.GetComponent<PlayerAttack>().minAttackDamage += 3;
        target.GetComponent<PlayerAttack>().maxAttackDamage += 3;
        transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("enemySpawn");
    }

    private void HealCharacter(GameObject target)
    {
        //
    }

    private void TeleportCharacter(GameObject target)
    {
        //
    }
}

public enum HazardAction
{
    DealDamage,
    Poison,
    Aflame,
    Freeze,
    DamageBoost,
    Heal,
    Teleport
}