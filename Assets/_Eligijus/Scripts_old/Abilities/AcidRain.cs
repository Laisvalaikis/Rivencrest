using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AcidRain : BaseAction
{
    void Start()
    {
        actionStateName = "AcidRain";
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile.transform.position))
                {
                    GameObject target = GetSpecificGroundTile(gameObject, 0, 0, blockingLayer);
                    target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
                    tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("forest3");//¯\_(ツ)_/¯
                }
            }
            FinishAbility();
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableTextPreview(tile, MergedTileList, "POISON");
    }
    
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> EnemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile.transform.position))
                {
                    GameObject character = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    EnemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (EnemyCharacterList.Count > 1 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        else if (EnemyCharacterList.Count > 0 && actionChanceNumber <= 40)
        {
            return GetSpecificGroundTile(EnemyCharacterList[Random.Range(0, EnemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
