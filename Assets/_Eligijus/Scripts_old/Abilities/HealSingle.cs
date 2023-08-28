using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealSingle : BaseAction
{
    //private string actionStateName = "HealSingle";
    //public int healAmount = 40;
    public int minHealAmount = 3;
    public int maxHealAmount = 7;


    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();
    //private List<GameObject> MergedTileList = new List<GameObject>();

    void Start()
    {
        actionStateName = "HealSingle";
        isAbilitySlow = false;
        friendlyFire = true;
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
    */
    /*
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
        //Merging into one list
        MergedTileList.Clear();
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                if (!MergedTileList.Contains(tile))
                {
                    MergedTileList.Add(tile);
                }
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
    public void HighlightAll()
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
    */
    protected override void HighlightAll()
    {
        foreach (List<GameObject> movementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in movementTileList)
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
    public override void ResolveAbility(Vector3 position)
    {
        
        if (DoesCharacterHaveBlessing("Gather round"))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("heal");
            int randomHeal = Random.Range(minHealAmount, maxHealAmount);
            bool crit = IsItCriticalStrike(ref randomHeal);
            gameObject.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile))
                {
                    randomHeal = Random.Range(minHealAmount, maxHealAmount);
                    crit = IsItCriticalStrike(ref randomHeal);
                    GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().Heal(randomHeal, crit);
                }
            }
            FinishAbility();
        }
        else if (CanTileBeClicked(position))
        {
            int randomHeal = Random.Range(minHealAmount, maxHealAmount);
            bool crit = IsItCriticalStrike(ref randomHeal);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("heal");
            GetSpecificGroundTile(position).GetComponent<PlayerInformation>().Heal(randomHeal, crit);
            FinishAbility();
        }
    }
    public bool CanTileBeClicked(GameObject tile)
    {
        if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") && isAllegianceSame(tile))
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minHealAmount, maxHealAmount);
    }
    public override void EnableDamagePreview(GameObject tile, int minAttackDamage, int maxAttackDamage = -1)
    {
        if (!tile.GetComponent<HighlightTile>().FogOfWarTile.activeSelf && CanTileBeClicked(tile))
        {
            //if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
            //    && GetSpecificGroundTile(tile, 0, 0, blockingLayer).GetComponent<PlayerInformation>().health - minAttackDamage <= 0 && canPreviewBeShown(tile))
            //{
            //    tile.transform.Find("mapTile").Find("Death").gameObject.SetActive(true);
            //    tile.transform.Find("mapTile").Find("DamageText").position = tile.transform.position + new Vector3(0f, 0.65f, 0f);
            //}
            tile.GetComponent<HighlightTile>().HighlightedByPlayerUI.GetComponent<SpriteRenderer>().color = GameObject.Find("GameInformation").GetComponent<ColorManager>().MovementHighlightHover;//tile.GetComponent<HighlightTile>().HoverHighlightColor;
                                                                                                                                                                                                   //ziurim ar random damage ar ne
            if ((CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player") || CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall")) && CanPreviewBeShown(tile.transform.position))
            {
                tile.transform.Find("mapTile").Find("DamageText").gameObject.SetActive(true);
                if (maxAttackDamage == -1)
                {
                    tile.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = "+" + minAttackDamage.ToString();
                }
                else
                {
                    tile.transform.Find("mapTile").Find("DamageText").gameObject.GetComponent<TextMeshPro>().text = minAttackDamage.ToString() + "-" + maxAttackDamage.ToString();
                }
            }
        }
    }
    public override GameObject PossibleAIActionTile()
    {
        if (CanGridBeEnabled())
        {
            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(2);

            List<GameObject> allyCharacterList = new List<GameObject>();

            foreach (GameObject character in characterList)
            {
               // if (isAllegianceSame(character) && character.GetComponent<PlayerInformation>().health < character.GetComponent<PlayerInformation>().MaxHealth)
                {
                    allyCharacterList.Add(character);
                }
            }
            if (allyCharacterList.Count > 0)
            {
                return allyCharacterList[Random.Range(0, allyCharacterList.Count - 1)];
            }
       
        }
        return null;
    }
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Far reach"))
        {
            AttackRange += 1;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        HealSingle ability = spawnedCharacter.AddComponent<HealSingle>();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;
        ability.minHealAmount = this.minHealAmount;
        ability.maxHealAmount = this.maxHealAmount;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Far reach") != null)
        {
            ability.AttackRange += 1;
        }

        return ability;
    }
}
