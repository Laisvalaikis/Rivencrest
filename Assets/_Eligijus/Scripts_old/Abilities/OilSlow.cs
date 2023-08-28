using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OilSlow : BaseAction
{
    private GameObject SlowedTarget = null;
    //public int minAttackDamage = 2;
    //public int maxAttackDamage = 3;


    void Start()
    {
        actionStateName = "OilSlow";
        isAbilitySlow = false;
    }
    public override void OnTurnStart()//pradzioj ejimo
    {
        if (SlowedTarget != null)
        {
            SlowedTarget = null;
        }
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            GameObject target = GetSpecificGroundTile(position).GetCurrentCharacter();
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            GetSpecificGroundTile(position).GetCurrentCharacter().GetComponent<PlayerInformation>().ApplyDebuff("OilSlow");
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("lime1");
            FinishAbility();

        }
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();

            foreach (GameObject tile in MergedTileList)
            {
                if (CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Player"))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    if (!isAllegianceSame(character) && CanTileBeClicked(tile.transform.position))
                    {
                        EnemyCharacterList.Add(character);
                    }
                }
            }
        }

        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
