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
    
    //might be bullshit now
    protected override void SetNonHoveredAttackColor(ChunkData chunkData)
    {
        HighlightTile tileHighlight = chunkData.GetTileHighlight();
        if (chunkData.CharacterIsOnTile() && IsAllegianceSame(chunkData))
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

        if (character != null && IsAllegianceSame(chunkData))
        {
            tileHighlight.SetHighlightColor(AttackHoverCharacter);
            EnableDamagePreview(chunkData,"BLOCK");
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
        }
    }
    public override bool CanTileBeClicked(ChunkData chunk)
    {
        return IsAllegianceSame(chunk);
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
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spellToBool");
            //transform.Find("CharacterModel").GetComponent<Animator>().SetBool("block", true);
            PlayerInformation playerInformationLocal = chunk.GetCurrentPlayerInformation();
            if (playerInformationLocal != null)
                playerInformationLocal.BlockingAlly = GameTileMap.Tilemap.GetCurrentCharacter();
            _characterBeingBlocked = chunk.GetCurrentCharacter();
            GetComponent<PlayerInformation>().Blocker = true;
            FinishAbility();
        }
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

