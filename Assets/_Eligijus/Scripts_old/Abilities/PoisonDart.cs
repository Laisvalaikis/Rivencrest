using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoisonDart : BaseAction
{
    public string ImpactName = "forest3";
    private List<GameObject> PoisonTiles = new List<GameObject>();
    //public int minAttackDamage = 5;
    //public int maxAttackDamage = 7;



    void Start()
    {
        actionStateName = "PoisonDart";
        isAbilitySlow = false;
    }
    public override void ResolveAbility(Vector3 position)
    {
        
        if (CanTileBeClicked(position))
        {
            base.ResolveAbility(position);
            transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");
            //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell2");

            GameObject target = GetSpecificGroundTile(position);
            DealRandomDamageToTarget(target, minAttackDamage, maxAttackDamage);
            target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
            //clickedTile.transform.Find("mapTile").Find("VFX9x9Upper").gameObject.GetComponent<Animator>().SetTrigger("crowAttack");
            //clickedTile.transform.Find("mapTile").Find("VFXImpactUpper").gameObject.GetComponent<Animator>().SetTrigger(ImpactName);
            if (DoesCharacterHaveBlessing("Explosive dart"))
            {
                PoisonAdjacent(target);
            }

            FinishAbility();
            StartCoroutine(ExecuteAfterTime(0.1f, () =>
            {
                //OffTileHover(clickedTile);

            }));
        }
    }

    private void PoisonAdjacent(GameObject center)
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
                    target.GetComponent<PlayerInformation>().Poisons.Add(new PlayerInformation.Poison(gameObject, 2, 2));
                }
            }
        }
    }
    private void CreatePoisonTileList(GameObject centerTile)
    {
        PoisonTiles.Clear();
        var spellDirectionVectors = new List<(int, int)>
        {
            //(0, 0),
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };
        foreach (var x in spellDirectionVectors)
        {
            if (CheckIfSpecificLayer(centerTile, x.Item1, x.Item2, groundLayer) && (!CheckIfSpecificLayer(centerTile, x.Item1, x.Item2, blockingLayer) || CheckIfSpecificTag(centerTile, x.Item1, x.Item2, blockingLayer, "Player")))
            {
                PoisonTiles.Add(GetSpecificGroundTile(centerTile, x.Item1, x.Item2, groundLayer));
            }
        }
    }
    public override void OnTileHover(GameObject tile)
    {
        print("on tile hover");
        EnableDamagePreview(tile, minAttackDamage, maxAttackDamage);
        if(DoesCharacterHaveBlessing("Explosive dart"))
        {
            CreatePoisonTileList(tile);
            foreach(GameObject tileInList in PoisonTiles)
            {
                tileInList.transform.Find("mapTile").Find("Highlight").gameObject.SetActive(true);
            }
        }
    }
    public override void OffTileHover(GameObject tile)
    {
        print("off tile hover");
        DisablePreview(tile);
        if (DoesCharacterHaveBlessing("Explosive dart"))
        {
            //tile.transform.Find("mapTile").Find("Direction").gameObject.SetActive(false);
            foreach (GameObject tileInList in PoisonTiles)
            {
                DisablePreview(tileInList);
            }
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
                if (CanTileBeClicked(tile.transform.position))
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
    }
}
