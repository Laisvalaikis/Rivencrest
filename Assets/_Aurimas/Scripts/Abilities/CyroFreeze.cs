using System.Collections.Generic;
using UnityEngine;

public class CyroFreeze : BaseAction
{
   private PlayerInformation _playerInformation;
   private bool _isAbilityActive = false;

   public override void CreateAvailableChunkList(int attackRange)
   {
      base.CreateAvailableChunkList(attackRange);
      _chunkList.Add(GameTileMap.Tilemap.GetChunk(transform.position));
   }
   public override void OnTurnStart()
   {
      if (_isAbilityActive && (GetComponent<PlayerInformation>().health > 0))
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
               if (CheckIfSpecificInformationType(GetSpecificGroundTile(transform.position), InformationType.Player)) //wrong
               {
                  ChunkData chunkData = GameTileMap.Tilemap.GetChunk(transform.position);
                  if (IsAllegianceSame(chunkData))
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
      _isAbilityActive = false;
   }
   public override void ResolveAbility(Vector3 position)
   {
      base.ResolveAbility(position);
      _isAbilityActive = true;
      _playerInformation.Stasis = true;
      FinishAbility();
   }
}
