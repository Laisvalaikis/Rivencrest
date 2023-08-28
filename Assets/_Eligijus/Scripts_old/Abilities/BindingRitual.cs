using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BindingRitual : BaseAction
{
    //private string actionStateName = "CrowAttack";
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;




    void Start()
    {
        actionStateName = "BindingRitual";
    }

    public override void CreateGrid()
    {
        // transform.gameObject.GetComponent<PlayerInformation>().currentState = actionStateName;
        this.AvailableTiles.Clear();
        if (AttackRange > 0)
        {
            this.AvailableTiles.Add(new List<GameObject>());
            //AddSurroundingsToList(transform.gameObject, 0);
        }

        for (int i = 1; i <= AttackRange - 1; i++)
        {
            this.AvailableTiles.Add(new List<GameObject>());

            foreach (var tileInPreviousList in this.AvailableTiles[i - 1])
            {
               // AddSurroundingsToList(tileInPreviousList, i);
            }
        }
        //Merging into one list
        MergedTileList.Clear();
        foreach (List<GameObject> movementTileList in this.AvailableTiles)
        {
            foreach (GameObject tile in movementTileList)
            {
                if (!MergedTileList.Contains(tile))
                {
                    MergedTileList.Add(tile);
                }
            }
        }
        //RemoveInner
        for (int i = this.AvailableTiles.Count - 1; i > 0; i--)
        {
            foreach (GameObject tile in this.AvailableTiles[i - 1])
            {
                MergedTileList.Remove(tile);
            }
        }
        if (CheckIfSpecificLayer(gameObject, 0, 0, groundLayer))
        {
            MergedTileList.Remove(GetSpecificGroundTile(gameObject, 0, 0, groundLayer));
        }
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
                if (base.CanTileBeClicked(tile.transform.position))
                {
                    GameObject target = GetSpecificGroundTile(tile, 0, 0, blockingLayer);
                    DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
                    target.GetComponent<PlayerInformation>().ApplyDebuff("IceSlow");
                }
                //if (tile != GetSpecificGroundTile(gameObject, 0, 0, groundLayer))
                //{
                tile.transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("undead6");
                //tile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger("undead1");
                // }
            }
            FinishAbility();
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        EnableDamagePreview(tile, MergedTileList, minAttackDamage, maxAttackDamage);
    }
    public override void OffTileHover(GameObject tile)
    {
        DisablePreview(tile, MergedTileList);
    }
    public override GameObject PossibleAIActionTile()
    {
        List<GameObject> enemyCharacterList = new List<GameObject>();
        if (CanGridBeEnabled())
        {
            CreateGrid();
            foreach (GameObject tile in MergedTileList)
            {
                if (CanTileBeClicked(tile.transform.position))
                {
                    GameObject character = GetSpecificGroundTile(tile.transform.position).GetCurrentCharacter();
                    enemyCharacterList.Add(character);
                }
            }
        }
        int actionChanceNumber = UnityEngine.Random.Range(0, 100); //ar paleist spella ar ne
        if (enemyCharacterList.Count > 1 && actionChanceNumber <= 100)
        {
            return GetSpecificGroundTile(enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        else if (enemyCharacterList.Count > 0 && actionChanceNumber <= 45)
        {
            return GetSpecificGroundTile(enemyCharacterList[Random.Range(0, enemyCharacterList.Count - 1)], 0, 0, groundLayer);
        }
        return null;
    }
}
