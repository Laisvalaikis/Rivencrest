using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FromTheShadows : BaseAction
{
    public string ImpactName = "red1";
    //public int minAttackDamage = 3;
    //public int maxAttackDamage = 4;



    void Start()
    {
        actionStateName = "FromTheShadows";
        //isAbilitySlow = false;
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            //DamageAdjacent(clickedTile);
            transform.position = position + new Vector3(0f, 0f, -1f);
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger(ImpactName);        
            FinishAbility();
        }
    }
    public bool CanTileBeClicked(Vector3 position)
    {
        return CheckIfSpecificLayer(position, 0, 0, groundLayer) && !CheckIfSpecificLayer(position, 0, 0, blockingLayer);
    }
    private void DamageAdjacent(GameObject center)
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
            if (CheckIfSpecificLayer(center, x.Item1, x.Item2, groundLayer)) //animation on ground
            {
                GetSpecificGroundTile(center, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger(ImpactName);
            }
            if (CheckIfSpecificTag(center, x.Item1, x.Item2, blockingLayer, "Player"))
            {
                GameObject target = GetSpecificGroundTile(center, x.Item1, x.Item2, blockingLayer);

                if (!isAllegianceSame(target))
                {
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                }
            }
        }
    }
    /*public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (canGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (canTileBeClicked(tile))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }*/
}
