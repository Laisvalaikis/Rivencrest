using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwordPush : BaseAction
{
    public int pushDamage = 15;
    public int centerDamage = 30;
    private GameObject TileThatWasClicked;

    void Start()
    {
        actionStateName = "SwordPush";
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1");
       // TileThatWasClicked = position;
    }
    public bool canTileBeClicked(GameObject tile)
    {
        return true;
    }
    public void SwordPushAnimationStart()
    {
        if (TileThatWasClicked != null)
        {
            DisableGrid();
            TileThatWasClicked.transform.Find("VFX9x9").gameObject.GetComponent<Animator>().SetTrigger("swordPush");
        }
    }

    protected override void HighlightAll()
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
    public void SwordPushAnimationEnd()
    {
        if (TileThatWasClicked != null)
        {
            var pushDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
            int pushDamageDealt = pushDamage;
            int centerDamageDealt = centerDamage;
            bool crit = IsItCriticalStrike(ref pushDamageDealt);
            if (crit) centerDamageDealt *= 2;
            foreach (var x in pushDirectionVectors)
            {
                if (CheckIfSpecificTag(TileThatWasClicked, x.Item1, x.Item2, blockingLayer, "Player"))
                {
                    ChunkData pushTarget = GetSpecificGroundTile(new Vector3(x.Item1, x.Item2,0));
                    if (!CheckIfSpecificLayer(TileThatWasClicked, x.Item1 * 2, x.Item2 * 2, blockingLayer))//pushing target
                    {
                        pushTarget.GetCurrentCharacter().transform.position = GetSpecificGroundTile(TileThatWasClicked, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.position + new Vector3(0f, 0f, -1f);
                    }
                    dodgeActivation(ref pushDamageDealt, pushTarget.GetCurrentPlayerInformation());
                    pushTarget.GetCurrentPlayerInformation().DealDamage(pushDamageDealt, crit, gameObject);
                }
            }
            if (CheckIfSpecificTag(TileThatWasClicked, 0, 0, blockingLayer, "Player"))
            {
                ChunkData centerTarget = GetSpecificGroundTile(new Vector3(0, 0, 0));
                dodgeActivation(ref centerDamageDealt, centerTarget.GetCurrentPlayerInformation());
                centerTarget.GetCurrentPlayerInformation().DealDamage(centerDamageDealt, crit, gameObject); //deal damage to center
                if (DoesCharacterHaveBlessing("Halt"))
                {
                    centerTarget.GetCurrentPlayerInformation().ApplyDebuff("CantMove");
                }
            }
            FinishAbility();
            OffTileHover(TileThatWasClicked);
            TileThatWasClicked = null;
            GameObject.Find("GameInformation").gameObject.GetComponent<GameInformation>().FakeUpdate();
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        //center
        tile.transform.Find("mapTile").Find("Direction").gameObject.SetActive(true);
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
        {
            if (GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().health - centerDamage <= 0)
            {
                tile.transform.Find("mapTile").Find("Death").gameObject.SetActive(true);
                tile.transform.Find("mapTile").Find("DamageText").position = tile.transform.position + new Vector3(0f, 0.65f, 0f);
            }
            tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
            tile.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = "-" + centerDamage.ToString();
        }
        //push directions
        var pushDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        int i = 1;
        foreach (var x in pushDirectionVectors)
        {
            if (CheckIfSpecificTag(tile, x.Item1, x.Item2, blockingLayer, "Player"))
            {
                if (!CheckIfSpecificLayer(tile, x.Item1 * 2, x.Item2 * 2, blockingLayer))
                {
                    GetSpecificGroundTile(tile, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("Arrow").gameObject.SetActive(true);
                    GetSpecificGroundTile(tile, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("Arrow").gameObject.GetComponent<Animator>().SetBool("End", true);
                    GetSpecificGroundTile(tile, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("Arrow").gameObject.GetComponent<Animator>().SetFloat("Direction", i);
                    //GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer);
                }
                if (GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer).GetComponent<PlayerInformation>().health - pushDamage <= 0)
                {
                    GetSpecificGroundTile(tile, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("Death").gameObject.SetActive(true);
                    GetSpecificGroundTile(tile, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("DamageText").position = GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer).transform.position + new Vector3(0f, 0.65f, 0f);
                }
                GetSpecificGroundTile(tile, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
                GetSpecificGroundTile(tile, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = "-" + pushDamage.ToString();
            }
            i++;
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
        {
            if (GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().health - centerDamage <= 0)
            {
                tile.transform.Find("mapTile").Find("Death").gameObject.SetActive(false);
                tile.transform.Find("mapTile").Find("DamageText").position = tile.transform.position + new Vector3(0f, 0.50f, 0f);
            }
        }
        tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(false);
        tile.transform.Find("mapTile").Find("Direction").gameObject.SetActive(false);
        //push directions
        var pushDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        int i = 1;
        foreach (var x in pushDirectionVectors)
        {
            if (CheckIfSpecificLayer(tile, x.Item1 * 2, x.Item2 * 2, groundLayer))
            {
                //print($"Disabling arrow: {GetSpecificGroundTile(tile, x.Item1 * 2, x.Item2 * 2, groundLayer).name}");
                GetSpecificGroundTile(tile, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("Arrow").gameObject.SetActive(false);
                //GetSpecificGroundTile(tile, x.Item1 * 2, x.Item2 * 2, groundLayer).transform.Find("Arrow").gameObject.GetComponent<Animator>().SetBool("End", false);
            }
            if (CheckIfSpecificTag(tile, x.Item1, x.Item2, blockingLayer, "Player"))
            {
                if (CheckIfSpecificTag(tile, x.Item1, x.Item2, blockingLayer, "Player") && GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer).GetComponent<PlayerInformation>().health - pushDamage <= 0)
                {
                    GetSpecificGroundTile(tile, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("Death").gameObject.SetActive(false);
                    GetSpecificGroundTile(tile, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("DamageText").position = GetSpecificGroundTile(tile, x.Item1, x.Item2, blockingLayer).transform.position + new Vector3(0f, 0.5f, 0f);
                }
                GetSpecificGroundTile(tile, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("DamageText").gameObject.SetActive(false);
            }
            i++;
        }
    }

    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Sword mastery"))
        {
            isAbilitySlow = false;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        SwordPush ability = new SwordPush();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;
        ability.pushDamage = this.pushDamage;
        ability.centerDamage = this.centerDamage;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Sword mastery") != null)
        {
            ability.isAbilitySlow = false;
        }

        return ability;
    }

    public override string GetDamageString()
    {
        return pushDamage + "/" + centerDamage;
    }
}
