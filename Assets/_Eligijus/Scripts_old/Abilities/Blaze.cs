using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blaze : BaseAction
{
    //public int minAttackDamage = 1;
    //public int maxAttackDamage = 3;
    public int bonusDamage = 4;



    void Start()
    {
        actionStateName = "Blaze";
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            GameObject target = GetSpecificGroundTile(position).GetCurrentCharacter();
            bool aflame = target.GetComponent<PlayerInformation>().Aflame != null;
            if (!aflame)
            {
                target.GetComponent<PlayerInformation>().Aflame = gameObject;
            }
            else
            {
                TriggerAflame(target);
                GetSpecificGroundTile(target, 0, 0, groundLayer).transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("orange3");
            }
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            if(aflame)
                DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            else DealRandomDamageToTarget(target, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);

            //target.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetBool("aflame", true);
            //target.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetTrigger("start");
            FinishAbility();
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
        bool aflame = target.GetComponent<PlayerInformation>().Aflame != null;
        if (aflame)
            EnableDamagePreview(tile, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        else EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile);
    }
    /*
    public override void OnTurnStart()
    */
    public override void TriggerAflame(GameObject aflameCharacter) //Explode
    {
        if (aflameCharacter != null && aflameCharacter.GetComponent<PlayerInformation>().Aflame != null && aflameCharacter.GetComponent<PlayerInformation>().health > 0)
        {
            var directionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
            aflameCharacter.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetTrigger("explode");
            aflameCharacter.transform.Find("VFX").Find("Aflame").GetComponent<Animator>().SetBool("aflame", false);
            int randomDamage;
            bool crit;
            foreach (var x in directionVectors)
            {
                bool isPlayer = CheckIfSpecificTag(aflameCharacter, x.Item1, x.Item2, blockingLayer, "Player");
                if (isPlayer)
                {
                    ChunkData target = GetSpecificGroundTile(new Vector3(x.Item1, x.Item2, 0));
                    
                    if (isAllegianceSame(aflameCharacter, target.GetCurrentCharacter(), blockingLayer))
                    {
                        randomDamage = Random.Range(minAttackDamage, maxAttackDamage);
                        crit = IsItCriticalStrike(ref randomDamage);
                        dodgeActivation(ref randomDamage, target.GetCurrentPlayerInformation());
                        target.GetCurrentPlayerInformation().DealDamage(randomDamage, crit, gameObject);
                    }
                }
            }

            PlayerInformation playerInformation = aflameCharacter.GetComponent<PlayerInformation>();
            randomDamage = Random.Range(minAttackDamage, maxAttackDamage);
            crit = IsItCriticalStrike(ref randomDamage);
            dodgeActivation(ref randomDamage, playerInformation);
            playerInformation.DealDamage(randomDamage, crit, gameObject);
            playerInformation.Aflame = null;
        }
    }
    public override GameObject PossibleAIActionTile()
    {
        if (CanGridBeEnabled())
        {

            List<GameObject> characterList = GetComponent<AIBehaviour>().GetCharactersInGrid(AttackRange);

            List<GameObject> enemyCharacterList = new List<GameObject>();

            foreach (GameObject character in characterList)
            {
                if (!isAllegianceSame(character))
                {
                    enemyCharacterList.Add(character);
                }
            }
            if (enemyCharacterList.Count > 0)
            {
                return enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)];
            }
        }

        return null;
    }
}
