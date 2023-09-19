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
                Vector3 chunkPosition = chunk.GetPosition();
                //tile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("cometFall");
                //Enemy
                if (CheckIfSpecificTag(chunkPosition, 0, 0, blockingLayer, "Player") && !IsAllegianceSame(chunkPosition))
                {
                    DealRandomDamageToTarget(chunk, MinAttackDamage, MaxAttackDamage);
                }
                //Ally
                else if (CheckIfSpecificTag(chunkPosition, 0, 0, blockingLayer, "Player") && IsAllegianceSame(chunkPosition))
                {
                    DealRandomDamageToTarget(chunk, MinAttackDamage/3, MaxAttackDamage/3);
                }
                //tile.transform.Find("mapTile").Find("CometZone").gameObject.SetActive(false);
            }
            _damageTiles.Clear();
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        base.ResolveAbility(position);
        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell1"); //CometFallStart animation
        _damageTiles.Clear();
        if (CheckIfSpecificLayer(position,0, 0, groundLayer))
        {
            _damageTiles.Add(GameTileMap.Tilemap.GetChunk(position));
            //GetSpecificGroundTile(clickedTile, 0, 0, groundLayer).transform.Find("mapTile").Find("CometZone").gameObject.SetActive(true);
        }
        FinishAbility();
    }
    protected override bool CanTileBeClicked(Vector3 position)
    {
        return CheckIfSpecificLayer(position, 0, 0, groundLayer);
    }
}
