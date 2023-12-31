using System.Collections.Generic;
using UnityEngine;

public class CometFall : BaseAction
{
    private const int MinAttackDamage = 8;
    private const int MaxAttackDamage = 10;
    private List<ChunkData> _damageTiles = new List<ChunkData>();
    public override void OnTurnStart()
    {
        if (_damageTiles.Count > 0)
        {
            foreach (ChunkData chunk in _damageTiles)
            {
                //tile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("cometFall");
                //Enemy
                if (CheckIfSpecificInformationType(chunk, InformationType.Player) && !IsAllegianceSame(chunk))
                {
                    DealRandomDamageToTarget(chunk, MinAttackDamage, MaxAttackDamage);
                }
                //Ally
                else if (CheckIfSpecificInformationType(chunk, InformationType.Player) && IsAllegianceSame(chunk))
                {
                    DealRandomDamageToTarget(chunk, MinAttackDamage/3, MaxAttackDamage/3);
                }
                //tile.transform.Find("mapTile").Find("CometZone").gameObject.SetActive(false);
            }
            _damageTiles.Clear();
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1"); //CometFallStart animation
        _damageTiles.Clear();
        _damageTiles.Add(chunk);
        //GetSpecificGroundTile(clickedTile, 0, 0, groundLayer).transform.Find("mapTile").Find("CometZone").gameObject.SetActive(true);
        FinishAbility();
    }

    public override bool CanTileBeClicked(ChunkData chunk)
    {
        return true; //might be bullshit
    }
}
