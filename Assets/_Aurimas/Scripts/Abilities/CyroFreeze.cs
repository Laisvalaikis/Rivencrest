using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyroFreeze : BaseAction
{
   private PlayerInformation _playerInformation;
   private bool isAbilityActive = false;

   public override void OnTurnStart() //pradzioj ejimo
   {
      if (isAbilityActive && (GetComponent<PlayerInformation>().health > 0))
      {
         StartCoroutine(ExecuteAfterTime(0.2f, () =>
         {
            transform.Find("CharacterModel").GetComponent<Animator>().SetBool("stasis", false);
            GetComponent<PlayerInformation>().Stasis = false;
         }));
         //
         StartCoroutine(ExecuteAfterTime(0.4f, () =>
         {
            var pushDirectionVectors = new List<(int, int)>
            {
               (1, 0),
               (0, 1),
               (-1, 0),
               (0, -1)
            };
            foreach (var x in pushDirectionVectors)
            {
               if (CheckIfSpecificLayer(gameObject, x.Item1, x.Item2, groundLayer)) //animation on ground
               {
                  GetSpecificGroundTile(gameObject, x.Item1, x.Item2, groundLayer).transform.Find("mapTile")
                     .Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("white1");
               }

               if (CheckIfSpecificTag(gameObject, x.Item1, x.Item2, blockingLayer, "Player"))
               {
                  // GameObject target = GetSpecificGroundTile(gameObject, x.Item1, x.Item2, blockingLayer);
                  ChunkData target =
                     GameTileMap.Tilemap.GetChunk(gameObject.transform.position + new Vector3(0, 0.5f, 0));
                  if (isAllegianceSame(target.GetPosition()))
                  {
                     DealRandomDamageToTarget(target, minAttackDamage / 2, maxAttackDamage / 2);
                  }
                  else
                  {
                     DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                  }
               }
            }
         }));
      }
   }

   public override void ResolveAbility(Vector3 position)
   {
      base.ResolveAbility(position);
      isAbilityActive = true;
      _playerInformation.Stasis = true;
      FinishAbility();
   }
}
