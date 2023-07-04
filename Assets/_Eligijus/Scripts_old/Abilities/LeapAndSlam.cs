using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeapAndSlam : BaseAction
{
    //public int minAttackDamage = 4;
    //public int maxAttackDamage = 5;

    void Start()
    {
        actionStateName = "LeapAndSlam";
        isAbilitySlow = false;
    }

    public override void CreateGrid()
    {
        base.CreateGrid();
       //HighlightOuter
        /* for (int i = AvailableTiles.Count - 2; i >= 0; i--)
        {
            foreach (GameObject tile in AvailableTiles[i])
            {
                MergedTileList.Remove(tile);
            }
        }*/
        MergedTileList.RemoveAll(tile => !canTileBeClicked(tile));
    }

    public override void ResolveAbility(GameObject clickedTile)
    {
        
        if (canTileBeClicked(clickedTile))
        {   
            base.ResolveAbility(clickedTile);
            DealDamageToAdjacent(clickedTile);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("playerChop");
            transform.position = clickedTile.transform.position + new Vector3(0f, 0f, -1f);
            gameInformation.FocusSelectedCharacter(gameObject);

            FinishAbility();
        }
    }

    private void DealDamageToAdjacent(GameObject center)
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
                GetSpecificGroundTile(center, x.Item1, x.Item2, groundLayer).transform.Find("mapTile").Find("VFXImpactBelow").gameObject.GetComponent<Animator>().SetTrigger("burgundy3");
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

    public override bool canTileBeClicked(GameObject tile)
    {
        if (!CheckIfSpecificLayer(tile, 0, 0, blockingLayer))
        {
            return true;
        }
        return false;
    }

    public override void OnTileHover(GameObject tile)
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
                if (canTileBeClicked(tile) && !CheckIfSpecificTag(tile, 0, 0, blockingLayer, "Wall"))
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
