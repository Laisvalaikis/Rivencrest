using UnityEngine;

public class BlockAbility : BaseAction
{
    private GameObject _characterBeingBlocked;
    
    void Start()
    {
        AttackHighlight = new Color32(123,156, 178,255);
        AttackHighlightHover = new Color32(103, 136, 158, 255);
        CharacterOnGrid = new Color32(146, 212, 255, 255);
        isAbilitySlow = false;
    }
    
    protected override void SetNonHoveredAttackColor(ChunkData chunkData)
    {
        GameObject character = chunkData.GetCurrentCharacter();
        HighlightTile tileHighlight = chunkData.GetTileHighlight();
        if (character != null && IsAllegianceSame(chunkData.GetPosition()))
        {
            tileHighlight.SetHighlightColor(CharacterOnGrid);
        }
        else
        {
            tileHighlight.SetHighlightColor(AttackHighlight);
        }
    }
    
    protected override void SetHoveredAttackColor(ChunkData chunkData)
    {
        GameObject character = chunkData.GetCurrentCharacter();
        HighlightTile tileHighlight = chunkData.GetTileHighlight();

        if (character != null && IsAllegianceSame(chunkData.GetPosition()))
        {
            tileHighlight.SetHighlightColor(AttackHoverCharacter);
        }
        else
        {
            tileHighlight.SetHighlightColor(AttackHighlightHover);
        }
    }
    
    protected override void HighlightGridTile(ChunkData chunkData)
    {
        if(chunkData.GetCurrentCharacter()!=GameTileMap.Tilemap.GetCurrentCharacter())
        {
            chunkData.GetTileHighlight().ActivateColorGridTile(true);
            SetNonHoveredAttackColor(chunkData);
            _chunkList.Add(chunkData);
        }
    }
    public override void OnTurnStart()
    {
        if (_characterBeingBlocked != null)
        {
            if (DoesCharacterHaveBlessing("Sense of safety"))
            {
                int randomHeal = Random.Range(3, 5);
                bool crit = IsItCriticalStrike(ref randomHeal);
                _characterBeingBlocked.GetComponent<PlayerInformation>().Heal(randomHeal, crit);
            }
            _characterBeingBlocked.GetComponent<PlayerInformation>().BlockingAlly = null;
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
        PlayerInformation playerInformationLocal = chunk.GetCurrentPlayerInformation();
        if(playerInformationLocal!=null)
            playerInformationLocal.BlockingAlly = GameTileMap.Tilemap.GetCurrentCharacter();
        _characterBeingBlocked = chunk.GetCurrentCharacter();
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

