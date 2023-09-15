using System.Collections.Generic;
using UnityEngine;

public class BlockAbility : BaseAction
{
    private GameObject characterBeingBlocked;
    
    void Start()
    {
        actionStateName = "Block";
        isAbilitySlow = false;
    }
    
    protected override void HighlightGridTile(ChunkData chunkData)
    {
        if(chunkData.GetCurrentCharacter()!=GameTileMap.Tilemap.GetCurrentCharacter())
        {
            chunkData.GetTileHighlight().ActivateColorGridTile(true);
            _chunkList.Add(chunkData);
        }
    }
    public override void OnTurnStart()
    {
        if (characterBeingBlocked != null)
        {
            if (DoesCharacterHaveBlessing("Sense of safety"))
            {
                int randomHeal = Random.Range(3, 5);
                bool crit = IsItCriticalStrike(ref randomHeal);
                characterBeingBlocked.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
            }
            characterBeingBlocked.GetComponent<PlayerInformation>().BlockingAlly = null;
            //transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", false);
            GetComponent<PlayerInformation>().Blocker = false;
        }
    }

    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spellToBool");
        //transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", true);
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(position);
        PlayerInformation playerInformation = chunk.GetCurrentPlayerInformation();
        if(playerInformation!=null)
            playerInformation.BlockingAlly = GameTileMap.Tilemap.GetCurrentCharacter();
        characterBeingBlocked = chunk.GetCurrentCharacter();
        GetComponent<PlayerInformation>().Blocker = true;
        FinishAbility();
    }

    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, "BLOCK");
    }
    
    public override void BuffAbility()
    {
        if (DoesCharacterHaveBlessing("Block from afar"))
        {
            AttackRange++;
        }
    }
    /*public override BaseAction GetBuffedAbility(List<Blessing> blessings)
    {
        //Sukuriu kopija
        BlockAbility ability = new BlockAbility();
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
    }*/
}

