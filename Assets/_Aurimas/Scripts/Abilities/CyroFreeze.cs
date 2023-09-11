using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyroFreeze : BaseAction
{
   private PlayerInformation _playerInformation;
   private bool isAbilityActive = false;

   public override void CreateGrid(ChunkData centerChunk, int radius)
   {
      _chunkList.Clear();
      _chunkList.Add(centerChunk);
      HighlightGridTile(centerChunk);
   }
   public override void CreateGrid()
   {
      ChunkData startChunk = GameTileMap.Tilemap.GetChunk(transform.position);
      CreateGrid(startChunk, AttackRange);
   }

   public override void OnTurnStart()
   {
      if (isAbilityActive && (GetComponent<PlayerInformation>().health > 0))
      {
         StartCoroutine(ExecuteAfterTime(0.4f, () =>
         {
            var pushDirectionVectors = new List<(int, int)>
            {
               (AttackRange, 0),
               (0, AttackRange),
               (-AttackRange, 0),
               (0, -AttackRange)
            };
            foreach (var x in pushDirectionVectors)
            {
               if (CheckIfSpecificTag(gameObject, x.Item1, x.Item2, blockingLayer, "Player"))
               {
                  ChunkData chunkData = GameTileMap.Tilemap.GetChunk(transform.position);
                  if (isAllegianceSame(chunkData.GetPosition()))
                  {
                     DealRandomDamageToTarget(chunkData, minAttackDamage / 2, maxAttackDamage / 2);
                  }
                  else
                  {
                     DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
                  }
               }
            }
         }));
      }
      isAbilityActive = false;
   }
   public override void ResolveAbility(Vector3 position)
   {
      base.ResolveAbility(position);
      isAbilityActive = true;
      _playerInformation.Stasis = true;
      FinishAbility();
   }
   public override void OnTileHover(GameObject tile)
   {
      EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
   }
    
   public override void OffTileHover(GameObject tile)
   {
      DisablePreview(tile, MergedTileList);
   }
}
