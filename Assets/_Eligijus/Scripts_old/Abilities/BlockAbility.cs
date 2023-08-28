using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAbility : BaseAction
{
    //private string actionStateName = "Block";
    

    private GameObject characterBeingBlocked;

    //private List<List<GameObject>> AvailableTiles = new List<List<GameObject>>();

    void Start()
    {
        actionStateName = "Block";
        isAbilitySlow = false;
    }
    /*
    private void AddSurroundingsToList(GameObject middleTile, int movementIndex)
    {
        var directionVectors = new List<(int, int)>
        {
            (AttackRange, 0),
            (0, AttackRange),
            (-AttackRange, 0),
            (0, -AttackRange)
        };

        foreach (var x in directionVectors)
        {
            bool isGround = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, groundLayer);
            bool isBlockingLayer = CheckIfSpecificLayer(middleTile, x.Item1, x.Item2, blockingLayer);
            bool isPlayer = CheckIfSpecificTag(middleTile, x.Item1, x.Item2, blockingLayer, "Player");
            if (isGround && (!isBlockingLayer || isPlayer))
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
    */
    /*
    public override void CreateGrid()
    {
        transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        this.AvailableTiles.Add(new List<GameObject>());
        AddSurroundingsToList(transform.gameObject, 0);
    }
    public override void DisableGrid()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = false;
                tile.GetComponent<HighlightTile>().SetHighlightBool(false);
            }
        }
    }
    */
    /*
    public void HighlightAll()
    {
        foreach (List<GameObject> MovementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in MovementTileList)
            {
                tile.GetComponent<HighlightTile>().SetHighlightBool(true);
                tile.GetComponent<HighlightTile>().activeState = actionStateName;
                tile.GetComponent<HighlightTile>().ChangeBaseColor();
                tile.GetComponent<HighlightTile>().canAbilityTargetAllies = true;
            }
        }
    }
    */
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (characterBeingBlocked != null)
        {
            if (DoesCharacterHaveBlessing("Sense of safety"))
            {
                int randomHeal = Random.Range(3, 4);
                bool crit = IsItCriticalStrike(ref randomHeal);
                characterBeingBlocked.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
            }
            characterBeingBlocked.GetComponent<PlayerInformation>().BlockingAlly = null;
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", false);
            GetComponent<PlayerInformation>().Blocker = false;
        }
    }

    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spellToBool");
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", true);
            GetSpecificGroundTile(position).GetCurrentCharacter().GetComponent<PlayerInformation>().BlockingAlly = transform.gameObject;
            characterBeingBlocked = GetSpecificGroundTile(position).GetCurrentCharacter();
            GetComponent<PlayerInformation>().Blocker = true;
            FinishAbility();
        }
    }
    public bool CanTileBeClicked(GameObject tile)//negalima blokuoti, jei esi blokuojamas
    {
        if (GetComponent<PlayerInformation>().BlockingAlly == null && CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player")
            && isAllegianceSame(tile))
        {
            return true;
        }
        else return false;
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "BLOCK");
    }
    public override GameObject PossibleAIActionTile()
    {
        if (CanGridBeEnabled())
        {
            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(1);

            List<GameObject> allyCharacterList = new List<GameObject>();

            foreach (GameObject character in characterList)
            {
                //if (character != gameObject && canTileBeClicked(character) && character.GetComponent<PlayerInformation>().health < character.GetComponent<PlayerInformation>().MaxHealth)
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
        if (DoesCharacterHaveBlessing("Block from afar"))
        {
            AttackRange++;
        }
    }
    public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        BlockAbility ability = spawnedCharacter.AddComponent<BlockAbility>();
        ability.actionStateName = this.actionStateName;
        ability.AttackRange = this.AttackRange;
        ability.AbilityCooldown = this.AbilityCooldown;
        ability.minAttackDamage = this.minAttackDamage;
        ability.maxAttackDamage = this.maxAttackDamage;
        ability.isAbilitySlow = this.isAbilitySlow;
        ability.friendlyFire = this.friendlyFire;

        //Ir pabuffinu
        if (blessings.Find(x => x.blessingName == "Block from afar") != null)
        {
            ability.AttackRange++;
        }

        return ability;
    }
}

